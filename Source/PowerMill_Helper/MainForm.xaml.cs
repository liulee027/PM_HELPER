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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using Delcam.Plugins.Events;
using System.Windows.Media.Animation;
using UserControl = System.Windows.Controls.UserControl;
using Button = System.Windows.Controls.Button;
using System.Threading;
using System.Xml.Linq;

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
                ReadSetting();
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
        private void ResigerEvent()
        {
            try
            {
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectOpening", UserOpeningNewPG));
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectOpened", UserOpenNewPG));
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectClosed", UserCLosePG));
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectClosing", UserCLosingPG));
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectSavedAs", UserSaveAs));
                PmCom.EventUtils.Subscribe(new EventSubscription("ProjectSaved", UserSave));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityActivated", UserEntityActivated));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityDeactivated", UserEntityDeActivated));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityCreated", UserEntityCreate));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityDeleted", UserEntityDelete));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityRenamed", UserEntityRename));
                PmCom.EventUtils.Subscribe(new EventSubscription("ToolpathCalculationStarted", UserToolpathCalculationStarted));
                PmCom.EventUtils.Subscribe(new EventSubscription("ToolpathCalculationFinished", UserToolpathCalculationFinished));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityDrawn", UserDrawEntity));
                PmCom.EventUtils.Subscribe(new EventSubscription("EntityUndrawn", UserUNDrawEntity));
                PmCom.EventUtils.Subscribe(new EventSubscription("StockModelStateAdded", UserADDStockmodelState));
                PmCom.EventUtils.Subscribe(new EventSubscription("StockModelStateRemoved", UserRemoveStockModelState));
                PmCom.EventUtils.Subscribe(new EventSubscription("FeatureSetFeatureAdded", UserFreaturesetAdd));
                PmCom.EventUtils.Subscribe(new EventSubscription("FeatureSetFeatureRemoved", UserFreaturesetRemove));
            }
            catch (Exception)
            {
            }
        }
        private void UserOpeningNewPG(string event_name, Dictionary<string, string> event_arguments)
        {
        }

        private void UserOpenNewPG(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = GetPMVal("project_pathname(1)");
            MCS.SaveState = true;
        }

        private void UserCLosePG(string event_name, Dictionary<string, string> event_arguments)
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

        private void UserSave(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = GetPMVal("project_pathname(1)");
            MCS.SaveState = true;
        }

        private void UserEntityActivated(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string EntityName = event_arguments["Name"];
            try
            {
                if (MCS.PMEmtitys.ContainsKey(text)) MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName).Isactivate = true;
            }                      
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("UserEntityActivated\r" + ex.ToString());
            }
   
        }

        private void UserEntityDeActivated(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string EntityName = event_arguments["Name"];
            try
            {
                if (MCS.PMEmtitys.ContainsKey(text)) MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName).Isactivate = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UserEntityDeActivated\r" + ex.ToString());
            }
        
        }

        private void UserEntityCreate(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string name = event_arguments["Name"];
            try
            {
                if (MCS.PMEmtitys.ContainsKey(text))
                {
                    PMEntity pMEntity = MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == name);
                    if (pMEntity==null)
                    {
                        MCS.PMEmtitys[text].Add(new PMEntity() { Name = name });
                        MCS.Onchange($"PM{text}List");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("UserEntityCreate\r" + ex.ToString());
            }
        }

        private void UserEntityDelete(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string EntityName = event_arguments["Name"];
            try
            {
                if (MCS.PMEmtitys.ContainsKey(text))
                {
                    MCS.PMEmtitys[text].Remove(MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName));
                    MCS.Onchange($"PM{text}List");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("UserEntityDelete\r" + ex.ToString());
            }
        
        }

        private void UserEntityRename(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string name = event_arguments["Name"];
            string PreviousName = event_arguments["PreviousName"];
            try
            {
                if (MCS.PMEmtitys.ContainsKey(text)) MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == PreviousName).Name = name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UserEntityRename\r" + ex.ToString());
            }
        }

        private void UserToolpathCalculationStarted(string event_name, Dictionary<string, string> event_arguments)
        {
        }

        private void UserToolpathCalculationFinished(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.SaveState = false;
        }

        private void UserDrawEntity(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string EntityName = event_arguments["Name"];
            try
            {
                MCS.LogVoid(text);
                MCS.LogVoid(EntityName);

                if (MCS.PMEmtitys.ContainsKey(text))
                {
                    PMEntity pMEntity = MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName);
                    if (pMEntity == null)
                    {
                        pMEntity = new PMEntity() { Name = EntityName };
                        MCS.PMEmtitys[text].Add(pMEntity);
                    }
                    pMEntity.IsDraw = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UserDrawEntity\r" + ex.ToString());
            }
        }

        private void UserUNDrawEntity(string event_name, Dictionary<string, string> event_arguments)
        {
            string text = event_arguments["EntityType"];
            string EntityName = event_arguments["Name"];
            try
            { 
                if (MCS.PMEmtitys.ContainsKey(text)) MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName).IsDraw = false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("UserUNDrawEntity\r" + ex.ToString());
            }
          
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
        SettingForm SettingForm_ = null;
        MacroLib MacroLib_ = null;
        Debug Debug_ = null;
        EntitySelect EntitySelect_ = null;

        private void LoadControl()
        {
            try
            {
                DynamicIslaned = new DynamicIslaned();
                MainFormGrid.Children.Add(DynamicIslaned);
                DynamicIslaned.UserSelectProgEvent += DynamicIslaned_UserSelectProgEvent;
                DynamicIslaned.UserSaveProgEvent += DynamicIslaned_UserSaveProgEvent;
                DynamicIslaned.UserSaveOtherWhereEvent += DynamicIslaned_UserSaveOtherWhereEvent;
                DynamicIslaned.UserOpenSettingEvent += DynamicIslaned_UserOpenSettingEvent;
                DynamicIslaned.UserOpenMacroLibEvent += DynamicIslaned_UserOpenMacroLibEvent;
                DynamicIslaned.UserSelectAppClickEvent+= DynamicIslaned_UserSelectAppClickEvent;
                DynamicIslaned.ONUseropenAppEvent += ExpendDynamicIsland__ONUseropenAppEvent;

                SettingForm_ = new SettingForm();
                AppGrid.Children.Add(SettingForm_);
                SettingForm_.Setting_MacroLib_Addfolder += SettingForm_MacroLib_Addfolder;
                SettingForm_.Setting_MacroLib_Removerfolder += SettingForm_MacroLib_Removerfolder;
                SettingForm_.Setting_MacroLib_Selectdownfolder += SettingForm_MacroLib_Selectdownfolder;
                SettingForm_.Setting_MacroLib_SelectUpfolder += SettingForm_MacroLib_SelectUpfolder;
                SettingForm_.Setting_MacroLib_ResReadfolder += SettingForm_MacroLib_ResReadfolder;
                SettingForm_.PreviewMouseDown += Usercortol_MoveIndexTop;
            
                string MacorLibFolderslistStr = ConfigINI.ReadSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", "");
                if (MacorLibFolderslistStr != "")
                {
                    foreach (string item in MacorLibFolderslistStr.Split(',')) MCS.MacorLibFolders.Add(item);
                    MCS.DrawMacorLibTreeView();
                }
                SettingForm_.Visibility = Visibility.Hidden;
                SettingForm_.DataContext = MCS;
                MacroLib_ = new MacroLib();
                AppGrid.Children.Add(MacroLib_);
                MacroLib_.OnTreeview_SelectSomthingEnent += MacroLib_Treeview_SelectSomthing;
                MacroLib_.PreviewMouseDown += Usercortol_MoveIndexTop;
                MacroLib_.Visibility = Visibility.Hidden;
                AppGrid.DataContext = MCS;
                Debug_ = new Debug();
                AppGrid.Children.Add(Debug_);
                Debug_.PreviewMouseDown += Usercortol_MoveIndexTop;
                Debug_.Visibility = Visibility.Hidden;
                Debug_.DataContext = MCS;
                EntitySelect_ = new EntitySelect();
                MainFormGrid.Children.Add(EntitySelect_);
                EntitySelect_.Visibility = Visibility.Hidden;
                EntitySelect_.DataContext = MCS;
                MCS.EntitySelectCollection = MCS.PMToolpathList;

            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadControl\r" + ex.ToString());
            }
        }

        private void Usercortol_MoveIndexTop(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bool ischange = false;
                UserControl userControl = (UserControl)sender;
                for (int i = 0; i < AppGrid.Children.Count; i++)
                {
                    UserControl UserControl1 = AppGrid.Children[i] as UserControl;
                    if (UserControl1 == userControl)
                    {
                        ischange = true;
                        continue;
                    }
                    if (ischange)
                    {
                        Canvas.SetZIndex(UserControl1, i - 1);
                    }
                }
                Canvas.SetZIndex(userControl, AppGrid.Children.Count - 1); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usercortol_MoveIndexTop\r"+ex.ToString());
            }
           
        }

        #region DynamicIslaned
        private void DynamicIslaned_UserSelectAppClickEvent(object sender, RoutedEventArgs e)
        {
            if (DynamicIslaned.DynamicIslanedAppBorder.Height == DynamicIslaned.DynamicIslanedAppBorder.MinHeight)
            {
                OpenIsolateLayer();
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation(DynamicIslaned.DynamicIslanedAppBorder.Height, DynamicIslaned.DynamicIslanedAppBorder.MaxHeight, new Duration(TimeSpan.FromSeconds(0.2)),0);
                Storyboard.SetTarget(doubleAnimation, DynamicIslaned.DynamicIslanedAppBorder);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Height"));
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
                MCS.ExpenDynamicIslandCloseEffect_ = 1;
            }
            else
            {
                CloseIsolateLayer();
                Close_ExpendDynamicIslaned();
            }
        }
        private void Close_ExpendDynamicIslaned()
        {
            if (DynamicIslaned.DynamicIslanedAppBorder.Height == DynamicIslaned.DynamicIslanedAppBorder.MaxHeight)
            {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation(DynamicIslaned.DynamicIslanedAppBorder.Height, DynamicIslaned.DynamicIslanedAppBorder.MinHeight, new Duration(TimeSpan.FromSeconds(0.2)));
                Storyboard.SetTarget(doubleAnimation, DynamicIslaned.DynamicIslanedAppBorder);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Height"));
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
                MCS.ExpenDynamicIslandCloseEffect_ = 0;
            }
  
        }
        private void DynamicIslaned_UserOpenMacroLibEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                MacroLib_.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DynamicIslaned_UserOpenMacroLibEvent\r" + ex.ToString());
            }

        }
        private void DynamicIslaned_UserOpenSettingEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingForm_.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DynamicIslaned_UserOpenSettingEvent\r" + ex.ToString());
            }

        }

        private void DynamicIslaned_UserSaveOtherWhereEvent(object sender, RoutedEventArgs e)
        {

            PMCom("FORM RIBBON BACKSTAGE CLOSE PROJECT SAVE AS FILESAVE");
        }

        private void DynamicIslaned_UserSaveProgEvent(object sender, RoutedEventArgs e)
        {
            PMCom("PROJECT SAVE");
        }
        private void DynamicIslaned_UserSelectProgEvent(object sender, RoutedEventArgs e)
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
                MessageBox.Show("DynamicIslaned_UserSelectProgEvent\r" + ex.ToString());
            }


        }
        #endregion

        #region EntitySelect

        #endregion

        #region SettingForm
        private void SettingForm_MacroLib_ResReadfolder(object sender, RoutedEventArgs e)
        {
            try
            {
                MCS.DrawMacorLibTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_MacroLib_ResReadfolder\r" + ex.ToString());
            }

        }
        private void SettingForm_MacroLib_SelectUpfolder(object sender, RoutedEventArgs e)
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
                MessageBox.Show("SettingForm_MacroLib_SelectUpfolder\r" + ex.ToString());
            }


        }

        private void SettingForm_MacroLib_Selectdownfolder(object sender, RoutedEventArgs e)
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
                MessageBox.Show("SettingForm_MacroLib_Selectdownfolder\r" + ex.ToString());
            }


        }

        private void SettingForm_MacroLib_Removerfolder(object sender, RoutedEventArgs e)
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
                MessageBox.Show("SettingForm_MacroLib_Removerfolder\r" + ex.ToString());
            }
        }

        private void SettingForm_MacroLib_Addfolder(object sender, RoutedEventArgs e)
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

        #region OpenExpendApp
        private void ExpendDynamicIsland__ONUseropenAppEvent(string appName)
        {
            switch (appName)
            {
                case "NC输出":
                    NcoutStart();
                    break;
                case "Log":
                    Debug_.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
            CloseIsolateLayer();
            Close_ExpendDynamicIslaned();
            Usercortol_MoveIndexTop(Debug_, null);
        }
        #endregion

        #region MacroLib
        private void MacroLib_Treeview_SelectSomthing(NamePath namePath)
        {
            try
            {
                PMCom($"Macro '{namePath.FullPath}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("MacroLib_Treeview_SelectSomthing\r" + ex.ToString());
            }
        }
        #endregion

        #region NCout
        NCout NCout_ = null;
        private void NcoutStart()
        {
            try
            {
                if (NCout_ != null)
                {
                    NCout_.Visibility = Visibility.Visible;
                }
                else
                {
                    NCout_ = new NCout();
                    NCout_.Visibility = Visibility.Visible;
                    AppGrid.Children.Add(NCout_);
                    NCout_.Ncout_UserselectNcoutWorkplant += NCout_Ncout_UserselectNcoutWorkplant;
                    NCout_.PreviewMouseDown += Usercortol_MoveIndexTop;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NcoutStart\r"+ex.ToString());
            }
       
        }

        private void NCout_Ncout_UserselectNcoutWorkplant(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                EntitySelect_.Opacity = 0;
                EntitySelect_.Height = 0;
                Window window =Window.GetWindow(button);
                System.Windows.Point point =button.TransformToAncestor(window).Transform(new System.Windows.Point(0,0));
                EntitySelect_.Margin = new Thickness(point.X, point.Y +20, 0, 0);
                EntitySelect_.Visibility = Visibility.Visible;
                MCS.EntitySelectCollection.Clear();
                string GetPmEntityCollectionPath = GetPMVal("join(apply(extract(folder('workplane'),'name'),\"pathname('workplane',this)\"),',')");
                Storyboard storyboard = new Storyboard();
                DoubleAnimation doubleAnimation1 = new DoubleAnimation(0, EntitySelect_.MaxHeight, new Duration(TimeSpan.FromSeconds(0.2)), 0);
                Storyboard.SetTarget(doubleAnimation1, EntitySelect_);
                Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath("Height"));
                storyboard.Children.Add(doubleAnimation1);
                DoubleAnimation doubleAnimation2 = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.2)), 0);
                Storyboard.SetTarget(doubleAnimation2, EntitySelect_);
                Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("Opacity"));
                storyboard.Children.Add(doubleAnimation2);
                storyboard.Begin();
                foreach (string item in GetPmEntityCollectionPath.Split(',')) CreateSelectEntityTreeviewNoodPath(item.Replace("Workplane\\", ""), MCS.PMWorkplaneList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_Ncout_UserselectNcoutWorkplant\r"+ex.ToString());
            }
        }
        private void Open_EntitySelect_Grid()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show("Open_EntitySelect_Grid\r"+ex.ToString());
            }
        }


        private void CreateSelectEntityTreeviewNoodPath(string noodpath_ , ObservableCollection<PMEntity> EntityCollection)
        {
            try
            {
                if (noodpath_.IndexOf('\\') > 0)
                {
                    string rootpath_ = noodpath_.Substring(0, noodpath_.LastIndexOf('\\'));
                    string noodname_ = noodpath_.Substring(noodpath_.LastIndexOf('\\') + 1);

                    if (!SelectEntityTreeviewRootPathDic.ContainsKey(rootpath_))
                    {
                        CreateSelectEntityTreeviewRootPath(rootpath_, EntityCollection);
                    }
                    PMEntity foundEntity = EntityCollection.FirstOrDefault(e => e.Name == noodname_);
                    foundEntity.isRootTree = false;
                    foundEntity.RootPath = noodpath_;
                    SelectEntityTreeviewRootPathDic[rootpath_].Children.Add(foundEntity);
                }
                else
                {

                    PMEntity foundEntity = EntityCollection.FirstOrDefault(e => e.Name == noodpath_);
                    foundEntity.isRootTree = false;
                    foundEntity.RootPath = "";
                    MCS.EntitySelectCollection.Add(foundEntity);
                }
              
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateSelectEntityTreeviewNoodPath\r" + ex.ToString());
            }
  
        }
        private void CreateSelectEntityTreeviewRootPath(string rootpath_, ObservableCollection<PMEntity> EntityCollection)
        {
            try
            {
                if (rootpath_.IndexOf('\\') > 0)
                {
                    string rootrootpath_ = rootpath_.Substring(0, rootpath_.LastIndexOf('\\'));
                    if (!SelectEntityTreeviewRootPathDic.ContainsKey(rootpath_))
                    {
                        CreateSelectEntityTreeviewRootPath(rootpath_, EntityCollection);
                    }
                    else
                    {
                        PMEntity rootEntity = new PMEntity();
                        rootEntity.Name = rootrootpath_;
                        rootEntity.RootPath = rootpath_;
                        rootEntity.isRootTree = true;
                        SelectEntityTreeviewRootPathDic.Add(rootpath_, rootEntity);
                        SelectEntityTreeviewRootPathDic[rootpath_].Children.Add(rootEntity);
                    }
                }
                else
                {
                    PMEntity rootEntity = new PMEntity();
                    rootEntity.Name = rootpath_;
                    rootEntity.RootPath = rootpath_;
                    rootEntity.isRootTree = true;
                    SelectEntityTreeviewRootPathDic.Add(rootpath_, rootEntity);
                    MCS.EntitySelectCollection.Add(rootEntity);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CreateSelectEntityTreeviewRootPath\r" + ex.ToString());
            }
         
           

        }
        private Dictionary<string, PMEntity> SelectEntityTreeviewRootPathDic = new Dictionary<string, PMEntity>();



        #endregion

        #endregion

        #region 隔离层
        private void OpenIsolateLayer()
        {
            MCS.IsolateShow = true;
        }
        private void CloseIsolateLayer()
        {
            MCS.IsolateShow = false;
        }
        private void TouchIsolate(object sender, MouseButtonEventArgs e)
        {
            MCS.IsolateShow = false;    
            Close_ExpendDynamicIslaned();
        }

        #endregion

        #region System
        private void ReadSetting()
        {
            MCS.DynamicIslandIsVisibility = ConfigINI.ReadSetting(MCS.ConfigInitPath, "DynamicIslaned", "DynamicIslandIsVisibility", "1") == "1" ? true : false;
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

        #endregion

  
    }
}
