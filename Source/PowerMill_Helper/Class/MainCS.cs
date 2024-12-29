using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PowerMill_Helper.Theme;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;


namespace PowerMill_Helper.Class
{
   internal class MainCS : INotifyPropertyChanged
    {
        public MainCS() {

            try
            {
                _ = StartOpacityAnimation();
                SaveStateColor = new SolidColorBrush(Colors.GreenYellow);
                SaveStateEffect = Colors.GreenYellow;
                ProjectName = "NULL";
                PluginPath = this.GetType().Assembly.Location;
                FileInfo fileInfo_ = new FileInfo(PluginPath);
                PluginFolder = fileInfo_.Directory.FullName;
                ConfigInitPath = System.IO.Path.Combine(PluginFolder, "config.ini");
                PMEmtitys_Setup();
                IsolateShow_ = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Model启动错误\r"+ex.ToString());
            }
          

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void Onchange(string PropertyName)
        {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Info
        private string ProjectName_;
        public string ProjectName
        {
            get { return ProjectName_; }
            set
            {
                ProjectName_ = value;
                OnPropertyChanged();
            }
        }
        #region Path
        // System.Environment.CurrentDirectory;Pm路径
        public string PluginPath { get; set; }
        public string PluginFolder {  get; set; }
        public string ConfigInitPath { get; set; }
        #endregion
        #region Debug
        private string Debugstr_ = "";
        public string Logstr { get => Debugstr_; set { Debugstr_=value;Onchange("Logstr");} }
        public void LogVoid(string str)
        {
            if (Debugstr_.Length > 110000) Debugstr_ = "";
            Debugstr_ = str + "\r" + Debugstr_;
            Onchange("Logstr");
        }
        #endregion


        #endregion
        #region System
        private bool IsolateShow_;
        public bool IsolateShow { 
            get=>IsolateShow_;
            set
            {
                IsolateShow_ = value;
                if (value)
                {
                    AppGirdBlurRadius = 15;
                    Onchange("AppGirdBlurRadius");
                    IsolateTouch = Visibility.Visible;
                    Onchange("IsolateTouch");
                }
                else
                {
                    AppGirdBlurRadius = 0;
                    Onchange("AppGirdBlurRadius");
                    IsolateTouch = Visibility.Hidden;
                    Onchange("IsolateTouch");
                }
                
            }
        } 
        public double AppGirdBlurRadius { get; set; } = 0;
        public Visibility IsolateTouch { get; set; } = Visibility.Hidden;

        #endregion
        #region PmEntity
        private void PMEmtitys_Setup()
        {
            PMEmtitys = new Dictionary<string, ObservableCollection<PMEntity>>();
            PMEmtitys.Add("NCprogram", PMNCprogramList);
            PMEmtitys.Add("Workplane", PMWorkplaneList);
            PMEmtitys.Add("Tool", PMToolList);
            PMEmtitys.Add("Boundary", PMBoundaryList);
            PMEmtitys.Add("Pattern", PMPatternList);
            PMEmtitys.Add("Stockmode", PMStockmodelList);
            PMEmtitys.Add("Group", PMGroupList);
            PMEmtitys.Add("Featuregroup", PMFeaturegroupList);
            PMEmtitys.Add("Featureset", PMFeaturesetList);
            PMEmtitys.Add("Machinetool", PMMachinetoolList);
            PMEmtitys.Add("Toolpath", PMToolpathList);
        }
        public Dictionary<string, ObservableCollection<PMEntity>> PMEmtitys { get; set; }

        public ObservableCollection<PMEntity> PMNCprogramList { get; set; } = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> PMWorkplaneList { get; set; } = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> PMToolList { get; set; } = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> PMBoundaryList { get; set; } = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> PMPatternList { get; set; } = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> PMStockmodelList { get; set; } = new ObservableCollection<PMEntity>();
        public  ObservableCollection<PMEntity> PMGroupList { get; set; } = new ObservableCollection<PMEntity>();
        public  ObservableCollection<PMEntity> PMFeaturegroupList { get; set; } = new ObservableCollection<PMEntity>();
        public  ObservableCollection<PMEntity> PMFeaturesetList { get; set; } = new ObservableCollection<PMEntity>();
        public  ObservableCollection<PMEntity> PMMachinetoolList { get; set; } = new ObservableCollection<PMEntity>();
        public  ObservableCollection<PMEntity> PMToolpathList { get; set; } = new ObservableCollection<PMEntity>();

        private ObservableCollection<PMEntity> EntitySelectCollection_ = new ObservableCollection<PMEntity>();

        public ObservableCollection<PMEntity> EntitySelectCollection { get => EntitySelectCollection_; set { EntitySelectCollection_ = value; OnPropertyChanged(); } }

       

        #endregion
        #region 设置窗口
        #region DynamicIsland
        public Visibility DynamicIslandOpen { get; set; } = Visibility.Visible;
        private bool? DynamicIslandIsVisibility_ { get; set; } = true;
        public bool? DynamicIslandIsVisibility {
            get => DynamicIslandIsVisibility_;
            set { 
                DynamicIslandIsVisibility_ = value;
                if (value==true)
                {
                    DynamicIslandOpen = Visibility.Visible;
                }
                else
                {
                    DynamicIslandOpen = Visibility.Hidden;
                }
                Onchange("DynamicIslandIsVisibility_");
                Onchange("DynamicIslandOpen");
                ConfigINI.WriteSetting(ConfigInitPath, "DynamicIslaned", "DynamicIslandIsVisibility", value == true ? "1" : "0");
                //MessageBox.Show(value == true ? "1" : "0");
            } }
        private double ExpenDynamicIslandOpenEffect_ = 0;
        public double ExpenDynamicIslandCloseEffect_ { get => ExpenDynamicIslandOpenEffect_; set { ExpenDynamicIslandOpenEffect_ = value; OnPropertyChanged(); } }



        #endregion
        #region 宏库

        private ObservableCollection<string> MacorrLibFolders_=new ObservableCollection<string>();
        public ObservableCollection<string> MacorLibFolders
        {
            get=> MacorrLibFolders_;
            set
            {
                MacorrLibFolders_ = value;
               // System.Windows.Forms.MessageBox.Show(string.Join(",", value.ToArray()));
                OnPropertyChanged();
            }
        }
 
        public String MacorLibFolderssettingSelected { get; set; }
        public void DrawMacorLibTreeView()
        {
            try
            {
                MacorLibTreeViewList = new ObservableCollection<NamePath>();
                foreach (string item in MacorrLibFolders_)
                {
                    if (Directory.Exists(item))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(item);
                        NamePath namePath = new NamePath();
                        MacorLibTreeViewList.Add(namePath);
                        ReadMacorLibFolders(namePath, directoryInfo.FullName);
                    }
                }
                Onchange("MacorLibTreeViewList");
            }
            catch (Exception ex)
            {
                MessageBox.Show("DrawMacorLibTreeView\r"+ex.ToString());
            }
            
        }
        public ObservableCollection<NamePath> MacorLibTreeViewList{get;  set;  }=new ObservableCollection<NamePath>();
        private void ReadMacorLibFolders(NamePath ParentNode,string Path)
        {
            try
            {
                //NamePath namePath = new NamePath();
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                ParentNode.PathStr = directoryInfo.Name;
                ParentNode.IconPath = @"Image\OpenFile.png";
                ParentNode.isFolder = true;
                foreach (DirectoryInfo item in directoryInfo.GetDirectories())
                {
                    NamePath namePath_ = new NamePath();
                    namePath_.PathStr = item.Name;
                    ParentNode.Children.Add(namePath_);
                    ReadMacorLibFolders(namePath_, item.FullName);
                }
                foreach (FileInfo item in directoryInfo.GetFiles())
                {
                    NamePath namePath_ = new NamePath();
                    if (item.Extension != "")
                    {
                        namePath_.PathStr = item.Name.Replace(item.Extension, "");
                    }
                    else
                    {
                        namePath_.PathStr = item.Name;
                    }
                    namePath_.FullPath = item.FullName;
                    namePath_.isFolder = false;
                    ParentNode.Children.Add(namePath_);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadMacorLibFolders\r"+ex.ToString());
            }
           
        }

        #endregion
        #endregion
        #region 保存状态
        private bool SaveState_;
        public bool SaveState
        {
            get { return SaveState_; }
            set
            {
                SaveState_ = value;
                if (value)
                {
                    SaveStateColor = new SolidColorBrush(Colors.GreenYellow);
                    SaveStateEffect=Colors.GreenYellow;
                }
                else
                {
                    SaveStateColor = new SolidColorBrush(Colors.OrangeRed);
                    SaveStateEffect=Colors.OrangeRed;
                }
            }
        }
        private SolidColorBrush SaveStateColor_; 
        public SolidColorBrush SaveStateColor
        {
            get { return SaveStateColor_; }
            set
            {
                SaveStateColor_ = value;
                OnPropertyChanged();
            }
        }
        private Color SaveStateEffect_;
        public Color SaveStateEffect
        {
            get { return SaveStateEffect_; }
            set
            {
                SaveStateEffect_ = value;
                OnPropertyChanged();
            }
        }

        private double SaveStateOpacity_;
        public double SaveStateOpacity
        {
            get => SaveStateOpacity_;
            set
            {
                SaveStateOpacity_ = value;
                OnPropertyChanged();
            }
        }
        public async Task StartOpacityAnimation()
        {
            while (true)
            {
                // 渐亮
                for (double i = 0.0; i <= 0.5; i += 0.05)
                {
                    SaveStateOpacity = i;
                    await Task.Delay(50); // 控制动画速度
                }

                // 渐灭
                for (double i = 0.5; i >= 0.0; i -= 0.05)
                {
                    SaveStateOpacity = i;
                    await Task.Delay(50); // 控制动画速度
                }
            }
        }
        #endregion
        #region Ncout
        private PMEntity Ncout_Selected_OutputWorkplane_=new PMEntity() { Name=""};
        public PMEntity Ncout_Selected_OutputWorkplane { get => Ncout_Selected_OutputWorkplane_;set { Ncout_Selected_OutputWorkplane_ = value;OnPropertyChanged(); } }
        #endregion




    }
}
