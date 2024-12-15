using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PowerMill_Helper.Theme;


namespace PowerMill_Helper.Class
{
   internal class MainCS : INotifyPropertyChanged
    {
        public MainCS() {
            _ = StartOpacityAnimation();
            SaveStateColor = new SolidColorBrush(Colors.GreenYellow);
            SaveStateEffect = Colors.GreenYellow;
            ProjectName = "NULL";
            PluginPath = this.GetType().Assembly.Location;
            FileInfo fileInfo_ = new FileInfo(PluginPath);
            PluginFolder = fileInfo_.Directory.FullName;
            ConfigInitPath=System.IO.Path.Combine(PluginFolder, "config.ini");

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


        #endregion
        #region 设置窗口
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
            MacorLibTreeViewList= new ObservableCollection<NamePath>();
            foreach (string item in MacorrLibFolders_)
            {
                DirectoryInfo    directoryInfo = new DirectoryInfo(item);
                NamePath namePath = new NamePath();
                MacorLibTreeViewList.Add(namePath);
                ReadMacorLibFolders(namePath, directoryInfo.FullName);
            }
            Onchange("MacorLibTreeViewList");
        }
        public ObservableCollection<NamePath> MacorLibTreeViewList{get;  set;  }=new ObservableCollection<NamePath>();
        private void ReadMacorLibFolders(NamePath ParentNode,string Path)
        {
            //NamePath namePath = new NamePath();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path);
            ParentNode.PathStr = directoryInfo.Name;
            ParentNode.IconPath =@"Image\OpenFile.png";
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
                if (item.Extension!="")
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




    }
}
