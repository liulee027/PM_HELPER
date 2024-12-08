using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace PowerMill_Helper.Class
{
   internal class MainCS : INotifyPropertyChanged
    {
        public MainCS() {
            _ = StartOpacityAnimation();
            SaveStateColor = new SolidColorBrush(Colors.GreenYellow);
            SaveStateEffect = Colors.GreenYellow;
            ProjectName = "NULL";
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
