using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using ChatClient.ChatService;

namespace ChatClient.ChatService
{
    public class FileData: IDisposable
    {
        private MemoryStream _stream;
        private long _size;

        private string _name;

        public FileData(string path)
        {
            _name = System.IO.Path.GetFileName(path);
            _stream =  new MemoryStream(File.ReadAllBytes(path));
            _size = _stream.Length;
        }

        public DataLink GetLink()
        {
            return new DataLink()
            {
                FileName = _name,
                Length = _size
            };
        }

        public static implicit operator DataLink(FileData f)
        {
            return f.GetLink();
        }

        public int ReadBytes(byte[] buffer, int buffer_size)
        {
            return _stream.Read(buffer, 0, buffer_size);
        }

        public void CloseStream()
        {
            _stream.Close();
        }

        public override string ToString()
        {
            return _name;
        }

        void IDisposable.Dispose()
        {
            _stream.Close();
        }
    }

    public partial class DataLink
    {
        public override string ToString()
        {
            return this.FileName;
        }
    }
}
