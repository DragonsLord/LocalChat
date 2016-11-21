using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Message
    {
        public string Text;

        public string Author;

        public IEnumerable<string> Recievers;

        public bool IsDataAttached;

        public List<DataLink> DataLinks;
    }
}
