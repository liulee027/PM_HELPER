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
    /// SettingForm.xaml 的交互逻辑
    /// </summary>
    public partial class SettingForm : UserControl
    {
        public SettingForm()
        {
            InitializeComponent();
        }

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



        public event RoutedEventHandler Setting_MacroLib_Addfolder;
        private void MacroLib_addFolder(object sender, RoutedEventArgs e)
        {
            Setting_MacroLib_Addfolder?.Invoke(this, e);
        }
        public event RoutedEventHandler Setting_MacroLib_Removerfolder;
        private void MacroLib_Remover(object sender, RoutedEventArgs e)
        {
            Setting_MacroLib_Removerfolder?.Invoke(this, e);
        }
        public event RoutedEventHandler Setting_MacroLib_SelectUpfolder;
        private void MacroLib_SelectUp(object sender, RoutedEventArgs e)
        {
            Setting_MacroLib_SelectUpfolder?.Invoke(this, e);
        }
        public event RoutedEventHandler Setting_MacroLib_Selectdownfolder;
        private void MacroLib_Selectdown(object sender, RoutedEventArgs e)
        {
            Setting_MacroLib_Selectdownfolder?.Invoke(this, e);
        }
        public event RoutedEventHandler Setting_MacroLib_ResReadfolder;
        private void MacroLib_RES(object sender, RoutedEventArgs e)
        {
            Setting_MacroLib_ResReadfolder?.Invoke(this, e);
        }
    }
}
