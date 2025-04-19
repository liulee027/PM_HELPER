using PMHelperService.CLASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PipeTestClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartPipeClient();


   
        }
        #region Pipe
        private PipeClient pipeClient;

        private void StartPipeClient()
        {
            try
            {
                pipeClient = new PipeClient();
                pipeClient.MessageReceived += PipeClient_MessageReceived;
                pipeClient.StatusChanged += PipeClient_StatusChanged;
                pipeClient.Start();

                new Thread(SendMSG).Start();
            }
            catch (Exception ex)
            {
               MessageBox.Show("StartPipeClient\r"+ex.ToString());
            }
        }

        private void StopPipeClient()
        {
            try
            {
                pipeClient?.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("StopPipeClient\r" + ex.ToString());
            }
            
        }

        private void PipeClient_MessageReceived(string message)
        {
            try
            {
                // 处理接收到的消息
                Console.WriteLine($"收到服务器消息: {message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("PipeClient_MessageReceived\r" + ex.ToString());
            }
            
        }
        private void PipeClient_StatusChanged(string status)
        {
            try
            {
                // 处理状态更新，例如在UI中显示
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PipeClient_StatusChanged\r" + ex.ToString());
            }
          
        }

        private async void SendMessageToServer(string message)
        {
            try
            {
                await pipeClient.SendMessage(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendMessageToServer\r" + ex.ToString());
            }
           
        }

        private void SendMSG()
        {
            while (true)
            {
             Thread.Sleep(1000);
                SendMessageToServer(DateTime.Now.ToString());
            }
        }

     
        #endregion





    }
}
