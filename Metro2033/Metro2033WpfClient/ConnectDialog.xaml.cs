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
using System.Windows.Shapes;

namespace Metro2033WpfClient
{
    /// <summary>
    /// Interaktionslogik für ConnectDialog.xaml
    /// </summary>
    public partial class ConnectDialog : Window
    {
        public ConnectDialog()
        {
            InitializeComponent();
            txtip.SelectAll();
            txtip.Focus();
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Hide();
        }

        public string Ip
        {
            get
            {
                return this.txtip.Text;
            }
        }

        public string Port
        {
            get
            {
                return this.txtport.Text;
            }
        }

    }
}
