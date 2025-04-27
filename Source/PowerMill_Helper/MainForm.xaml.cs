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

                //new Thread(AutoOpenBD).Start();

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
                DynamicIslaned = new DynamicIslaned(MCS, PmServices);
                MainFormGrid.Children.Add(DynamicIslaned);
                DynamicIslaned.UserSelectAppClickEvent+= DynamicIslaned_UserSelectAppClickEvent;
                DynamicIslaned.ONUseropenAppEvent += ExpendDynamicIsland__ONUseropenAppEvent;

                SettingForm_Start();

                EntitySelect_ = new EntitySelect();
                MainFormGrid.Children.Add(EntitySelect_);
                EntitySelect_.OnEntitySelect_SelectSomthingEnent += EntitySelectSomething;
                EntitySelect_.Visibility = Visibility.Hidden;
                EntitySelect_.DataContext = MCS;

                Debug_ = new Debug();
                AppGrid.Children.Add(Debug_);
                Debug_.PreviewMouseDown += Usercortol_MoveIndexTop;
                Debug_.Visibility = Visibility.Hidden;
                Debug_.DataContext = MCS;

                /*
                CheckTP_ = new CheckTP(MCS, PmServices);
                AppGrid.Children.Add(Debug_);
                CheckTP_.PreviewMouseDown += Usercortol_MoveIndexTop;
                CheckTP_.Visibility = Visibility.Hidden;
                CheckTP_.DataContext = MCS;
                */

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

        #endregion

        #region DynamicIslaned
        private DynamicIslaned DynamicIslaned = null;
        #region 灵动岛打开拓展窗口
        private void DynamicIslaned_UserSelectAppClickEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DynamicIslaned.DynamicIslanedAppBorder.Height == DynamicIslaned.DynamicIslanedAppBorder.MinHeight)
                {
                    OpenIsolateLayer();
                    Storyboard storyboard = new Storyboard();
                    DoubleAnimation doubleAnimation = new DoubleAnimation(DynamicIslaned.DynamicIslanedAppBorder.Height, DynamicIslaned.DynamicIslanedAppBorder.MaxHeight, new Duration(TimeSpan.FromSeconds(0.2)), 0);
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
            catch (Exception ex)
            {
                MessageBox.Show("DynamicIslaned_UserOpenMacroLibEvent\r" + ex.ToString());
            }

        }
        #endregion
        #region 灵动岛关闭拓展窗口
        private void Close_ExpendDynamicIslaned()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("DynamicIslaned_UserOpenMacroLibEvent\r" + ex.ToString());
            }


        }
        #endregion
        #endregion

        #region 灵动岛打开指定功能
        private void ExpendDynamicIsland__ONUseropenAppEvent(string appName)
        {
            switch (appName)
            {
                case "NC输出":
                    NcoutStart();
                    break;
                case "MacroLib":
                    MacroLib_Startup();
                    break;
                case "Log":
                    Debug_.Visibility = Visibility.Visible;
                    break;
                case "SettingForm":
                    SettingForm_.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            CloseIsolateLayer();
            Close_ExpendDynamicIslaned();
            Usercortol_MoveIndexTop(Debug_, null);
        }
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
        private MacroLib MacroLib_ = null;
        private void MacroLib_Startup()
        {
            if (MacroLib_ != null)
            {
                MacroLib_.Visibility = Visibility.Visible;
            }
            else
            {
                MacroLib_ = new MacroLib(MCS, PmServices);
                AppGrid.Children.Add(MacroLib_);
                MacroLib_.OnTreeview_SelectSomthingEnent += MacroLib_Treeview_SelectSomthing;
                MacroLib_.PreviewMouseDown += Usercortol_MoveIndexTop;
                MacroLib_.Visibility = Visibility.Hidden;

                return;
            }
        }
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
        private NCout NCout_ = null;
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
                    NCout_ = new NCout(MCS,PmServices);
                    NCout_.Visibility = Visibility.Visible;
                    AppGrid.Children.Add(NCout_);
                    NCout_.Ncout_UserselectNcoutWorkplant += UserselectEntity;
                    NCout_.PreviewMouseDown += Usercortol_MoveIndexTop;
                    return;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("NcoutStart\r" + ex.ToString());
            }
        }

        #endregion

        #region CheckTP
        private CheckTP CheckTP_;
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
                    SettingForm_ = new SettingForm(MCS, PmServices);
                    AppGrid.Children.Add(SettingForm_);
                    SettingForm_.PreviewMouseDown += Usercortol_MoveIndexTop;
                    SettingForm_.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SettingForm_Start\r" + ex.ToString());
            }
        }

  

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
