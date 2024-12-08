using Delcam.Plugins.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using PowerMill_Helper.Tool;
using PowerMill_Helper.Class;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MessageBox=System.Windows.Forms.MessageBox;
using System.IO;

namespace PowerMill_Helper
{
    /// <summary>
    /// MainFrom.xaml 的交互逻辑
    /// </summary>
    public partial class MainForm : Window
    {
        public string token;
        public PowerMILL.PluginServices PmServices;
        private IPluginCommunicationsInterface PmCom;
        private IntPtr PowerMIlInptr;
        MainCS MCS = null;//ViewModel
        public MainForm(IPluginCommunicationsInterface conn, string token, PowerMILL.PluginServices servicess, IntPtr Pminptr)
        {
            InitializeComponent();
            try
            {
                MCS = (MainCS)DataContext;
                #region Pm接口
                this.token = token;
                this.PmServices = servicess;
                this.PmCom = conn;
                this.PowerMIlInptr = Pminptr;
                #endregion
                #region 实现Pm事件
                ResigerEvent();
                #endregion
                #region 主窗口大小
                Screen[] screens = Screen.AllScreens;
                MainPage.Width = screens[0].WorkingArea.Width;
                MainPage.Height = screens[0].WorkingArea.Height;
                #endregion
                #region 加载控件
                LoadControl();
                #endregion
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
           

        }
        #region PmCommand
        private  void PMCom(string comd)
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
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(PmServices.GetParameterXML(comd));
            XmlNode xmlNode = doc.DocumentElement;
             return xmlNode.InnerText;
        }
        #endregion
        #region PmEvent
        #region ResigerPMEvent
        private void ResigerEvent()
        {
            try
            {
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectOpening", UserOpeningNewPG));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectOpened", UserOpenNewPG));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectClosed", UserCLosePG));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectClosing", UserCLosingPG));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectSavedAs", UserSaveAs));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ProjectSaved", UserSave));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityActivated", UserEntityActivated));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityDeactivated", UserEntityDeActivated));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityCreated", UserEntityCreate));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityDeleted", UserEntityDelete));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityRenamed", UserEntityRename));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ToolpathCalculationStarted", UserTooolPathStar));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("ToolpathCalculationFinished", UserTooolPathCalu));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityDrawn", UserDrawEntity));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("EntityUndrawn", UserUNDrawEntity));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("StockModelStateAdded", UserADDStockmodelState));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("StockModelStateRemoved", UserRemoveStockModelState));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("FeatureSetFeatureAdded", UserFreaturesetAdd));
                PmCom.EventUtils.Subscribe(new Delcam.Plugins.Events.EventSubscription("FeatureSetFeatureRemoved", UserFreaturesetRemove));
            }
            catch (Exception)
            {
                //MessageBox.Show("Test");
            }
            
        }
        #endregion
        private void UserOpeningNewPG(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private  void UserOpenNewPG(string event_name, Dictionary<string, string> event_arguments)
        {
                MCS.ProjectName = GetPMVal("project_pathname(1)");
                MCS.SaveState = true;
        }
        private  void UserCLosePG(string event_name, Dictionary<string, string> event_arguments)
        {
                MCS.ProjectName = "NULL";
            MCS.SaveState = true;
        }
        private void UserCLosingPG(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserSaveAs(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = GetPMVal("project_pathname(1)");
            MCS.SaveState = true;
           
        }
        private  void UserSave(string event_name, Dictionary<string, string> event_arguments)
        {
                MCS.ProjectName = GetPMVal("project_pathname(1)");
                MCS.SaveState = true;
        }
        private void UserEntityActivated(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserEntityDeActivated(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserEntityCreate(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserEntityDelete(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserEntityRename(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserTooolPathStar(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserTooolPathCalu(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.SaveState = false;
        }
        private void UserDrawEntity(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserUNDrawEntity(string event_name, Dictionary<string, string> event_arguments)
        {
            
        }
        private void UserADDStockmodelState(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserRemoveStockModelState(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserFreaturesetAdd(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        private void UserFreaturesetRemove(string event_name, Dictionary<string, string> event_arguments)
        {
        }
        #endregion

        #region 加载控件
        DynamicIslaned DynamicIslaned = null;
        private void LoadControl()
        {
            DynamicIslaned=new DynamicIslaned();
            MainFormGrid.Children.Add  (DynamicIslaned);
            DynamicIslaned.UserSelectProgEvent += DynamicIslaned_UserSelectProgEvent;
            DynamicIslaned.UserSaveProgEvent += DynamicIslaned_UserSaveProgEvent;
            DynamicIslaned.UserSaveOtherWhereEvent += DynamicIslaned_UserSaveOtherWhereEvent;

        }

        private void DynamicIslaned_UserSaveOtherWhereEvent(object sender, RoutedEventArgs e)
        {
            PMCom("FORM RIBBON BACKSTAGE CLOSE PROJECT SAVE AS FILESAVE");
        }

        private void DynamicIslaned_UserSaveProgEvent(object sender, RoutedEventArgs e)
        {
            PMCom("PROJECT SAVE");
        }

        public static string OpenFileBrowserDialog(bool multiselect)
        {
            FolderSelectDialog fbd = new FolderSelectDialog();
            fbd.Multiselect = multiselect;

            fbd.ShowDialog();

            string selected_folders = "";
            if (multiselect)
            {
                string[] names = fbd.FileNames;
                selected_folders = string.Join(",", names);
            }
            else
            {
                selected_folders = fbd.FileName;
            }

            return selected_folders;
        }
        private void DynamicIslaned_UserSelectProgEvent(object sender, RoutedEventArgs e)
        {
            string theDirPath = "";
            theDirPath = OpenFileBrowserDialog(false);
            if (File.Exists(theDirPath + "\\pmill.ico"))
            {
                PMCom($"PROJECT OPEN '{theDirPath}'");
            }
            else
            {
                if (theDirPath != "")
                {
                    MessageBox.Show("此文件夹不是PowerMIill项目");
                }
            }
        }
        #endregion

    }
}
