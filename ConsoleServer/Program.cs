using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using System.ServiceModel;
using Contracts;

namespace ConsoleServer
{
    class Program
    {
        private static readonly string _logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        static void Main()
        {
            ChatService chat = new ChatService(System.Configuration.ConfigurationManager.AppSettings["DataFolder"]);
            chat.Log += Console.WriteLine;
            if (_logFilePath != null || _logFilePath != string.Empty)
                chat.Log += (s) => { File.AppendAllText(_logFilePath, s + Environment.NewLine); };
            using (ServiceHost host = new ServiceHost(chat))
            {
                //host.Description.Behaviors.Add(new BehaviorWithExceptions());  // work only with basicHttp???
                string server_input = "";
                host.Opened += chat.host_Opened;
                host.Closing += chat.host_Closing;
                host.Open();
                Console.WriteLine("ChatServer started");
                Console.WriteLine("Enter '/stop' to close server");
                while (server_input.ToLower() != "/stop")
                {
                    if (server_input != String.Empty)
                        chat.SendMessage(new Message
                        {
                            Author = "SERVER",
                            Text = server_input,
                            Recievers = chat.UserNames
                        });
                    server_input = Console.ReadLine(); 
                }
            }
        }


    }
}
