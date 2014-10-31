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
using System.Collections;
using System.Threading;
using System.Reflection;

namespace Metro2033WpfServer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hashtable clientList = new Hashtable();

        private byte connected = 0;

        private TcpListener listener;

        /// <summary>
        /// Property verändert Statuslabel
        /// </summary>
        internal string Status
        {
            get { return status.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { status.Content = value; })); }
        }

        /// <summary>
        /// Erstellt Hauptfenster und passt die Konsole an die Applikation an
        /// </summary>
        public MainWindow()
        {
            Console.Title = "Metro 2033 RPG Server Konsole";
            Console.Clear();
            Console.WriteLine(AssemblyProduct);
            Console.WriteLine("Version: " + AssemblyVersion);
            Console.WriteLine(AssemblyCopyright);
            Console.WriteLine("-----------------\n");

            InitializeComponent();

            Console.WriteLine(DateTime.Now.ToString() + " Datenmanager initialisiert!\n");
        }

        /// <summary>
        /// Bricht TcpListener ab, so das serverthread endpunkt erreichen werden kann und beendet die Applikation.
        /// </summary>
        /// <param name="e">Das feuernde Ereignis</param>
        protected override void OnClosed(EventArgs e)
        {
            AbortListenForClients();
            base.OnClosed(e);
            Console.WriteLine(DateTime.Now.ToString() + " Datenmanager beendet!");
            //zu Debugzwecken, da konsole ansonsten schließt
            Console.ReadLine();
            Console.WriteLine("A-Jay war hier! :P\n");
        }

        /// <summary>
        /// Methode für custom close button
        /// </summary>
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Methode für senden button
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (textchat.Text != "")
            {
                Broadcast(textchat.Text);
                textchat.Text = "";
            }
            textchat.Focus();
        }

        /// <summary>
        /// gibt assembly infos über dialogfenster aus
        /// </summary>
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void GetInfo(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Owner = this;
            info.ShowDialog();
        }

        /// <summary>
        /// öffnet Dialogfenster für würfel
        /// </summary>
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void GetWuerfelDialog(object sender, RoutedEventArgs e)
        {
            WuerfelDialog dialog = new WuerfelDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        #region Assemblyattributaccessoren

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        # region Dispatcher Gui manipulation

        private void AddChat(string text)
        {
            Dispatcher.Invoke(new Action(() => { textlog.AppendText(text + System.Environment.NewLine); textlog.ScrollToEnd(); }));
        }

        #endregion

        #region Servermanagment

        /// <summary>
        /// Methode wartet auf Clienten
        /// </summary>
        private void ListenForClients()
        {
            Console.WriteLine(DateTime.Now.ToString() + " Serverthread gestartet!");
            listener = new TcpListener(1337);
            listener.Start();
            Console.WriteLine("Warte auf Verbindungen...\n");
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
                catch
                {
                    Console.WriteLine(DateTime.Now.ToString() + " Clientaufnahme abgebrochen!");
                    break;
                }

            }
            Console.WriteLine(DateTime.Now.ToString() + " Serverthread Endpunkt erreicht!\n");
        }

        /// <summary>
        /// Methode behandelt verbindungen
        /// </summary>
        /// <param name="client">der zu behandelnde Client</param>
        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;

            StreamReader thisreader = new StreamReader(tcpClient.GetStream());
            StreamWriter thiswriter = new StreamWriter(tcpClient.GetStream());

            clientList.Add(thiswriter, client);
            connected++;
            Console.WriteLine(DateTime.Now.ToString() + " " + tcpClient.Client.LocalEndPoint + " hat sich verbunden!");
            Status = "Spieler verbunden: " + connected;

            while (true)
            {
                string income;
                try
                {
                    if ((income = thisreader.ReadLine()) != null)
                    {
                        Broadcast(income);
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Bricht das Akzeptieren von Clients in der "ListenForClients Methode" ab.
        /// </summary>
        public void AbortListenForClients()
        {
            try
            {
                listener.Stop();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Sendet einen Stringtext an alle Clienten
        /// </summary>
        /// <param name="msg"></param>
        public void Broadcast(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + msg);
            AddChat(msg);

            foreach (DictionaryEntry Item in clientList)
            {
                TcpClient broadcastClient = (TcpClient)Item.Value;
                StreamWriter broadcastWriter = (StreamWriter)Item.Key;
                broadcastWriter.WriteLine(msg);
                broadcastWriter.Flush();
            }
        }

        #endregion

        /// <summary>
        /// Methode erstellt gegebenenfalls neue Datei und startet Serverthread
        /// </summary>
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void NewFile(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Datei"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Extensible Markup Language (.xml)|*.xml"; // Filter files by extension 

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                string filename = dlg.FileName;


                //----------------------------------------

                Thread serverThread = new Thread(this.ListenForClients);
                serverThread.Start();

                charakterplus.IsEnabled = true;
                charakterminus.IsEnabled = true;
                charakterchange.IsEnabled = true;
                tabs.IsEnabled = true;
            }
        }

        /// <summary>
        /// Methode erlaubt gegebenfalls laden aus einer DAtei und startet Serverthread
        /// </summary>
        /// <param name="sender">Das sendende Objekt</param>
        /// <param name="e">Das feuernde Ereignis</param>
        private void LoadFile(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Datei"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Extensible Markup Language (.xml)|*.xml"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
            }
        }
    }
}
