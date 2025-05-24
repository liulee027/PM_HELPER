using SanNiuSignal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using General_lib;
using System.Configuration;
using Newtonsoft.Json;
using System.Timers;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Windows.Forms;
using System.Diagnostics;

namespace PMH_Service
{
    public class Service_Application
    {
        public Service_Application()
        {

            
        }

      
        public void Start()
        {
            ReadConfig();

            Local_TcpServer_Start();//启动本地TCP通信

            ConnectStart();//启动远程服务器连接

        }

        #region 配置初始化
        string ConfigInitPath = "";
        private void ReadConfig()
        {
            #region 配置文件初始化？
            //ConfigInitPath = System.IO.Path.Combine(PluginFolder, "config.ini");
            ConfigInitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PowerMoFunPMPlugin", "config.ini");
            string directoryPath = Path.GetDirectoryName(ConfigInitPath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!File.Exists(ConfigInitPath))
            {
                File.Create(ConfigInitPath).Dispose();

                ConfigINI.WriteSetting(ConfigInitPath, "LocalTCP", "prot","8910");
            }
                
            #endregion

            try
            {
                LocalServerPort = int.Parse(ConfigINI.ReadSetting(ConfigInitPath, "LocalTCP", "prot", "8910"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取tcp端口号出现问题\r"+ex.ToString());
            }
           
        }
        #endregion

        #region LocalTcp txServer
        private ITxServer txServer = null;
        private int LocalServerPort = 8910;
        private ObservableCollection<LocalClient> local_clients = new ObservableCollection<LocalClient>();
        private Dictionary<IPEndPoint, LocalClient> local_clients_DIC = new Dictionary<IPEndPoint, LocalClient>();

        private void Local_TcpServer_Start()
        {
            try
            {

                txServer = TxStart.startServer(LocalServerPort);
                txServer.BufferSize = 1024;
                txServer.ClientMax = 100;
                txServer.Connect += TxServer_Connect;
                txServer.Disconnection += TxServer_Disconnection;
                txServer.AcceptString += TxServer_AcceptString;
                txServer.StartEngine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Local_TcpServer_Start\r" + ex.ToString());
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

                    txServer.sendMessage(object1, CheckPmSetupVersionresponseString); 

                    //Console.WriteLine("有客户端连入");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TxServer_Connect\r" + ex.ToString());
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
                Console.WriteLine("TxServer_Disconnection\r" + ex.ToString());
            }
        }



        #endregion
        #region LocalMsg
        private void TxServer_AcceptString(IPEndPoint ipEndpoint, string Msg)
        {
            try
            {
                RequestMsg reposrMsg =JsonConvert.DeserializeObject< RequestMsg >(Msg);
                if (reposrMsg != null)
                {
                    if (reposrMsg.Title== "DownLoadNewPmSetup")
                    {
                        Task.Run(() => DownLoadNewPmSetup());
                    }
                }
                //Console.WriteLine($"IP:{ipEndpoint} Msg:{Msg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("TxServer_AcceptString\r" + ex.ToString());
            }
        }
        #endregion

        #region PMServer
        string PMserverApiurl = ConfigurationManager.AppSettings["ServerapiUrl"];
        private Timer _CheckPmSetupVersiontimer;
        private static readonly HttpClient _PMClient = new HttpClient();

        private void ConnectStart()
        {
            CheckPmSetupVersiontimerEvent(null, null);
            _CheckPmSetupVersiontimer = new Timer(1000*60); // 每 60 秒执行一次
            _CheckPmSetupVersiontimer.Elapsed += CheckPmSetupVersiontimerEvent;
            _CheckPmSetupVersiontimer.AutoReset = true;
            _CheckPmSetupVersiontimer.Enabled = true;

            
        }
        private string CheckPmSetupVersionresponseString ="";
        private async void CheckPmSetupVersiontimerEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                var postData = new RequestMsg()
                {
                    Title= "GetPMSetupVersion"
                };

                string json = JsonConvert.SerializeObject(postData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _PMClient.PostAsync(PMserverApiurl+ "PmClient", content);
                CheckPmSetupVersionresponseString = await response.Content.ReadAsStringAsync();
                
                foreach (var item in local_clients) txServer.sendMessage(item.IPEndPoint, CheckPmSetupVersionresponseString);
                Console.WriteLine("最新版本信息: " + CheckPmSetupVersionresponseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("检查版本失败: " + ex.Message);
            }
        }
        #endregion

        #region 最新软件包下载
        //因为使用了CloudFire代理服务器，直接用服务器Get安装包下载速度很慢
        //因此软件获取最新安装包将会通过网页来获取
        //后期考虑将安装包部署在CloudFire R2资源服务器中来为用户提供下载通道(已测试成功但是安全性未知)
        private void DownLoadNewPmSetup()
        {
            string url = "https://liu1ee.online/download.html";
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true 
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开浏览器: " + ex.Message);
            }

        }




        #endregion
    }
}
