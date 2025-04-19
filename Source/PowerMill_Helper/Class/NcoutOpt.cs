using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMill_Helper.Class
{
    public class NcoutOpt
    {
      
    
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("FullPath")]
        public string FullPath { get; set; }
        [JsonProperty("OptCanSelectWkOutput")]
        public bool OptCanSelectWkOutput { get; set; }
        [JsonProperty("OptCanSelectMachineWk")]
        public bool OptCanSelectMachineWk { get; set; }
        [JsonProperty("OptSelectMachineWkName")]
        public string OptSelectMachineWkName { get => OptSelectMachineWkName_; set { OptSelectMachineWkName_ = value.Replace("，",",").Trim(); } }
       

        private string OptSelectMachineWkName_ = "G54,G55,G56,G57,G58" ;

        public string[] OptSelectMachineWkNameLIst { get=> OptSelectMachineWkName_.Split(','); }  
    }

}
