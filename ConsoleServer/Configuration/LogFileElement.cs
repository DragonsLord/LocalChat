using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConsoleServer.Configuration
{
    public class LogFileElement: ConfigurationElement
    {
        [ConfigurationProperty("path", DefaultValue = "log.txt" ,IsKey = false, IsRequired = false)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }

    }
}
