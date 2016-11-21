using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConsoleServer.Configuration
{
    public enum DataType { All, Images, Executable, Files}

    public class DataFolderElement: ConfigurationElement
    {
        [ConfigurationProperty("dataType", DefaultValue = DataType.All, IsKey = true)]
        public DataType DataType
        {
            get { return ((DataType)(base["dataType"])); }
            set { base["dataType"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = true, IsKey = false)]
        public string Path
        {
            get { return ((string)(base["path"])); }
            set { base["path"] = value; }
        }
    }
}
