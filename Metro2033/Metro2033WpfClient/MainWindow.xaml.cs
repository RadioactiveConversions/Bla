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

namespace Metro2033WpfClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (textchat.Text != "")
            {
                textlog.AppendText((textchat.Text + System.Environment.NewLine));
                textlog.ScrollToEnd();
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
                    status.Content = "Verbindung aufgebaut";

                }
                catch
                {

                }

                MessageBox.Show("User clicked OK");
            }

            dialog.Close();
        }

    }
}
