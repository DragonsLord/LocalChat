using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleServer.Configuration
{
    public class FolderConfigSection: ConfigurationSection
    {
        [ConfigurationProperty("DataFolders")]
        public DataFoldersCollection FolderItems
        {
            get { return ((DataFoldersCollection)(base["DataFolders"])); }
        }
        [ConfigurationProperty("LogFiles")]
        public LogFilesCollection LogFiles
        {
            get { return ((LogFilesCollection)(base["LogFiles"])); }
        }
    }
}
