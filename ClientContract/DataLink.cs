using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class DataLink
    {
        [DataMember]
        public string FileName;
        [DataMember]
        public long Length;
        [DataMember]
        public string Adress;
    }
}
