using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ConsoleServer
{
    public class ChatErrorHandler: IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            Console.WriteLine(new string('-', 30));
            Console.WriteLine(string.Format("ERROR: {0}\n{1}", error.Message, error.StackTrace));
            Console.WriteLine(new string('-', 30));
            return true;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            if (error is FaultException)
                return;

            var er = new FaultException(error.Message, new FaultCode("ServerException"));
            fault = Message.CreateMessage(version, er.CreateMessageFault(), "Error occured on server");
        }
    }
}
