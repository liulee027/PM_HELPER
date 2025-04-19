using PowerMill_Helper.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// NCout.xaml 的交互逻辑
    /// </summary>
    public partial class NCout : UserControl
    {
        public NCout()
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
        public event RoutedEventHandler Ncout_ResAll_Event;
        private void ThisAppRes(object sender, RoutedEventArgs e)
        {
            Ncout_ResAll_Event?.Invoke(this, e);
        }

        #endregion


        public event RoutedEventHandler Ncout_UserselectNcoutWorkplant;
        private void Ncout_UserselectNcoutWorkplantButton(object sender, RoutedEventArgs e)
        {
            Ncout_UserselectNcoutWorkplant?.Invoke(sender, e);
        }

        public delegate void OnSelectTpToRight(ObservableCollection<PMEntity> PMEntitys);
        public event OnSelectTpToRight OnSelectTpToRight_Event;
        private void ChooseTpToRight(object sender, RoutedEventArgs e)
        {
            ObservableCollection<PMEntity> PMEntityS_ = new ObservableCollection<PMEntity>();
            foreach (PMEntity item in LeftList.SelectedItems)
            {
                PMEntityS_.Add(item);
            }
            OnSelectTpToRight_Event?.Invoke(PMEntityS_);
        }

        public event RoutedEventHandler Ncout_ClearRight_Event;
        private void ClearRight(object sender, RoutedEventArgs e)
        {
            Ncout_ClearRight_Event?.Invoke(this, e);
        }

        private void ShowChangeTlNumberinput(object sender, RoutedEventArgs e)
        {
            if (RightTpListbox.SelectedItems.Count>0)
            {
                Button button = (Button)sender;
                button.Visibility = Visibility.Hidden;
                Grid grid = (Grid)button.Parent;
                TextBox textBox = (TextBox)grid.Children[0];
                textBox.Text = "";
                textBox.Focus();
            }
        }

        private void ChangeTlNumberinputEnter(object sender, KeyEventArgs e)
        {
         
            if (e.Key==Key.Enter)
            {
                ChangeTlNumberinputEnterVoid(sender, null);
            }
            if (e.Key==Key.Escape)
            {
                TextBox textBox = (TextBox)sender;
                Grid grid = (Grid)textBox.Parent;
                Button button = (Button)grid.Children[1];
                button.Visibility = Visibility.Visible;
            }

            
        }
        public delegate void ChangeTlNumber(int number, IList selectedItems);
        public event ChangeTlNumber ChangeTlNumber_Event;
        private void ChangeTlNumberinputEnterVoid(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (RightTpListbox.SelectedItems.Count > 0)
            {
             
                if (int.TryParse(textBox.Text, out int number))
                {
                    ChangeTlNumber_Event?.Invoke(number, RightTpListbox.SelectedItems);
                    Grid grid = (Grid)textBox.Parent;
                    Button button = (Button)grid.Children[1];
                    button.Visibility = Visibility.Visible;
                }
                else
                {
                    if (textBox.Text.Trim()=="")
                    {
                        Grid grid = (Grid)textBox.Parent;
                        Button button = (Button)grid.Children[1];
                        button.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("输入数据不为整数");
                    }
                  
                    
                }
            }
            else
            {
                Grid grid = (Grid)textBox.Parent;
                Button button = (Button)grid.Children[1];
                button.Visibility = Visibility.Visible;
            }
        }

        private void ShowChangeTpMachineWorkplaneCombobx(object sender, RoutedEventArgs e)
        {
            if (RightTpListbox.SelectedItems.Count > 0)
            {
                Button button = (Button)sender;
                button.Visibility = Visibility.Hidden;
                Grid grid = (Grid)button.Parent;
                ComboBox comboBox = (ComboBox)grid.Children[0];
                comboBox.IsDropDownOpen=true;

            }
        }
        public delegate void ChangeTpworkplane(string workplane, IList selectedItems);
        public event ChangeTpworkplane ChangeTpworkplane_Event;
        private void ShowChangeTpMachineWorkplaneSelected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (RightTpListbox.SelectedItems.Count > 0)
            {
                ChangeTpworkplane_Event?.Invoke(comboBox.SelectedItem.ToString(), RightTpListbox.SelectedItems);
            }
            Grid grid = (Grid)comboBox.Parent;
            Button button = (Button)grid.Children[1];
            button.Visibility = Visibility.Visible;

        }
        public event RoutedEventHandler CreateToNcToolpath_Event;
        private void CreateToNcToolpath(object sender, RoutedEventArgs e)
        {
            CreateToNcToolpath_Event?.Invoke(this,null);
        }

        public event RoutedEventHandler CreateSheet_Event;
        private void CreateSheet(object sender, RoutedEventArgs e)
        {
            CreateSheet_Event?.Invoke(this, null);
        }
        public event RoutedEventHandler OutPutNc_Click;
        private void OutPutNc(object sender, RoutedEventArgs e)
        {
            OutPutNc_Click?.Invoke(this, null);
        }
        public event RoutedEventHandler openNCFolder_Click;
        private void openNCFolder(object sender, RoutedEventArgs e)
        {
            openNCFolder_Click?.Invoke(this, null);
        }
    }
}
