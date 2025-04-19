using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AtWebServer.Class;
using Newtonsoft.Json;
using SanNiuSignal;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PMHelperService
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() { InitializeComponent(); }

        #region LocalTcpServer
        private ITxServer txServer=null;
        private int LocalServerPort = 8901;

        class LocalClient
        {
            public int HWND { get; set; }
            public string ProjectName { get; set; }
            public string ProjectPath { get; set; }
            public IPEndPoint IPEndPoint { get; set; }
        }
        private ObservableCollection<LocalClient> local_clients = new ObservableCollection<LocalClient>();
        private Dictionary<IPEndPoint, LocalClient> local_clients_DIC = new Dictionary<IPEndPoint, LocalClient>() ;

        private void Local_TcpServer_Start()
        {
            try
            {
                txServer = TxStart.startServer(LocalServerPort);
                txServer.AcceptString += TxServer_AcceptString;
                txServer.Connect += TxServer_Connect;
                txServer.Disconnection += TxServer_Disconnection;
                txServer.StartEngine();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Local_TcpServer_Start\r"+ex.ToString());
            }
        }
        private void TxServer_Connect(IPEndPoint object1)
        {
            try
            {
                if (!local_clients_DIC.ContainsKey(object1))
                {
                    LocalClient localClient = new LocalClient();
                    localClient.IPEndPoint = object1;
                    local_clients_DIC.Add(object1, localClient);
                    local_clients.Add(localClient);
                    Console.WriteLine("有客户端连入");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TxServer_Connect\r" + ex.ToString());
            }
        }

        private void TxServer_Disconnection(IPEndPoint object1, string object2)
        {
            try
            {
                if (local_clients_DIC.ContainsKey(object1))
                {
                    local_clients.Remove(local_clients_DIC[object1]);
                    local_clients_DIC.Remove(object1);
                    Console.WriteLine("有客户端断开");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TxServer_Disconnection\r" + ex.ToString());
            }
        }



        

        

        private void TxServer_AcceptString(IPEndPoint object1, string object2)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("TxServer_AcceptString\r" + ex.ToString());
            }
        }


        #endregion

        #region Http.net
        private bool IsLink = false;
        private static readonly HttpClient httpClient = new HttpClient();
        private static string PmClienturl = "http://liu1ee.space:2052/api/PmClient";
        
        #endregion

        #region 启动
        // 在 #region 检查版本 部分添加以下代码
       
        private async void windowload(object sender, RoutedEventArgs e)
        {
            #region 此窗体隐藏
            this.Hide();
            this.ShowInTaskbar = false;
            #endregion

            #region 查看是否启动
            if (Process.GetProcessesByName("PMHelperService").Length > 1)
            {
                System.Environment.Exit(System.Environment.ExitCode);
                this.Close();
                return;
            }
            #endregion

            #region 启动本地TcpServer
            Local_TcpServer_Start();
            #endregion

            #region 检查版本
            await CheckVersionAsync();
            #endregion

            #region 心跳

            #endregion

        }
        #endregion

        #region 检查版本
        private async Task CheckVersionAsync()
        {
            try
            {

                // 使用POST方法
                HttpResponseMessage response = await httpClient.PostAsync(PmClienturl, httpcontent);


                if (response.IsSuccessStatusCode)
                {
                    string json_ = await response.Content.ReadAsStringAsync();

                    // 创建 ResponMsg 对象
                    ResponMsg responMsg = new ResponMsg()
                    {
                        Title = "GetPMVersion",
                        Msg = json_
                    };

                 
                }
                else
                {
                    Console.WriteLine($"获取版本失败: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求异常: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查版本异常: {ex.Message}");
            }
        }
        #endregion

        #region 检查与服务器连线
        private async Task CheckIsLinkAsync()
        {
            try
            {

                string jsonData = JsonConvert.SerializeObject(data);
                var httpcontent = new StringContent("{}", Encoding.UTF8, "application/json");
                // 使用POST方法
                HttpResponseMessage response = await httpClient.PostAsync(PmClienturl, httpcontent);


                if (response.IsSuccessStatusCode)
                {
                    string json_ = await response.Content.ReadAsStringAsync();

                    // 创建 ResponMsg 对象
                    ResponMsg responMsg = new ResponMsg()
                    {
                        Title = "GetPMVersion",
                        Msg = json_
                    };

                  
                }
                else
                {
                    Console.WriteLine($"获取版本失败: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP请求异常: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"检查版本异常: {ex.Message}");
            }
        }
        #endregion

        #region Closing
        private void windowclosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        #endregion

       
    }
}
