using System;
using System.IO;
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
using System.DirectoryServices;
using Images = System.Drawing;

using System.ServiceModel;
using ChatClient.ChatService;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatProviderCallback
    {
        private string _name = Environment.UserName;

        private IChatProvider chat_service;

        public MainWindow()
        {
            InitializeComponent();
            this.UserNameLabel.Content = _name + ":";
            this.UserNameBox.Text = _name;
            this.DataContext = this; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScanLocalNetwork();
        }

        private async void ScanLocalNetwork()
        {
            this.StatusLabel.Content = "Scaning Local Network...";
            this.AdressBox.Items.Clear();
            this.AdressBox.Items.Add("localhost");
            List<string> computers = await Task<List<string>>.Run(() =>
            {
                List<string> comps = new List<string>();
                DirectoryEntry root = new DirectoryEntry("WinNT://WORKGROUP");
                foreach (DirectoryEntry computer in root.Children)
                {
                    if (computer.Name != "Schema")
                        comps.Add(computer.Name);
                }
                return comps;
            });
            computers.ForEach((name) => this.AdressBox.Items.Add(name));
            this.StatusLabel.Content = "Ready";
        }

        public void Write(Message message)
        {
            Run line = new Run(message.Author);
            if (message.Author == _name)
                line.Foreground = Brushes.Red;
            else line.Foreground = Brushes.Blue;
            this.MessageBox.Inlines.Add(new Run(String.Format("[{0}]", DateTime.Now.ToString("HH:mm:ss"))) { Foreground = Brushes.Black});
            this.MessageBox.Inlines.Add(line);
            this.MessageBox.Inlines.Add(": " + message.Text);
            this.MessageBox.Inlines.Add(new LineBreak());
            
            if (message.IsDataAttached)
            {
                foreach (var link in message.DataLinks)
                {
                    this.MessageBox.Inlines.Add(new DownloadButton(link, chat_service) { Style = (Style)this.Resources["DownloadTracker"] });
                }
                this.MessageBox.Inlines.Add(new LineBreak());
            }

            this.Scroll.ScrollToBottom();
        }

        private void SendMessagebtn_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void SendMessage()
        {
            if (chat_service != null)
            {
                if (this.MessageField.Text != null && (this.MessageField.Text != string.Empty || this.DataHandler.Items.Count > 0))
                {
                    try
                    {
                        chat_service.SendMessage(new Message
                        {
                            Author = _name,
                            Text = this.MessageField.Text,
                            Recievers = this.UsersList.Items.OfType<string>().ToList(),
                            IsDataAttached = (this.DataHandler.Items.Count != 0),
                            DataLinks = this.DataHandler.Items.OfType<FileData>().Select(f => f.GetLink()).ToList()
                        });

                        this.MessageField.Text = string.Empty;
                    }
                    //catch (FaultException<ExceptionType> fault)
                    //{
                    //    if (fault.Detail == ExceptionType.EmptyOrUnknownRecieverList)
                    //        System.Windows.Forms.MessageBox.Show("Incorrect recievers list");
                    //    if (fault.Detail == ExceptionType.ErrorDuringDataTransfer)
                    //        System.Windows.Forms.MessageBox.Show("Failed to upload data");
                    //}
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            else 
            {
                if (this.MessageField.Text != null && this.MessageField.Text != string.Empty)
                {
                    Run line = new Run(String.Format("[{2}]{0}: {1}", _name, this.MessageField.Text, DateTime.Now.ToString("HH:mm:ss")));
                    line.Foreground = Brushes.Gray;
                    this.MessageBox.Inlines.Add(line);
                    this.MessageBox.Inlines.Add(new LineBreak());
                    this.MessageField.Text = string.Empty;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (chat_service != null)
                chat_service.UnregisterUser(_name);
        }

        private async void Connect_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.UserNameBox.Text != String.Empty && (this.AdressBox.SelectedItem != null || this.AdressBox.Text != String.Empty))
                _name = this.UserNameBox.Text;
            else 
            {
                System.Windows.Forms.MessageBox.Show("Some fields are empty");
                return;
            }
            string local_machine = (this.AdressBox.SelectedItem != null)? this.AdressBox.SelectedItem.ToString(): this.AdressBox.Text;
            if (local_machine != null)
            {
                try
                {
                    this.ConnectionLabel.Content = String.Format("Connecting to {0}", local_machine);
                    InstanceContext context = new InstanceContext(this);
                    chat_service = new ChatProviderClient(context, "NetTcpBinding_IChatProvider", String.Format("net.tcp://{0}:8733/ChatProvider/", local_machine));

                    await chat_service.RegisterNewUserAsync(_name);
                    this.UserNameLabel.Content = _name + ":";

                    this.ConnectionLabel.Content = "Getting users list";

                    var users = await chat_service.GetUsersListAsync();
                    users.ForEach(user => this.UsersList.Items.Add(user));

                    this.ConnectionLabel.Content = String.Format("Connected to {0}", local_machine);
                }
                catch(EndpointNotFoundException)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("It seems that {0} doesn't host ChatServer",local_machine));
                    chat_service = null;
                    this.ConnectionLabel.Content = "Disconnected";
                }
                catch(FaultException<ExceptionType> fault)
                {
                    chat_service = null;
                    var error_type = fault.Detail;
                    if (error_type == ExceptionType.UserAlreadyRegisterd)
                        System.Windows.Forms.MessageBox.Show(String.Format("User with name {0} already registered.\nTry Another name",_name));
                    this.ConnectionLabel.Content = "Disconnected";
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    chat_service = null;
                    this.ConnectionLabel.Content = "Disconnected";
                }
            }
        }

        private void Disconnect_btn_Click(object sender, RoutedEventArgs e)
        {
            if (chat_service != null)
                chat_service.UnregisterUser(_name);

            Disconnect();
        }

        public void Disconnect()
        {
            this.ConnectionLabel.Content = "Disconnected";
            this.UsersList.Items.Clear();
            chat_service = null;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog open_file_dialog = new System.Windows.Forms.OpenFileDialog();
            open_file_dialog.Multiselect = true;
            if (open_file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach(var file_name in open_file_dialog.FileNames)
                    this.DataHandler.Items.Add(new FileData(file_name));
                this.DataHandler.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void DataHandler_Drop(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            var pathes = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in pathes)
            {
                FileData file = new FileData(path);
                this.DataHandler.Items.Add(file);
            }
        }

        public void NewUserRegistered(string user_name)
        {
            this.UsersList.Items.Add(user_name);
        }

        public void UserUnregistered(string user_name)
        {
            this.UsersList.Items.Remove(user_name);
        }

        public byte[] Upload(int buffer_size, int file_index)
        {
            byte[] buffer = new byte[buffer_size];
            var file = (FileData)this.DataHandler.Items[file_index];
            if (file.ReadBytes(buffer, buffer_size) > 0)
                return buffer;
            else {
                file.CloseStream();
                this.DataHandler.Items.Remove(file);
                if (DataHandler.Items.Count == 0)
                    DataHandler.Visibility = System.Windows.Visibility.Collapsed;
                return null; 
            }
        }

        private void MessageField_PreviewDragEnter(object sender, DragEventArgs e)
        {
            this.DataHandler.Visibility = System.Windows.Visibility.Visible;
        }

        private void MessageField_PreviewDragLeave(object sender, DragEventArgs e)
        {
            if (this.DataHandler.Items.Count == 0)
                this.DataHandler.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OnDownloadStarted(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "Downloading...";
        }

        private void OnDownloadEnded(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = String.Format("{0} downloaded", ((DownloadButton)sender).FileName);
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.DataHandler.Items.RemoveAt(DataHandler.SelectedIndex);
            if (DataHandler.Items.Count == 0)
                DataHandler.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
