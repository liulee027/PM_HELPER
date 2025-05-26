using Newtonsoft.Json;
using PowerMill_Helper.Class;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// SettingForm.xaml 的交互逻辑
    /// </summary>
    public partial class SettingForm : UserControl
    {
        public SettingForm(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
            this.DataContext = MCS;


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

        #region ControlTitleEvent
        Point Grid_Move_Pos = new Point();
        private void Move_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                Grid_Move_Pos = e.GetPosition(null);
                label.CaptureMouse();
                label.Cursor = Cursors.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MouseDown\r" + ex.ToString());
            }


        }
        private void Move_Border_Mousemove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double dx = e.GetPosition(null).X - Grid_Move_Pos.X + this.Margin.Left;
                    double dy = e.GetPosition(null).Y - Grid_Move_Pos.Y + this.Margin.Top;
                    this.Margin = new Thickness(dx, dy, 0, 0);
                    Grid_Move_Pos = e.GetPosition(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_Mousemove\r" + ex.ToString());
            }

        }
        private void Move_Border_MoseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                label.ReleaseMouseCapture();
                label.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MoseUp\r" + ex.ToString());
            }
        }

        private void BorderCloserButton(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BorderCloserButton\r" + ex.ToString());
            }

        }
        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. sender转为Button
                var btn = sender as Button;
                if (btn == null) return;

                // 2. 获取Button的父StackPanel
                var parentStackPanel = btn.Parent as StackPanel;
                if (parentStackPanel == null) return;

                // 3. 查询Button在StackPanel中的index
                int btnIndex = parentStackPanel.Children.IndexOf(btn);
                if (btnIndex < 0) return;

                // 4. 查找名为"SteetingPage"的Grid
                var grid = this.FindName("SteetingPage") as Grid;
                if (grid == null) return;

                // 5. 遍历Grid下的StackPanel，设置对应index的为显示，其余为隐藏
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    var sp = grid.Children[i] as StackPanel;
                    if (sp != null)
                    {
                        sp.Visibility = (i == btnIndex) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置导航页面出错\r" + ex.ToString());
            }
        }

        #endregion

        #region Dynamicisland
        private void Select_DyniclandLogo(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建图片选择对话框
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "选择灵动岛Logo图片",
                    Filter = "图片文件 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|所有文件 (*.*)|*.*",
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MCS.DynamicIslandLogo = openFileDialog.FileName;
                    MCS.Onchange(nameof(MCS.DynamicIslandLogo));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("选择灵动岛Logo图片出错\r" + ex.ToString());
            }
        }
        #endregion

        #region MacroLib
        #region addFolder
        private void MacroLib_addFolder(object sender, RoutedEventArgs e)
        {
            try
            {
                string theDirPath = "";
                theDirPath = OpenFileBrowserDialog(false);
                if (theDirPath != "")
                {
                    MCS.MacorLibFolders.Add(theDirPath);
                    MCS.Onchange("MacorLibFolders");
                    MCS.DrawMacorLibTreeView();
                    ConfigINI.WriteSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", string.Join(",", MCS.MacorLibFolders.ToArray()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_Addfolder\r" + ex.ToString());
            }

        }


        #endregion
        #region Remover
        private void MacroLib_Remover(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show(MCS.MacorLibFolderssettingSelected);
                if (MCS.MacorLibFolderssettingSelected != null)
                {
                    MCS.MacorLibFolders.Remove(MCS.MacorLibFolderssettingSelected);
                    MCS.DrawMacorLibTreeView();
                    ConfigINI.WriteSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", string.Join(",", MCS.MacorLibFolders.ToArray()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_Remover\r" + ex.ToString());
            }
        }
        #endregion
        #region SelectUp
        private void MacroLib_SelectUp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MCS.MacorLibFolderssettingSelected != null)
                {
                    int index = MCS.MacorLibFolders.IndexOf(MCS.MacorLibFolderssettingSelected);
                    if (index > 0)
                    {
                        MCS.MacorLibFolders.Move(index, index - 1);
                        ConfigINI.WriteSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", string.Join(",", MCS.MacorLibFolders.ToArray()));
                    }

                    MCS.DrawMacorLibTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_SelectUp\r" + ex.ToString());
            }


        }
        #endregion
        #region Selectdown
        private void MacroLib_Selectdown(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MCS.MacorLibFolderssettingSelected != null)
                {
                    int index = MCS.MacorLibFolders.IndexOf(MCS.MacorLibFolderssettingSelected);
                    if (index < MCS.MacorLibFolders.Count - 1)
                    {
                        MCS.MacorLibFolders.Move(index, index + 1);
                        ConfigINI.WriteSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", string.Join(",", MCS.MacorLibFolders.ToArray()));
                    }
                    MCS.DrawMacorLibTreeView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_Selectdown\r" + ex.ToString());
            }


        }
        #endregion
        #region ResLoad
        private void MacroLib_RES(object sender, RoutedEventArgs e)
        {
            try
            {
                MCS.DrawMacorLibTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_RES\r" + ex.ToString());
            }

        }
        #endregion
        #endregion

        #region Ncout
        #region 选择Nc输出文件夹
        private void NCout_Settingoutfolder(object sender, RoutedEventArgs e)
        {
            try
            {
                string theDirPath = "";
                theDirPath = OpenFileBrowserDialog(false);
                if (theDirPath != "")
                {
                    MCS.Ncout_OutputFolderPath = theDirPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm__NCout_Settingoutfolder\r" + ex.ToString());
            }

        }
        #endregion
        #region 选择程序单模板
        private void Ncout_SettingSelectsheetpath(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "选择 程序单模板",
                    Filter = "所有文件 (*.*)|*.*",
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MCS.Ncout_sheetTemplatePath = openFileDialog.FileName;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_Ncout_SettingSelectsheetpath\r" + ex.ToString());
            }
        }
        #endregion
        #region OPT
        #region 新增
        private void NCout_SettingAddopt(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建文件选择对话框
                System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Title = "选择 PMOPTZ 后处理文件",
                    Filter = "PM后处理文件 (*.pmoptz)|*.pmoptz|所有文件 (*.*)|*.*",
                    Multiselect = true // 允许多选
                };

                // 显示对话框并获取结果
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 获取用户选择的文件路径
                    string[] selectedFiles = openFileDialog.FileNames;

                    // 处理选中的文件
                    foreach (string file in selectedFiles)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        MCS.NcOpts.Add(new NcoutOpt() { Name = Path.GetFileNameWithoutExtension(fileInfo.Name), FullPath = fileInfo.FullName });
                    }
                    // SaveSetting_NcoutOptfilelist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_SettingAddopt_Event\r" + ex.ToString());
            }
        }
        #endregion
        #region 删除
        private void NCout_SettingRemoveopt(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MCS.NcOpts.Contains(MCS.SettingNcoutSelectOpt))
                {
                    MCS.NcOpts.Remove(MCS.SettingNcoutSelectOpt);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_SettingRemoveopt_Event\r" + ex.ToString());
            }

        }
        #endregion
        #region 保存
        private void NCout_SettingSaveopt(object sender, RoutedEventArgs e)
        {
            SaveSetting_NcoutOptfilelist();
        }
        private void SaveSetting_NcoutOptfilelist()
        {
            try
            {
                // 将 MCS.NcOpts 集合转换为 JSON 字符串
                string json = JsonConvert.SerializeObject(MCS.NcOpts);
                ConfigINI.WriteSetting(MCS.ConfigInitPath, "NCout", "NCOPTFILELIST", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SaveSetting_NcoutOptfilelist\r" + ex.ToString());
            }
        }
        #endregion
        #endregion
        #endregion

        #region 碰撞检查
        public event RoutedEventHandler SetCheckTp_AddExperCOM_Click;
        private void CheckTp_AddExperCOM(object sender, RoutedEventArgs e)
        {
            SetCheckTp_AddExperCOM_Click?.Invoke(sender, e);
        }
        public event RoutedEventHandler SetCheckTp_RemoveExperCOM_Click;
        private void CheckTp_RemoveExperCOM(object sender, RoutedEventArgs e)
        {
            SetCheckTp_RemoveExperCOM_Click?.Invoke(sender, e);
        }
        public event RoutedEventHandler SetCheckTp_AddExper_ComtextCopy_Click;
        private void CheckTp_AddExper_ComtextCopy(object sender, RoutedEventArgs e)
        {
            SetCheckTp_AddExper_ComtextCopy_Click?.Invoke(sender, e);
        }
        public event RoutedEventHandler SetCheckTp_Openexplorer;
        private void CheckTp_Openexplorer(object sender, RoutedEventArgs e)
        {
            SetCheckTp_Openexplorer?.Invoke(sender, e);
        }
        #endregion

        #region 文件夹询问Window Void
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
        #endregion


        #region 打开配置文件
        private void NCout_OpenSettingini(object sender, RoutedEventArgs e)
        {
            if (File.Exists(MCS.ConfigInitPath))
            {
                Process.Start("notepad.exe", MCS.ConfigInitPath);
            }
        }

        #endregion


        #region 下载更新
        private void UpdataPmHelperClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MCS.Version != MCS.Serverversion)
                {
                    // 使用系统对话框提示用户
                    MessageBoxResult result = MessageBox.Show(MCS.SoftUpdateNote, $"是否要去官网下载最新新版本：{MCS.Serverversion}", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        GetnewVersionSetup?.Invoke(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("已是最新版本：）");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error UpdataPmHelperClick\r"+ex.ToString());
            }
           
        }
        public event RoutedEventHandler GetnewVersionSetup;




        #endregion

     
    }
}
