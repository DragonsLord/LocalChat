using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using Contracts;

namespace ConsoleServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class ChatService : IChatProvider
    {
        private readonly string data_folder;
        private MemoryStream _dataCache;

        public event Action<string> Log;
        private Dictionary<string, IContractClient> users = new Dictionary<string, IContractClient>();

        public IEnumerable<string> UserNames { get { return users.Keys; } }

        public ChatService(string data_folder)
        {
            this.data_folder = data_folder;
        }

        public void RegisterNewUser(string user_name)
        {
            if (users.ContainsKey(user_name))
                throw new FaultException<ExceptionType>(ExceptionType.UserAlreadyRegisterd);
            
            SendMessage(new Message { 
                Author="SYSTEM", 
                Text = String.Format("{0} enter the room", user_name), 
                Recievers = UserNames});

            users.Values.AsParallel<IContractClient>().ForAll(user => user.NewUserRegistered(user_name));
            
            var user_callback = OperationContext.Current.GetCallbackChannel<IContractClient>();
            users[user_name] = user_callback;

            Log(String.Format("[User {0} registered]", user_name));
        }

        public void UnregisterUser(string user_name)
        {
            users.Remove(user_name);
            users.Values.AsParallel<IContractClient>().ForAll(user => user.UserUnregistered(user_name));

            Log(String.Format("[User {0} unregistered]", user_name));
            SendMessage(new Message
            {
                Author = "SYSTEM",
                Text = String.Format("{0} leave the room", user_name),
                Recievers = UserNames
            });
        }

        public IEnumerable<string> GetUsersList()
        {
            return UserNames;
        }

        public async void SendMessage(Message message)
        {
            if (message.IsDataAttached)
                message.DataLinks = await Task<List<string>>.Run(() => { return RecieveData(message); });

            if (message.Author != "SYSTEM")
                Log(String.Format("{0}: {1}", message.Author, message.Text));

            var recievers = (from user in users where message.Recievers.Contains(user.Key) select user.Value).ToList();
            if (users.Count() != 0 && recievers.Count == 0)
            {
                Log(String.Format("User {0} use Empty or Unknown recievers list", message.Author));
            //  throw new FaultException<ExceptionType>(ExceptionType.EmptyOrUnknownRecieverList);
            }
            else recievers.AsParallel<IContractClient>().ForAll(user => user.Write(message));
            #region Old One Thread Way
            //bool empty = true;
            //foreach (var user in message.Recievers)
            //{
            //    if (users.ContainsKey(user))
            //    {
            //        empty = false;
            //        users[user].Write(message);
            //    }
            //}
            //if (empty && users.Count() != 0) throw new FaultException<ExceptionType>(ExceptionType.EmptyOrUnknownRecieverList);
            #endregion
        }

        public void host_Closing(object sender, EventArgs e)
        {
            foreach (var user in users.Values)
                user.Disconnect();

            Log(String.Format("{0}: Closed", DateTime.Now));
        }

        public void host_Opened(object sender, EventArgs e)
        {
            Log(String.Format("{0}: Opened", DateTime.Now));
        }

        private List<DataLink> RecieveData(Message message)
        {
            List<DataLink> links = new List<DataLink>();
            for (int i = 0; i < message.DataLinks.Count; i++)
            {
                DataLink current = message.DataLinks[i];
                current.Adress = String.Format("({0}){1}", DateTime.Now.ToFileTime(), current.FileName);
                Log(String.Format("Data uploading by {0}: {1}", message.Author, current.Adress));
                byte[] buffer;
                using (var write = File.OpenWrite(Path.Combine(data_folder, current.Adress)))
                {
                    try
                    {
                        while ((buffer = users[message.Author].Upload(5000, 0)) != null)
                            write.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                        Log(String.Format("ERROR: failed to save {0} from user {1}", message.DataLinks[i].FileName, message.Author));
                        //throw new FaultException<ExceptionType>(ExceptionType.ErrorDuringDataTransfer);
                    }
                }
                links.Add(current);
            }
            return links;
        }

        public byte[] DownloadData(string file_adress, int buffer_size)
        {
            if (_dataCache == null)
            {
                try
                {
                    _dataCache = new MemoryStream(File.ReadAllBytes(Path.Combine(data_folder, file_adress)));
                }
                catch(Exception)
                {
                    Log(String.Format("ERROR: file {0} not found", file_adress));
                    throw new FaultException<ExceptionType>(ExceptionType.FileNotFound);
                }
            }
            try
            {
                byte[] buffer = new byte[buffer_size];
                if (_dataCache.Read(buffer, 0, buffer_size) > 0)
                    return buffer;
                else
                {
                    _dataCache.Close();
                    _dataCache = null;
                    return null;
                }
            }
            catch (Exception)
            {
                Log(String.Format("ERROR: failed to transfer {0}", file_adress));
                throw new FaultException<ExceptionType>(ExceptionType.ErrorDuringDataTransfer);
            }
        }
    }
}
