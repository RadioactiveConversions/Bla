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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace Metro2033WpfClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread netWatcher;

        private TcpClient clientSocket;

        private StreamReader reader;

        private StreamWriter writer;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            reader.Close();
            base.OnClosed(e);
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (textchat.Text != "")
            {
                writer.WriteLine(textchat.Text);
                writer.Flush();
                textchat.Text = "";
            }
            textchat.Focus();
        }

        private void GetInfo(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Owner = this;
            info.ShowDialog();
        }

        private void GetWuerfelDialog(object sender, RoutedEventArgs e)
        {
            WuerfelDialog dialog = new WuerfelDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void GetConnectDialog(object sender, RoutedEventArgs e)
        {
            ConnectDialog dialog = new ConnectDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                status.Content = "Verbinde...";
                try
                {
                    clientSocket = new TcpClient(dialog.GetHost, int.Parse(dialog.GetPort));

                    reader = new StreamReader(clientSocket.GetStream());
                    writer = new StreamWriter(clientSocket.GetStream());

                    //Thread.Sleep(1);

                    menuDisconnect.IsEnabled = true;
                    status.Content = "Verbindung aufgebaut";
                    netWatcher = new Thread(StreamListening);
                    netWatcher.Start();
                }
                catch
                {
                    status.Content = "Verbindung fehlgeschlagen";
                }
            }

            dialog.Close();
        }

        private void CloseConnection(object sender, RoutedEventArgs e)
        {
            clientSocket.Close();
            menuDisconnect.IsEnabled = false;
            status.Content = "Verbindung getrennt";
        }

        private void AddChat(string text)
        {
            Dispatcher.Invoke(new Action(() => { textlog.AppendText(text + System.Environment.NewLine); textlog.ScrollToEnd(); }));
        }

        private void StreamListening()
        {
            string income;
            while (true)
            {
                try
                {
                    if ((income = reader.ReadLine()) != null)
                    {
                        AddChat(income);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

    }
}
