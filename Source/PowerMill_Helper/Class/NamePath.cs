using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerMill_Helper.Class
{
     public class NamePath 
    {


        public string PathStr { get; set; } 
        public string FullPath { get; set; } 
        public ObservableCollection<NamePath> Children { get; set; } = new ObservableCollection<NamePath>();
        public bool isFolder { get; set; } = true;
        public string IconPath { get; set; }
        public bool OptCanSelectWkOutput { get; set; } = true;
        public bool OptCanSelectMachineWk { get; set; } = false;
        public string OptSelectMachineWkName { get; set; } = "G54,G55,G56,G57,G58";

    }
}

