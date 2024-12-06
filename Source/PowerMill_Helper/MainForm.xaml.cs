using Delcam.Plugins.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace PowerMill_Helper
{
    /// <summary>
    /// MainFrom.xaml 的交互逻辑
    /// </summary>
    public partial class MainForm : Window
    {
        public MainForm(IPluginCommunicationsInterface conn, string token, PowerMILL.PluginServices servicess, IntPtr Pminptr)
        {
            InitializeComponent();

            Screen[] screens = Screen.AllScreens;
            MainPage.Width = screens[0].WorkingArea.Width;
            MainPage.Height = screens[0].WorkingArea.Height;
        }
    }
}
