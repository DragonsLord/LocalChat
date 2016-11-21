using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.ServiceModel;
using ChatClient.ChatService;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for DownloadButton.xaml
    /// </summary>
    public partial class DownloadButton : UserControl, INotifyPropertyChanged
    {
        private IChatProvider _server;
        private readonly string _fileAdress;
        public string FileName { get; private set; }

        private static readonly RoutedEvent DownloadStartedEvent = EventManager.RegisterRoutedEvent("DownloadStarted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DownloadButton));
        public event RoutedEventHandler DownloadStarted
        {
            add { AddHandler(DownloadStartedEvent, value); }
            remove { RemoveHandler(DownloadStartedEvent, value); }
        }
        private void RaiseDownloadStartedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(DownloadStartedEvent, this);
            RaiseEvent(args);
        }
        private static readonly RoutedEvent DownloadEndedEvent = EventManager.RegisterRoutedEvent("DownloadEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DownloadButton));
        public event RoutedEventHandler DownloadEnded
        {
            add { AddHandler(DownloadEndedEvent, value); }
            remove { RemoveHandler(DownloadEndedEvent, value); }
        }
        private void RaiseDownloadEndedEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(DownloadEndedEvent, this);
            RaiseEvent(args);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static readonly DependencyProperty IsDownloadingProperty = DependencyProperty.Register("IsDownloading",typeof(bool),typeof(DownloadButton));
        public bool IsDownloading {
            get { return (bool)GetValue(IsDownloadingProperty); }
            set { 
                SetValue(IsDownloadingProperty, value); 
                NotifyPropertyChanged(); 
            }
        }

        public DownloadButton(DataLink link, IChatProvider server)
        {
            InitializeComponent();
            _server = server;
            this.FileNameLable.Content = link.FileName;
            SetReadableSize(link.Length);
            _fileAdress = link.Adress;
            FileName = link.FileName;
        }

        private void SetReadableSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double s = size;
            while (s >= 1024 && ++order < sizes.Length)
            {
                s = s / 1024;
            }

            this.SizeLabel.Content = String.Format("{0:0.##} {1}", s, sizes[order]);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog save_dialog = new System.Windows.Forms.SaveFileDialog();
            save_dialog.Filter = String.Format("{0} file (*{0})|*{0}|All files (*.*)|*.*", System.IO.Path.GetExtension(_fileAdress));
            save_dialog.FilterIndex = 1;
            save_dialog.RestoreDirectory = true;
            if (save_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RaiseDownloadStartedEvent();
                IsDownloading = true;
                await Task.Run(() =>
                {
                    using (var stream = save_dialog.OpenFile())
                    {
                        byte[] buffer;
                        try
                        {
                            while ((buffer = _server.DownloadData(_fileAdress, 5000)) != null)
                                stream.Write(buffer, 0, buffer.Length);
                        }
                        catch (FaultException<ExceptionType> fault)
                        {
                            if (fault.Detail == ExceptionType.ErrorDuringDataTransfer)
                                System.Windows.Forms.MessageBox.Show("Error accured during downloading. Please, try again");
                            if (fault.Detail == ExceptionType.FileNotFound)
                                System.Windows.Forms.MessageBox.Show("File Not Found on server. Incorrect data link");
                        }
                    }
                });

                RaiseDownloadEndedEvent();
                IsDownloading = false;
            }
        }

    }
}
