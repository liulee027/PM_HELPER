using Delcam.Plugins.Framework;
using System;
using System.Windows.Forms;

namespace PowerMill_Helper
{
    public partial class Plugin_PanesPage_WinForm : UserControl
    {
        public Plugin_PanesPage_WinForm(IPluginCommunicationsInterface conn, string token, PowerMILL.PluginServices servicess, IntPtr Pminptr)
        {
            InitializeComponent();
            #region Pm接口
            this.token = token;
            this.PmServices = servicess;
            this.PmCom = conn;
            this.PowerMIlInptr = Pminptr;
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

        private void button1_Click(object sender, EventArgs e)
        {
            PMCom("Print='Plugin_Test'");
        }
    }
}
