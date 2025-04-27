using PowerMill_Helper.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// CheckTP.xaml 的交互逻辑
    /// </summary>
    public partial class CheckTP : UserControl
    {
        public CheckTP(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
            this.DataContext = MCS;
        }

        private MainCS MCS;

        #region PmCommand
        public string token;
        public PowerMILL.PluginServices PmServices;
        private void PMCom(string comd)
        {
            PmServices.InsertCommand(token, comd);
        }

        private string PMComEX(string comd)
        {
            object item;
            PmServices.InsertCommand(token, "ECHO OFF DCPDEBUG UNTRACE COMMAND ACCEPT");
            PmServices.DoCommandEx(token, comd, out item);
            return item.ToString().TrimEnd();
        }
        private string GetPMVal(string comd)
        {
            string Result = PmServices.GetParameterValueTerse(token, comd);
            if (Result.IndexOf("#错误:") == 0) return "";
            return PmServices.GetParameterValueTerse(token, comd);
        }
        #endregion

        #region ControlTitleEvent
        Point Grid_Move_Pos = new Point();
        private void Move_Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                Grid_Move_Pos = e.GetPosition(null);
                label.CaptureMouse();
                label.Cursor = Cursors.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MouseDown\r" + ex.ToString());
            }


        }
        private void Move_Border_Mousemove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    double dx = e.GetPosition(null).X - Grid_Move_Pos.X + this.Margin.Left;
                    double dy = e.GetPosition(null).Y - Grid_Move_Pos.Y + this.Margin.Top;
                    this.Margin = new Thickness(dx, dy, 0, 0);
                    Grid_Move_Pos = e.GetPosition(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_Mousemove\r" + ex.ToString());
            }

        }
        private void Move_Border_MoseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label label = (Label)sender;
                label.ReleaseMouseCapture();
                label.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Move_Border_MoseUp\r" + ex.ToString());
            }
        }

        private void BorderCloserButton(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BorderCloserButton\r" + ex.ToString());
            }

        }


        #endregion

        private void ThisAppRes(object sender, RoutedEventArgs e)
        {

        }


        private void PutRadiobuttonToTop(RadioButton radioButton)
        {

        }

    }
}
