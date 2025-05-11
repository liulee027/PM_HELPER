using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace General_lib
{
   public class LocalClient
    {
        public int HWND { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public IPEndPoint IPEndPoint { get; set; }
    }
}
