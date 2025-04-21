using PowerMILL;
using PowerMill_Helper.Class;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// DynamicIslaned.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicIslaned : UserControl
    {
        public DynamicIslaned(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
        }

        private MainCS MCS;

        #region PmCommand
        public string token;
        public PowerMILL.PluginServices PmServices;
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

        #region 选择程序文件夹
        private void Image_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("DynamicIslaned_Image_PreviewMouseUp\r" + ex.ToString());
            }


        }
        #endregion

        #region 保存程序
        private void UserSaveProg(object sender, MouseButtonEventArgs e)
        {
            PMCom("PROJECT SAVE");
        }
        #endregion

        #region 另存为程序
        private void UserSaveOtherWhere(object sender, MouseButtonEventArgs e)
        {

            PMCom("FORM RIBBON BACKSTAGE CLOSE PROJECT SAVE AS FILESAVE");
        }
        #endregion

        #region 打开设置
        private void DynamicislandOpenSetting(object sender, MouseButtonEventArgs e)
        {
            // UserOpenSettingEvent?.Invoke(this, e);
            ONUseropenAppEvent?.Invoke("SettingForm");
        }
        #endregion

        #region 打开宏库
        private void UserOpenMacroLib(object sender, RoutedEventArgs e)
        {
            //UserOpenMacroLibEvent?.Invoke(this, e);
            ONUseropenAppEvent?.Invoke("MacroLib");
        }
        #endregion

        #region 扩展功能展开
        public event RoutedEventHandler UserSelectAppClickEvent;
        private void UserSelectAppClick(object sender, RoutedEventArgs e)
        {
            UserSelectAppClickEvent?.Invoke(this, e);
        }
        #endregion


        public delegate void ONUseropenApp(string appName);
        public event ONUseropenApp ONUseropenAppEvent;
        private void ClickExpendApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Border Bdr = (Border)sender;
                TextBlock textBlock = Bdr.Child as TextBlock;
                ONUseropenAppEvent?.Invoke(textBlock.Text);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ClickExpendApp\r" + ex.ToString());
            }

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
    }
}
