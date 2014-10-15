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
    /// Interaktionslogik für WuerfelDialog.xaml
    /// </summary>
    public partial class WuerfelDialog : Window
    {
        private byte _numValue = 0;

        public WuerfelDialog()
        {
            InitializeComponent();
            txtNum.SelectAll();
            txtNum.Focus();
        }

        public byte NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!byte.TryParse(txtNum.Text, out _numValue))
            {
                txtNum.Text = _numValue.ToString();
                txtNum.SelectAll();
            }
        }

        private void Werfen(object sender, RoutedEventArgs e)
        {
            if (NumValue == 0)
                this.Close();
            else
                ;
        }
    }
}
