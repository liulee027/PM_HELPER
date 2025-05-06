using Delcam.Plugins.Framework;
using Newtonsoft.Json.Linq;
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

namespace PowerMill_Helper
{
    /// <summary>
    /// Plugin_PanesPage.xaml 的交互逻辑
    /// </summary>
    public partial class Plugin_PanesPage : UserControl
    {
        public Plugin_PanesPage(IPluginCommunicationsInterface conn, string token, PowerMILL.PluginServices servicess, IntPtr Pminptr)
        {
            InitializeComponent();
            #region Pm接口
            this.token = token;
            this.PmServices = servicess;
            this.PmCom = conn;
            this.PowerMIlInptr = Pminptr;
            #endregion

            #region 添加winform窗口
            /*
            Plugin_PanesPage_WinForm plugin_PanesPage_WinForm = new Plugin_PanesPage_WinForm(conn, token, servicess, Pminptr);
            MainForm.Child = plugin_PanesPage_WinForm;
            */
            #endregion

        }

        #region PmCommand
        public string token;
        public PowerMILL.PluginServices PmServices;
        private IPluginCommunicationsInterface PmCom;
        private IntPtr PowerMIlInptr;
        private void PMCom(string comd)
        {
            PmServices.InsertCommand(token, comd);
        }

        private string PMComEX(string comd)
        {
            object item;
            PmServices.InsertCommand(token, "ECHO OFF DCPDEBUG UNTRACE COMMAND ACCEPT");
            PmServices.DoCommandEx(token, comd, out item);
            return item.ToString().TrimEnd();
        }
        private string GetPMVal(string comd)
        {
            string Result = PmServices.GetParameterValueTerse(token, comd);
            if (Result.IndexOf("#错误:") == 0) return "";
            return PmServices.GetParameterValueTerse(token, comd);
        }
        #endregion
    }
}
