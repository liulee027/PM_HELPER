using Delcam.Plugins.Framework;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PowerMill_Helper
{
    [Guid("BC3610A0-A0F6-4244-8053-A99AADE569F5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class BasicPlugin : PluginFrameworkWithPanesAndTabs
    {
        public BasicPlugin()
        {


        }

        #region 插件信息
        public override string PluginName => "【动力磨坊】PowerMill灵动助手插件"; 

        public override string PluginAuthor => "Liu1ee";

        public override string PluginDescription => "PMHelper\r这个插件框架可以帮助各开发者更快去实现自己的功能。";

        public override string PluginIconPath => "Image/icon/Pluginicon.png";

        public override Version PluginVersion => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public override Version PowerMILLVersion => new Version(13, 1, 4);

        public override bool PluginHasOptions => throw new NotImplementedException();

        public override string PluginAssemblyName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        public override Guid PluginGuid => new Guid("BC3610A0-A0F6-4244-8053-A99AADE569F5");
        #endregion

        #region 垂直插件面板实现
        Plugin_PanesPage Plugin_PanesPage;
        protected override void register_panes()
        {
            //这里打开你的垂直插件面板
            //Plugin_PanesPage = new Plugin_PanesPage(this, Token, Services,(IntPtr)this.ParentWindow);
            // register_pane(new PaneDefinition(Plugin_PanesPage,700,300,"PowerMill_Plugin",null));

            //垂直插件面板入口启动全屏界面
            //在插件入口启动会报错？
            OpenMainFrom();

        }
        #endregion

        #region 水平插件面板实现
        protected override void register_tabs()
        {
            //这里打开你的水平插件面板


            //这里打开你的水平插件面板

        }
        #endregion

        #region PM Macro=>Plugin
        // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}
        public override void ProcessPluginCommand(string Command)
        {
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}Shutdown
            if (Command=="Shutdown")
            {

                Shutdown();
            }
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}VerifyToolpathS
            if (Command=="VerifyToolpathS")
            {
                mainForm.OpenCheckTP();
            }
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}CheckTPMsg
            if (Command.Contains("CheckTPMsg"))
            {
                mainForm.GetCheckTP_Msg(Command);
            }
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}showSettingPage
            if (Command == "showDynamicIslaned")
            {
                mainForm.MCS.DynamicIslandIsVisibility = true;
            }
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}hiddenDynamicIslaned
            if (Command == "hiddenDynamicIslaned")
            {
                mainForm.MCS.DynamicIslandIsVisibility = false;
            }
            // plugin {BC3610A0-A0F6-4244-8053-A99AADE569F5}showSettingPage
            if (Command == "showSettingPage")
            {
                mainForm.SettingForm_.Visibility = Visibility.Visible;
            }


        }
        #endregion 
        #region OpenMainFrom
        MainForm mainForm;
        private void OpenMainFrom()
        {
            try
            {
                mainForm = new MainForm(this, Token, Services, (IntPtr)this.ParentWindow);
                WindowInteropHelper wih = new WindowInteropHelper(mainForm);
                wih.Owner = new IntPtr((int)this.ParentWindow);
                RECT rect = new RECT();
                GetWindowRect((IntPtr)this.ParentWindow, ref rect);
                mainForm.ShowInTaskbar = false;
                mainForm.Left = 0;
                mainForm.Top = 0;
                mainForm.Show();
            }
            catch (Exception EX)
            {
                MessageBox.Show("ErrorVoid: OpenMainFrom\r" + EX.ToString());
            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        #endregion

        #region Shutdown
        private void Shutdown()
        {
            try
            {
                shutdown_framework();
            }
            catch (Exception ex)
            {
                MessageBox.Show("shutdown_framework Error\r " + ex.ToString());
            }

        }
        #endregion



    }
}
