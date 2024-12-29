using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private string Name_;
        public string Name
        {
            get { return Name_; }
            set { Name_ = value; OnPropertyChanged(); }
        }

        public string Type { get; set; }

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

        public bool isRootTree { get; set; } = false;

        public ObservableCollection<PMEntity> Children { get; set; } = new ObservableCollection<PMEntity>();


    }
}
