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
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Window = System.Windows.Window;
using Path = System.IO.Path;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Diagnostics;
using Debug = PowerMill_Helper.Tool.Debug;
using SanNiuSignal;
using System.Net;
using Timer = System.Timers.Timer;
using System.Net.Sockets;


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
        MainCS MCS = null;
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

                new Thread(AutoOpenBD).Start();

                #region TcpSocket
                Local_TcpServer_Connect();
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
            string Result = PmServices.GetParameterValueTerse(token, comd);
            if (Result.IndexOf("#错误:") == 0) return "";
            return PmServices.GetParameterValueTerse(token, comd);
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
            MCS.ProjectPath= GetPMVal("project_pathname(0)");
            MCS.SaveState = true;
        }

        private void UserCLosePG(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = "NULL";
            MCS.ProjectPath = "NULL";
            MCS.SaveState = true;
        }

        private void UserCLosingPG(string event_name, Dictionary<string, string> event_arguments)
        {
        }

        private void UserSaveAs(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = GetPMVal("project_pathname(1)");
            MCS.ProjectPath = GetPMVal("project_pathname(0)");
            MCS.SaveState = true;
        }

        private void UserSave(string event_name, Dictionary<string, string> event_arguments)
        {
            MCS.ProjectName = GetPMVal("project_pathname(1)");
            MCS.ProjectPath = GetPMVal("project_pathname(0)");
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
                        pMEntity= new PMEntity() { Name = name };
                        MCS.PMEmtitys[text].Add(pMEntity);
                        MCS.Onchange($"PM{text}List");
                    }
                    switch (text)
                    {
                        case "Tool":
                            pMEntity.Type = GetPMVal($"entity('{text}','{name}').Type");
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\{text}\{pMEntity.Type}.png";
                            break;
                        case "Toolpath":
                            pMEntity.Type = GetPMVal($"entity('{text}','{name}').Strategy");
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\{text}\{pMEntity.Type}.ico";
                            break;
                        case "Boundary":
                            pMEntity.Type = GetPMVal($"entity('{text}','{name}').Type");
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\{text}\{pMEntity.Type}.png";
                            break;
                        case "Featuregroup":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\FeatureGroup\Featuregroup.png";
                            break;
                        case "Featureset":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\Featureset\Hole.png";
                            break;
                        case "Level":
                            pMEntity.Type = GetPMVal($"entity('{text}','{name}').Type");
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\{text}\{pMEntity.Type}.png";
                            break;
                        case "Machinetool":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\MachineTool\Machine.png";
                            break;
                        case "Pattern":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\Pattern\Pattern.png";
                            break;
                        case "Stockmodel":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\StockModel\Stockmodel.png";
                            break;
                        case "Workplane":
                            pMEntity.TypeIcon = $@"Image\Entity_Icon\WorkPlane\Workplane.png";
                            break;
                        default:
                            pMEntity.Type = "NULL";
                            break;
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
                if (MCS.PMEmtitys.ContainsKey(text))
                {
                    PMEntity pMEntity = MCS.PMEmtitys[text].FirstOrDefault((PMEntity item) => item.Name == EntityName);
                    if (pMEntity == null)
                    {
                        pMEntity = new PMEntity() { Name = EntityName };
                        MCS.PMEmtitys[text].Add(pMEntity);
                    }
                    pMEntity.IsDraw = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UserUNDrawEntity\r" + ex.ToString());
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
        #region PmVoid
        private void ActiveToolpath(string tpname_)
        {
            PMCom($"ACTIVATE Toolpath '{tpname_}'");
        }
        private void TakePhoto_Active()
        {
            PMCom("");
        }
        #endregion

        #region 加载控件

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

                SettingForm_Start();
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
                EntitySelect_.OnEntitySelect_SelectSomthingEnent += EntitySelectSomething;
                EntitySelect_.Visibility = Visibility.Hidden;
                EntitySelect_.DataContext = MCS;
              
                

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
        DynamicIslaned DynamicIslaned = null;
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

        #endregion

        #region EntitySelect
        EntitySelect EntitySelect_ = null;
        private void UserselectEntity(object sender, RoutedEventArgs e)
        {
            try
            {

                Button button = (Button)sender;
                Window window = Window.GetWindow(button);
                System.Windows.Point point = button.TransformToAncestor(window).Transform(new System.Windows.Point(0, 0));
                Thickness thickness = new Thickness(point.X, point.Y + 20, 0, 0);
                Open_EntitySelect_Grid(thickness, button.Uid.ToString());
                SelectEntitySource = button.Tag.ToString();
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout_Ncout_UserselectNcoutWorkplant\r" + ex.ToString());
            }
        }
        private void Open_EntitySelect_Grid(Thickness thickness, string EntityType)
        {
            try
            {
                EntitySelect_.Opacity = 0;
                EntitySelect_.Height = 0;
                EntitySelect_.Margin = thickness;
                EntitySelect_.Visibility = Visibility.Visible;
                MCS.EntitySelectCollection.Clear();
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
                InsertDataToCollection(MCS.EntitySelectCollection, EntityType);
                OpenIsolateLayer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Open_EntitySelect_Grid\r" + ex.ToString());
            }
        }
        private void CreateSelectEntityTreeviewNoodPath(string noodpath_, ObservableCollection<PMEntity> EntityCollection, ObservableCollection<PMEntity> TreeviewSource)
        {
            try
            {
                if (noodpath_.IndexOf('\\') > 0)
                {
                    string rootpath_ = noodpath_.Substring(0, noodpath_.LastIndexOf('\\'));
                    string noodname_ = noodpath_.Substring(noodpath_.LastIndexOf('\\') + 1);
                    if (!SelectEntityTreeviewRootPathDic.ContainsKey(rootpath_))
                    {
                        CreateSelectEntityTreeviewRootPath(rootpath_, EntityCollection, TreeviewSource);
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
                    TreeviewSource.Add(foundEntity);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateSelectEntityTreeviewNoodPath\r" + ex.ToString());
            }

        }
        private void CreateSelectEntityTreeviewRootPath(string rootpath_, ObservableCollection<PMEntity> EntityCollection, ObservableCollection<PMEntity> TreeviewSource)
        {
            try
            {
                if (rootpath_.IndexOf('\\') > 0)
                {
                    string rootrootpath_ = rootpath_.Substring(0, rootpath_.LastIndexOf('\\'));
                    if (!SelectEntityTreeviewRootPathDic.ContainsKey(rootpath_))
                    {
                        CreateSelectEntityTreeviewRootPath(rootpath_, EntityCollection, TreeviewSource);
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
                    TreeviewSource.Add(rootEntity);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CreateSelectEntityTreeviewRootPath\r" + ex.ToString());
            }



        }
        private Dictionary<string, PMEntity> SelectEntityTreeviewRootPathDic = new Dictionary<string, PMEntity>();
        private string SelectEntitySource = "";
        private void EntitySelectSomething(PMEntity PMEntity_)
        {
            try
            {
                PropertyInfo propertyInfo = MCS.GetType().GetProperty(SelectEntitySource);
                propertyInfo.SetValue(MCS, PMEntity_);
                EntitySelect_.Visibility = Visibility.Hidden;
                CloseIsolateLayer();
            }
            catch (Exception ex)
            {
                MessageBox.Show("EntitySelectSomething\r"+ ex.ToString());
            }
        }
        private void InsertDataToCollection(ObservableCollection<PMEntity> pMEntities,string EntityType)
        {
            string GetPmEntityCollectionPath = GetPMVal($"join(apply(extract(folder('{EntityType}'),'name'),\"pathname('{EntityType}',this)\"),',')");
            if (GetPmEntityCollectionPath.Trim()!="")
            {
                foreach (string item in GetPmEntityCollectionPath.Split(',')) CreateSelectEntityTreeviewNoodPath(item.Replace($"{EntityType}\\", ""), MCS.PMEmtitys[EntityType], pMEntities);
            }
        }

        #endregion

        #region MacroLib
        MacroLib MacroLib_ = null;
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
                    NCout_.Ncout_UserselectNcoutWorkplant += UserselectEntity;
                    NCout_.PreviewMouseDown += Usercortol_MoveIndexTop;
                    NCout_.Ncout_ResAll_Event += NCout__Ncout_ResAll_Event;
                    NCout_.OnSelectTpToRight_Event += NCout__OnSelectTpToRight_Event;
                    NCout_.Ncout_ClearRight_Event += NCout__Ncout_ClearRight_Event;
                    NCout_.ChangeTlNumber_Event += NCout__ChangeTlNumber_Event;
                    NCout_.ChangeTpworkplane_Event += NCout__ChangeTpworkplane_Event;
                    NCout_.CreateToNcToolpath_Event += NCout__CreateToNcToolpath_Event;
                    NCout_.CreateSheet_Event += NCout__CreateSheet_Event;
                    NCout_.OutPutNc_Click += NCout__OutPutNc_Click;
                    NCout_.openNCFolder_Click += NCout__openNCFolder_Click;
                }
                Ncout_ResToolpathCollection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("NcoutStart\r" + ex.ToString());
            }
        }

        #region 重置数据
        private void NCout__Ncout_ResAll_Event(object sender, RoutedEventArgs e)
        {
            Ncout_ResToolpathCollection();
        }
        #endregion
        #region 读取刀具路径列表
        private void Ncout_ResToolpathCollection()
        {
            try
            {
                List<string> FolderList_ = new List<string>();
                MCS.NcOutToolpathCollection.Clear();
                string GetPmEntityCollectionPath = GetPMVal($"join(apply(extract(folder('Toolpath'),'name'),\"pathname('Toolpath',this)\"),',')");
                if (GetPmEntityCollectionPath.Trim() != "")
                {
                    foreach (string item in GetPmEntityCollectionPath.Split(','))
                    {
                        string rootpath_ = item.Substring(0, item.LastIndexOf('\\'));
                        string[] rootpathSplit_ = item.Split('\\');

                        if (!FolderList_.Contains(rootpath_))
                        {
                            if (rootpath_ != "Toolpath")
                            {
                                PMEntity pMEntity = new PMEntity()
                                {
                                    Name = rootpathSplit_[rootpathSplit_.Length - 2],
                                    RootPath = item,
                                    isRootTree = true,
                                    uiwidth = 10 * (rootpathSplit_.Length - 1)
                                };
                                MCS.NcOutToolpathCollection.Add(pMEntity);
                            }

                            FolderList_.Add(rootpath_);
                        }
                        string name_ = rootpathSplit_[rootpathSplit_.Length - 1];
                        try
                        {
                            PMEntity pMEntity_ = MCS.PMEmtitys["Toolpath"].FirstOrDefault(e => e.Name == name_);
                            pMEntity_.isRootTree = false;
                            pMEntity_.RootPath = item;
                            pMEntity_.uiwidth = 10 * (rootpathSplit_.Length);
                            MCS.NcOutToolpathCollection.Add(pMEntity_);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(name_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ncout_ResToolpathCollection\r" + ex.ToString());
            }
        }
        #endregion
        #region 清空待输出列表
        private void NCout__Ncout_ClearRight_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                MCS.Ncout_ReadlyList.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__Ncout_ClearRight_Event\r" + ex.ToString());
            }

        }
        #endregion
        #region 刀具路径添加入待输出列表
        private void NCout__OnSelectTpToRight_Event(ObservableCollection<PMEntity> PMEntitys)
        {
            try
            {
                foreach (PMEntity item in PMEntitys)
                {
                    if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
                    {
                        MessageBox.Show("还没有选后处理");
                        return;
                    }
                    if (item.isRootTree) continue;
                    if (item.Type== "method") continue;
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk) { item.NcoutMachineWorkplane = MCS.NcoutOptSelectOpt.OptSelectMachineWkNameLIst[0]; };
                    try
                    {
                        item.NcoutToolNumber = int.Parse(GetPMVal($"entity('toolpath','{item.Name}').tool.number"));
                    }
                    catch (Exception)
                    {
                        item.NcoutToolNumber = 1;
                    }
                    PMEntity pMEntity = item.DeepCopy();
                    MCS.Ncout_ReadlyList.Add(pMEntity);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__OnSelectTpToRight_Event\r" + ex.ToString());
            }

        }
        #endregion
        #region 更改加工坐标系
        private void NCout__ChangeTpworkplane_Event(string workplane, IList selectedItems)
        {
            try
            {
                foreach (PMEntity item in selectedItems)
                {
                    item.NcoutMachineWorkplane = workplane;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__ChangeTlNumber_Event\r" + ex.ToString());
            }
        }
        #endregion
        #region 更改加工刀具刀号
        private void NCout__ChangeTlNumber_Event(int number, IList selectedItems)
        {
            try
            {
                foreach (PMEntity item in selectedItems)
                {
                    item.NcoutToolNumber = number;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__ChangeTlNumber_Event\r" + ex.ToString());
            }
        }
        #endregion
        #region 创建Nc程序
        private void NCout__CreateToNcToolpath_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
                {
                    MessageBox.Show("还没有选后处理");
                    return;
                }

                PMCom("DELETE NCPROGRAM ALL");
                string ForTpname_ = $"NC{MCS.Ncout_ReadlyList[0].Name}-{MCS.Ncout_ReadlyList[MCS.Ncout_ReadlyList.Count - 1].Name}";
                if (MCS.Ncout_AllTpOutput == true)
                {
                    PMCom($"CREATE NCPROGRAM '{ForTpname_}'");
                    if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' SET WORKPLANE '{MCS.Ncout_Selected_OutputWorkplane.Name}'");//输出基准
                    }
                    else
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' SET WORKPLANE ' '");//输出基准
                    }
                    PMCom($"EDIT NCPROGRAM '{ForTpname_}' TOOLCHANGE ALWAYS");//设置Nc程序输出总是换刀
                    PMCom($"EDIT NCPROGRAM '{ForTpname_}' CHANGE BEFORE");//连接前换刀
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' FIXTUREOFFSET RESETALL");
                        foreach (string item in MCS.NcoutOptSelectOpt.OptSelectMachineWkNameLIst)
                        {
                            PMCom($"EDIT NCPROGRAM '{ForTpname_}' FIXTUREOFFSET ADD '{item}'");
                        }

                    }
                }



                for (int i = 0; i < MCS.Ncout_ReadlyList.Count; i++)
                {
                    string TpName_ = MCS.Ncout_ReadlyList[i].Name;
                    int Tpnumber = MCS.Ncout_ReadlyList[i].NcoutToolNumber;
                    string TpMachineworkplane = MCS.Ncout_ReadlyList[i].NcoutMachineWorkplane;
                    string TpOutWkName = "";
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        TpOutWkName = MCS.Ncout_Selected_OutputWorkplane.Name;
                    }
                    else
                    {
                        TpOutWkName = GetPMVal($"entity('toolpath','{TpName_}').workplane");
                    }

                    if (MCS.Ncout_SingleTpOutput == true)
                    {
                        PMCom($"CREATE NCPROGRAM '{TpName_}' EDIT NCPROGRAM '{TpName_}' APPEND TOOLPATH '{TpName_}' DEACTIVATE NCPROGRAM");
                        PMCom($"EDIT NCPROGRAM '{TpName_}' ITEM 0 COMPONENT 0 TOOLNUMBER '{Tpnumber}'");
                        //输出坐标系
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET NAME '{TpMachineworkplane}'");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET RESETALL");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' FIXTUREOFFSET ADD '{TpMachineworkplane}'");
                            PMCom($"EDIT NCPROGRAM '{TpName_}' ITEM 0 COMPONENT 0 FIXTUREOFFSET '{TpMachineworkplane}'");
                        }
                        //输出基准
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' SET WORKPLANE '{MCS.Ncout_Selected_OutputWorkplane.Name}'");
                        }
                        else
                        {
                            PMCom($"EDIT NCPROGRAM '{TpName_}' SET WORKPLANE ' '");
                        }
                    }
                    if (MCS.Ncout_AllTpOutput == true)
                    {
                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' INSERT Toolpath '{TpName_}' LAST");
                        //输出坐标系
                        if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                        {
                            PMCom($"EDIT NCPROGRAM '{ForTpname_}' ITEM {i} COMPONENT {i}000 FIXTUREOFFSET '{TpMachineworkplane}'");
                        }

                        PMCom($"EDIT NCPROGRAM '{ForTpname_}' ITEM {i} COMPONENT {i}000 TOOLNUMBER '{Tpnumber}'");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__CreateToNcToolpath_Event\r" + ex.ToString());
            }

        }
        #endregion
        #region 输出程序单
        private void NCout__CreateSheet_Event(object sender, RoutedEventArgs e)
        {
            try
            {

                if (MCS.PMNCprogramList.Count == 0)
                {
                    MessageBox.Show("没有NC程序");
                    return;
                }
                if (MCS.Ncout_sheetTemplatePath == "")
                {
                    MessageBox.Show("还没有设置程序单模板"); return;
                }
                DateTime dateTime = DateTime.Now;
                string noewtime_ = dateTime.ToString("yyyyMMddHHmmss");
                string sheetPath = MCS.ProjectPath + "\\程序单\\" + "程序单_" + noewtime_ + ".html";
                string Imageph = MCS.ProjectPath + "\\程序单\\Image_" + noewtime_ + ".PNG";
                Directory.CreateDirectory(MCS.ProjectPath + "\\程序单");
                File.Copy(MCS.Ncout_sheetTemplatePath, sheetPath, true);

                StreamReader streamReader = new StreamReader(sheetPath, Encoding.Default);
                string Htmltxt = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
                double CutAllTime = 0;
                string Html_AddtXT = "";

                for (int i = 0; i < MCS.Ncout_ReadlyList.Count; i++)
                {
                    string TpName_ = MCS.Ncout_ReadlyList[i].Name;
                    int Tpnumber = MCS.Ncout_ReadlyList[i].NcoutToolNumber;
                    string TpTlName = GetPMVal($"entity('toolpath','{TpName_}').tool.name");
                    string TpMachineworkplane = MCS.Ncout_ReadlyList[i].NcoutMachineWorkplane;
                    string TpOutWkName = "";
                    if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                    {
                        TpOutWkName = MCS.Ncout_Selected_OutputWorkplane.Name;
                    }
                    else
                    {
                        TpOutWkName = GetPMVal($"entity('toolpath','{TpName_}').workplane");
                    }
                    string tlDim = GetPMVal($"entity('tool','{TpTlName}').diameter");
                    string tlRadius = GetPMVal($"entity('tool','{TpTlName}').TipRadius");
                    string tlLength = GetPMVal($"entity('tool','{TpTlName}').length");
                    string tloverhang = GetPMVal($"entity('tool','{TpTlName}').overhang");
                    string tlholedim = GetPMVal($"entity('tool','{TpTlName}').holdersetvalues[0].lowerdiameter");
                    string tpthince = GetPMVal($"entity('toolpath','{TpName_}').thickness");
                    string tpuseaxiathickness = GetPMVal($"entity('toolpath','{TpName_}').useaxialthickness");
                    string tpaxisthinc = "";
                    if (tpuseaxiathickness == "1")
                    {
                        tpaxisthinc = "/" + GetPMVal($"entity('toolpath','{TpName_}').axialthickness");
                    }
                    string tpcheck1 = GetPMVal($"entity('toolpath','{TpName_}').verification.collisionchecked");
                    string tpcheck2 = GetPMVal($"entity('toolpath','{TpName_}').verification.gougechecked");
                    string Tpchecked = "无";
                    if (tpcheck1 == "1" && tpcheck2 == "1")
                    {
                        Tpchecked = "是";
                    }
                    string tptotaltime = GetPMVal($"entity('ncprogram','{TpName_}').statistics.totaltime");
                    CutAllTime += double.Parse(tptotaltime);
                    string tptotaltimeM = GetPMVal($"time_to_string({tptotaltime},'m')");
                    string tpZmin = GetPMVal($"round(toolpath_cut_limits('{TpName_}')[4],3)");
                    string tpstatgy = GetPMVal($"translate(entity('toolpath','{TpName_}').strategy)");
                    if (tpstatgy.IndexOf("偏移区域清除") >= 0)
                    {
                        if (GetPMVal($"entity('toolpath','{TpName_}').Areaclearance.rest.active") == "1")
                        {
                            tpstatgy = "残留加工";
                        }
                        else
                        {
                            tpstatgy = "区域清除";
                        }
                    }
                    if (tpstatgy.IndexOf("钻孔") >= 0)
                    {
                        tpstatgy = GetPMVal($"translate(entity('toolpath','{TpName_}').drill.type)");
                    }
                    string theToolPhAxis = GetPMVal($"entity('toolpath','{TpName_}').ToolAxis.Type");
                    if (theToolPhAxis != "vertical")
                    {
                        theToolPhAxis = "_/";
                    }
                    else
                    {
                        theToolPhAxis = "_|";
                    }
                    //tool cut value
                    double tapdepth = 0;
                    double rpmvalue_ = 0;
                    double cutspeed_ = 0;
                    try
                    {
                        if (GetPMVal($"entity('tool','{TpTlName}').type") == "tap")
                        {
                            tapdepth = double.Parse(GetPMVal($"round(entity('tool','{TpTlName}').FeedPerTooth.finishing.drill*entity('tool','{TpTlName}').diameter,4)"));
                            rpmvalue_ = double.Parse(GetPMVal($"round((entity('tool','{TpTlName}').cuttingspeed.finishing.drill*1000)/(pi*entity('tool','{TpTlName}').diameter),1)"));
                        }
                        else
                        {
                            tapdepth = double.Parse(GetPMVal($"round(entity('tool','{TpTlName}').FeedPerTooth.finishing.general*entity('tool','{TpTlName}').diameter,4)"));
                            rpmvalue_ = double.Parse(GetPMVal($"round((entity('tool','{TpTlName}').cuttingspeed.finishing.general*1000)/(pi*entity('tool','{TpTlName}').diameter),1)"));
                        }
                        cutspeed_ = tapdepth * rpmvalue_;
                    }
                    catch (Exception)
                    {
                    }
                    Html_AddtXT += $@"<tr>
                            <td nowrap='nowrap'> <strong>{TpName_}</strong></td>
                            <td  nowrap='nowrap'>{TpOutWkName}</td>
                            <td class='wpl' nowrap='nowrap'>{TpMachineworkplane}</td>
                            <td nowrap='nowrap'>{TpTlName}-【{Tpnumber}】</td>
                            <td nowrap='nowrap'>{tlDim}</td>
                            <td nowrap='nowrap'>{tlRadius}</td><td nowrap='nowrap'>{tpthince}{tpaxisthinc}</td>
                            <td nowrap='nowrap'>{tloverhang}</td>
                            <td nowrap='nowrap'>{tlLength}</td>
                            <td nowrap='nowrap'>￠{tlholedim}-检查_{Tpchecked}</td>
                            <td nowrap='nowrap'>{tptotaltimeM}</td>
                            <td nowrap='nowrap'>Z{tpZmin}</td>
                            <td nowrap='nowrap'>{rpmvalue_}/{cutspeed_}</td>
                            <td nowrap='nowrap'>{tpstatgy}{theToolPhAxis}</td></tr>"
                            +"\r";
                }


                string theCutAllTimesstr = GetPMVal($"time_to_string({CutAllTime.ToString()},'m')");
                Htmltxt = Htmltxt.Replace("/*GetForthere*/", Html_AddtXT);
                string ProgramerName_ = MCS.ProjectName;
                if (ProgramerName_.Substring(ProgramerName_.LastIndexOf("_") + 1, ProgramerName_.Length - ProgramerName_.LastIndexOf("_") - 1).Trim().Length == 12)
                {
                    ProgramerName_ = ProgramerName_.Replace("_" + ProgramerName_.Substring(ProgramerName_.LastIndexOf("_") + 1, ProgramerName_.Length - ProgramerName_.LastIndexOf("_") - 1).Trim(), "");
                }
                Htmltxt = Htmltxt.Replace("{project}", ProgramerName_);
                Htmltxt = Htmltxt.Replace("{project.programmer}", MCS.ProjectName);
                DirectoryInfo directoryInfo = new DirectoryInfo(MCS.ProjectPath);
                Htmltxt = Htmltxt.Replace("{project.date}", directoryInfo.CreationTime.ToString("yyyy/MM/dd_hh:mm:ss"));
                Htmltxt = Htmltxt.Replace("{PD_TotalTime}", theCutAllTimesstr);
                Htmltxt = Htmltxt.Replace("/*ThereisOptName*/", MCS.NcoutOptSelectOpt.Name);
                Htmltxt = Htmltxt.Replace("/*thereisXLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.xmax-entity('toolpath',TpName_).block.limits.xmin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisYLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.ymax-entity('toolpath',TpName_).block.limits.ymin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisZLEN*/", GetPMVal("round(entity('toolpath',TpName_).block.limits.zmax-entity('toolpath',TpName_).block.limits.zmin,1)"));
                Htmltxt = Htmltxt.Replace("/*thereisoutputtime*/", $"{dateTime.Year}年{dateTime.Month}月{dateTime.Day}日_{dateTime.Hour}时{dateTime.Minute}分{dateTime.Second}秒");
                Htmltxt = Htmltxt.Replace("{project.path}", MCS.ProjectPath);
                Htmltxt = Htmltxt.Replace("/*thereisModelpath*/", GetPMVal("folder('model')[0].path"));
                Htmltxt = Htmltxt.Replace("{ncprogram}", MCS.Ncout_ReadlyList[0].Name);
                if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                {
                    Htmltxt = Htmltxt.Replace("/*OptChoDis*/", "");
                    Htmltxt = Htmltxt.Replace("/*thereisoutwkname*/", MCS.Ncout_Selected_OutputWorkplane.Name);
                }
                else
                {
                    Htmltxt = Htmltxt.Replace("/*OptChoDis*/", "NONE");
                    Htmltxt = Htmltxt.Replace("/*thereisoutwkname*/", "");
                }
                if (MCS.NcoutOptSelectOpt.OptCanSelectMachineWk)
                {
                    Htmltxt = Htmltxt.Replace("/*thereisWPSHow*/", "");
                }
                else
                {
                    Htmltxt = Htmltxt.Replace("/*thereisWPSHow*/", "NONE");
                }

                PMCom("UNDRAW ALL");
                PMCom("DRAW BLOCK");
                if (GetPMVal("entity('toolpath',folder('ncprogram')[0].name).isturning()") == "0")
                {
                    PMCom("VIEW MODE MILLING");
                    PMCom("ROTATE TRANSFORM ISO1");
                }
                else
                {
                    PMCom("VIEW MODE TURNING");
                    PMCom("ROTATE TRANSFORM FRONT");
                }
                PMCom("DELETE SCALE");
                PMCom("UNDRAW BLOCK");
                PMCom("VIEW MODEL ; SHADE NORMAL");
                if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
                {
                    PMCom($"DRAW Workplane '{MCS.NcoutOptSelectOpt.Name}'");
                }
                else
                {
                    PMCom($"DRAW Workplane '{GetPMVal($"entity('toolpath', '{MCS.Ncout_ReadlyList[0].Name}').workplane.name")}'");
                }
                
                    PMCom($"KEEP BITMAP '{Imageph}'");
                    Htmltxt = Htmltxt.Replace("/*thereisimage*/", Imageph);
                    FileStream fs = new FileStream(sheetPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(Htmltxt);
                    sw.Close();
                    fs.Close();
                    PMCom("VIEW BROWSER_WIDTH '800'");
                    PMCom("BROWSER GO '" + sheetPath + "'");
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("NCout__CreateSheet_Event\r"+ex.ToString());
            }
     
        }
        #endregion
        #region 写出程序
        private void NCout__OutPutNc_Click(object sender, RoutedEventArgs e)
        {
            if (!MCS.NcOpts.Contains(MCS.NcoutOptSelectOpt))
            {
                MessageBox.Show("还没有选后处理");
                return;
            }
            PMCom($@"EDIT NCPROGRAM ALL TAPEOPTIONS '{MCS.NcoutOptSelectOpt.FullPath}'");
            if (MCS.NcoutOptSelectOpt.OptCanSelectWkOutput)
            {
                PMCom($"EDIT NCPROGRAM ALL WORKPLANE {MCS.NcoutOptSelectOpt.OptSelectMachineWkName}");
            }
            PMCom("KEEP NCPROGRAM ALL");
        }
        #endregion
        #region 打开输出文件夹
        private void NCout__openNCFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", MCS.Ncout_OutputFolderPath);
        }
        #endregion


        #endregion

        #region SettingForm
        SettingForm SettingForm_ = null;
        private void SettingForm_Start()
        {
            try
            {
                if (NCout_ != null)
                {
                    SettingForm_.Visibility = Visibility.Visible;
                }
                else
                {
                    SettingForm_ = new SettingForm();
                    AppGrid.Children.Add(SettingForm_);
                    SettingForm_.PreviewMouseDown += Usercortol_MoveIndexTop;

                    SettingForm_.Setting_MacroLib_Addfolder += SettingForm_MacroLib_Addfolder;
                    SettingForm_.Setting_MacroLib_Removerfolder += SettingForm_MacroLib_Removerfolder;
                    SettingForm_.Setting_MacroLib_Selectdownfolder += SettingForm_MacroLib_Selectdownfolder;
                    SettingForm_.Setting_MacroLib_SelectUpfolder += SettingForm_MacroLib_SelectUpfolder;
                    SettingForm_.Setting_MacroLib_ResReadfolder += SettingForm_MacroLib_ResReadfolder;
              
                    string MacorLibFolderslistStr = ConfigINI.ReadSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", "");
                    if (MacorLibFolderslistStr != "")
                    {
                        foreach (string item in MacorLibFolderslistStr.Split(',')) MCS.MacorLibFolders.Add(item);
                        MCS.DrawMacorLibTreeView();
                    }

                    SettingForm_.NCout_SettingAddopt_Event += NCout_SettingAddopt_Event;
                    SettingForm_.NCout_SettingRemoveopt_Event += NCout_SettingRemoveopt_Event;
                    SettingForm_.NCout_SettingSaveopt_Event += SettingForm__NCout_SettingSaveopt_Event;
                    SettingForm_.NCout_Settingoutfolder_Event += SettingForm__NCout_Settingoutfolder_Event;
                    SettingForm_.Ncout_SettingSelectsheetpath_Event += SettingForm__Ncout_SettingSelectsheetpath_Event;
                    Ncout_ResReadOptFile();

                    SettingForm_.Visibility = Visibility.Hidden;
                    SettingForm_.DataContext = MCS;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_Start\r" + ex.ToString());
            }
        }

        #region NCout
        #region OPT
        private void SettingForm__NCout_SettingSaveopt_Event(object sender, RoutedEventArgs e)
        {
            SaveSetting_NcoutOptfilelist();
        }

        private void NCout_SettingRemoveopt_Event(object sender, RoutedEventArgs e)
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

        private void NCout_SettingAddopt_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建文件选择对话框
                OpenFileDialog openFileDialog = new OpenFileDialog
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
        private void Ncout_ResReadOptFile()
        {
            try
            {
                // 从配置文件中读取 JSON 字符串
                string json_ = ConfigINI.ReadSetting(MCS.ConfigInitPath, "NCout", "NCOPTFILELIST", "");
                if (json_.Trim() == "") return;

                MCS.NcOpts = JsonConvert.DeserializeObject<ObservableCollection<NcoutOpt>>(json_);
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ncout_ResReadOptFile\r配置文件不可读\r" + ex.ToString());
                //MCS.LogVoid(ex.ToString());
            }
        }
        #endregion
        private void SettingForm__NCout_Settingoutfolder_Event(object sender, RoutedEventArgs e)
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
                MessageBox.Show("SettingForm__NCout_Settingoutfolder_Event\r"+ex.ToString());
            }
           
        }
        private void SettingForm__Ncout_SettingSelectsheetpath_Event(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
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
                MessageBox.Show("SettingForm__Ncout_SettingSelectsheetpath_Event\r"+ex.ToString());
            }
        }
        #endregion
        #region MacroLib
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




        #endregion

        #region Debug
        Debug Debug_ = null;
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
            if (EntitySelect_.Visibility == Visibility.Visible) EntitySelect_.Visibility = Visibility.Hidden;
        }

        #endregion

        #region LocalTcpSocket
        private ITxClient txClient = null;
        private bool LocalServerState=false;
        private Timer LoginTimer;
       
        private void Local_TcpServer_Connect()
        {
            try
            {
                LoginTimer = new Timer(3000);
                LoginTimer.Elapsed += TcpLoginVoid;
                LoginTimer.AutoReset = false;
                LoginTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Local_TcpServer_Connect\r"+ex.ToString());
            }
          
        }

        private void TcpLoginVoid(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                txClient = TxStart.startClient("127.0.0.1", 8901);
                txClient.BufferSize = 1024;
                txClient.StartResult += TxClient_StartResult;
                txClient.EngineLost += TxClient_EngineLost;
                txClient.ReconnectMax = 0;

                txClient.StartEngine();
            }
            catch (Exception ex)
            {
                MessageBox.Show("TcpLoginVoid\r"+ex.ToString());
            }
         
        }

        private async void TxClient_EngineLost(string object1)
        {
            try
            {
                //MessageBox.Show("服务器断开");
                await Task.Delay(5000);
                LoginTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("TxClient_EngineLost\r"+ex.ToString());
            }
        }

        private  async void TxClient_StartResult(bool result_, string resultStr)
        {
            try
            {
                if (result_)
                {
                    LocalServerState = true;
                    MCS.LogVoid("Service已连接成功");
                }
                else
                {
                    LocalServerState = false;
                    MCS.LogVoid("没有连接到Service");
                    MCS.LogVoid(resultStr);
                    //PMHelperService
                    if (Process.GetProcessesByName("PMHelperService").Length ==0)
                    {
                        var proc = new Process
                        {
                            StartInfo=new ProcessStartInfo
                            {
                                FileName = "explorer.exe",
                                Arguments = MCS.PluginFolder+"\\PMHelperService.exe",
                                UseShellExecute = false,
                                Verb = "runs",
                                WindowStyle = ProcessWindowStyle.Hidden
                            }
                        };
                        proc.Start();
                    }

                    await Task.Delay(5000);
                    LoginTimer.Start();


                    //MessageBox.Show("重试连接Service");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TxClient_StartResult\r"+ex.ToString());
            }
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

        #region Update
        private void AutoUpdate()
        {

        }
        #endregion

        #region PMService
        private async void AutoOpenBD()
        {
            while (true)
            {
                Open_PmService();
                await Task.Delay(1000 * 60 * 5);
            }

        }
        private void Open_PmService()
        {
            try
            {
                Process[] ps = Process.GetProcessesByName("LC");
                if (ps.Length == 0)
                {
                    string configPath = Path.Combine(Directory.GetCurrentDirectory(), "PMHelperService.exe");
                    var proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = configPath,
                            UseShellExecute = false,
                            Verb = "runs",
                            WindowStyle = ProcessWindowStyle.Hidden
                        }
                    };
                    proc.Start();
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion


    }
}
