using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ConsoleServer.Configuration
{
    [ConfigurationCollection(typeof(DataFolderElement), AddItemName = "DataFolder")]
    public class DataFoldersCollection: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataFolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataFolderElement)(element)).DataType;
        }

        public DataFolderElement this[int id]
        {
            get { return (DataFolderElement)BaseGet(id); }
        }
    }

    [ConfigurationCollection(typeof(LogFileElement), AddItemName = "LogFile")]
    public class LogFilesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogFileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogFileElement)(element)).Path;
        }

        public LogFileElement this[int id]
        {
            get { return (LogFileElement)BaseGet(id); }
        }
    }
}
