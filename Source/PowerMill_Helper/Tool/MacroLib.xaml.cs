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
    /// MacroLib.xaml 的交互逻辑
    /// </summary>
    public partial class MacroLib : UserControl
    {
        public MacroLib()
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
    

      
        //选择节点事件，单击展开文件夹
        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeView  treeView = (TreeView)sender;
            TreeViewItem treeViewItem = e.OriginalSource as TreeViewItem;
            if (treeViewItem == null || e.Handled) return;
            if (treeView.SelectedItem != null)
            {
                NamePath namepath_ = (NamePath)treeView.SelectedItem;
                if (namepath_.isFolder)
                {
                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                    treeViewItem.IsSelected = false;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
         public   delegate void OnTreeview_SelectSomthing(NamePath namePath);
        public event OnTreeview_SelectSomthing OnTreeview_SelectSomthingEnent;
        private void Treeview_SelectSomthing(object sender, MouseButtonEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            if (treeView.SelectedItem != null)
            {
                NamePath namepath_ = (NamePath)treeView.SelectedItem;
                if (namepath_.isFolder)
                {
                    e.Handled = true;
                }
                else
                {
                    OnTreeview_SelectSomthingEnent?.Invoke(namepath_);
                    e.Handled = true;
                }
            }
        }
    }
}
