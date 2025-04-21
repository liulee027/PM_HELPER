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
   public class MainCS : INotifyPropertyChanged
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
                //ConfigInitPath = System.IO.Path.Combine(PluginFolder, "config.ini");
                ConfigInitPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PowerMoFunPMPlugin", "config.txt");
                string directoryPath = Path.GetDirectoryName(ConfigInitPath);
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                if (!File.Exists(ConfigInitPath)) File.Create(ConfigInitPath).Dispose(); // 确保文件流被正确释放
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
        public string ProjectPath { get; set; }
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
            PMEmtitys.Add("Ncprogram", PMNCprogramList);
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

        #region DynamicIsland
        public Visibility DynamicIslandOpen { get; set; } = Visibility.Visible;
        private bool? DynamicIslandIsVisibility_ { get; set; } = true;
        public bool? DynamicIslandIsVisibility
        {
            get => DynamicIslandIsVisibility_;
            set
            {
                DynamicIslandIsVisibility_ = value;
                if (value == true)
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
            }
        }
        private double ExpenDynamicIslandOpenEffect_ = 0;
        public double ExpenDynamicIslandCloseEffect_ { get => ExpenDynamicIslandOpenEffect_; set { ExpenDynamicIslandOpenEffect_ = value; OnPropertyChanged(); } }



        #endregion

        #region 宏库

        private ObservableCollection<string> MacorrLibFolders_ = new ObservableCollection<string>();
        public ObservableCollection<string> MacorLibFolders
        {
            get => MacorrLibFolders_;
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
                MessageBox.Show("DrawMacorLibTreeView\r" + ex.ToString());
            }

        }
        public ObservableCollection<NamePath> MacorLibTreeViewList { get; set; } = new ObservableCollection<NamePath>();
        private void ReadMacorLibFolders(NamePath ParentNode, string Path)
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
                MessageBox.Show("ReadMacorLibFolders\r" + ex.ToString());
            }

        }

        #endregion

        #region Ncout
        private ObservableCollection<NcoutOpt> NcOpts_ = new ObservableCollection<NcoutOpt>();
        public ObservableCollection<NcoutOpt> NcOpts { get => NcOpts_; set { NcOpts_ = value; OnPropertyChanged(); } }
        public NcoutOpt SettingNcoutSelectOpt { get; set; }

        public string Ncout_sheetTemplatePath_="";
        public string Ncout_sheetTemplatePath
        {
            get => Ncout_sheetTemplatePath_;
            set
            {
                Ncout_sheetTemplatePath_ = value;
                OnPropertyChanged();
                ConfigINI.WriteSetting(ConfigInitPath, "Ncout", "sheetTemplatePath", value);
            }
        }
        public string ncoutOutputFolderPath_;
        public string Ncout_OutputFolderPath
        {
            get => ncoutOutputFolderPath_;
            set
            {
                ncoutOutputFolderPath_ = value;
                OnPropertyChanged();
                ConfigINI.WriteSetting(ConfigInitPath, "Ncout", "OutputFolderPath", value);
            }
        }

        public ObservableCollection<PMEntity> NcOutToolpathCollection_ = new ObservableCollection<PMEntity>();

        public ObservableCollection<PMEntity> NcOutToolpathCollection { get => NcOutToolpathCollection_; set { NcOutToolpathCollection_ = value; OnPropertyChanged(); } }

        public NcoutOpt NcoutOptSelectOpt_=null;
        public NcoutOpt NcoutOptSelectOpt { get => NcoutOptSelectOpt_; set { 
                NcoutOptSelectOpt_ = value;
                if (value.OptCanSelectWkOutput)
                {
                    NcoutSetWorkplanesVisibility = Visibility.Visible;
                }
                else
                {
                    NcoutSetWorkplanesVisibility = Visibility.Hidden;
                }
                if (value.OptCanSelectMachineWk)
                {
                    NcoutShowWorkplanesVisibility = 45;
                    NcoutShowChangeworkplaneVisibility = 23;
                }
                else
                {
                    NcoutShowWorkplanesVisibility = 0;
                    NcoutShowChangeworkplaneVisibility = 0;
                }
                OnPropertyChanged();
                Onchange("NcoutSetWorkplanesVisibility");
                Onchange("NcoutShowWorkplanesVisibility");
                Onchange("NcoutShowChangeworkplaneVisibility");
            } }
        public Visibility NcoutSetWorkplanesVisibility { get; set; }
        public int NcoutShowWorkplanesVisibility { get; set; } = 45;
        public int NcoutShowChangeworkplaneVisibility { get; set; } = 23;
        public PMEntity Ncout_Selected_OutputWorkplane_ = new PMEntity() { Name=" "};
        public PMEntity Ncout_Selected_OutputWorkplane { get => Ncout_Selected_OutputWorkplane_; set { Ncout_Selected_OutputWorkplane_ = value; OnPropertyChanged(); } }

        public ObservableCollection<PMEntity> Ncout_ReadlyList_ = new ObservableCollection<PMEntity>();
        public ObservableCollection<PMEntity> Ncout_ReadlyList {  get => Ncout_ReadlyList_; set { Ncout_ReadlyList_ = value; OnPropertyChanged(); } }

        public bool? Ncout_AllTpOutput_ = true;
        public bool? Ncout_AllTpOutput
        {
            get => Ncout_AllTpOutput_;
            set
            {
                Ncout_AllTpOutput_ = value;
                OnPropertyChanged();
                ConfigINI.WriteSetting(ConfigInitPath, "Ncout", "AllTpOutput", value.ToString());
            }
        }
        public bool? Ncout_SingleTpOutput_ = true;
        public bool? Ncout_SingleTpOutput
        {
            get => Ncout_SingleTpOutput_;
            set
            {
                Ncout_SingleTpOutput_ = value;
                OnPropertyChanged();
                ConfigINI.WriteSetting(ConfigInitPath, "Ncout", "SingleTpOutput", value.ToString());
            }
        }

        #endregion




    }
}
