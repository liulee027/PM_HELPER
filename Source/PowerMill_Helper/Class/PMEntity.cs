using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PowerMill_Helper.Class
{
    public class PMEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void Onchange(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PMEntity DeepCopy()
        {
            PMEntity copy = (PMEntity)this.MemberwiseClone();
            copy.Children = new ObservableCollection<PMEntity>(this.Children.Select(child => child.DeepCopy()));
            return copy;
        }
        private string Name_;
        public string Name
        {
            get { return Name_; }
            set { Name_ = value; OnPropertyChanged(); }
        }

        private string Type_;
        public string Type
        { 
            get => Type_;
            set
            {
                Type_ = value;
                OnPropertyChanged();
            }
        }

        private bool Isactivate_ = false;
        public bool Isactivate
        {
            get { return Isactivate_; }
            set { Isactivate_ = value; Onchange("ActivateState"); }
        }
        private string ActivateState_;
        public string ActivateState
        {
            get
            {
                if (Isactivate_)
                {
                    return ">";
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ActivateState_ = value;
            }
        }
        private bool Isdraw = false;

        public bool IsDraw
        {
            get { return Isdraw; }
            set {
                Isdraw = value;
                Onchange("DrawIcon");
            }
        }
        public string DrawIcon
        {
            get
            {
                if (Isdraw)
                {
                    return "Image\\Entity_Led\\Led_ON.png";
                }
                else
                {
                    return "Image\\Entity_Led\\Led_OFF.png";
                }

            }
        }
        private string TypeIcon_;
        public string TypeIcon { get { return TypeIcon_; } set { TypeIcon_ = value; Onchange("TypeIcon"); } }
        public string RootPath { get; set; }
        private bool isRootTree_ = false;

        public bool isRootTree { get=> isRootTree_; set {
                isRootTree_ = value;
                if (value)
                {
                    BackColor=new SolidColorBrush(Colors.AliceBlue);
                    TypeIcon= "Image\\Entity_Folder\\Folder_Icon.png";
                }
                else
                {
                    BackColor = new SolidColorBrush(Colors.Transparent);
                }
                Onchange("BackColor");
            } } 
        private SolidColorBrush BackColor { get; set; }

        public ObservableCollection<PMEntity> Children { get; set; } = new ObservableCollection<PMEntity>();

        private int uiwidth_=0;
        public int uiwidth { get=> uiwidth_; set{ uiwidth_ = value;OnPropertyChanged(); } }

        private int NcoutToolNumber_ = 1;
        public int NcoutToolNumber { get => NcoutToolNumber_; set { NcoutToolNumber_ = value; OnPropertyChanged(); } }
        private string NcoutMachineWorkplane_ { get; set; }
        public string NcoutMachineWorkplane { get => NcoutMachineWorkplane_; set { NcoutMachineWorkplane_ = value; OnPropertyChanged(); }  }

    }
}
