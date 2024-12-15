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

    }
}
