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

//추가한 헤더
using System.Windows.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace InterviewAI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient Client;
        private NetworkStream Nstream;


        public MainWindow()
        {
            InitializeComponent();
            //ConnectServer();
        }

        private void ConnectServer()
        {
            try
            {
                Client = new TcpClient();
                Client.Connect("10.10.20.113", 9195);
                Nstream = Client.GetStream();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Client?.Close();
            Nstream?.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Uri uri = new Uri("/Page1.xaml", UriKind.Relative);
            //Page1 page = new Page1();
            //frame1.Content = page;
        }
    }
}
