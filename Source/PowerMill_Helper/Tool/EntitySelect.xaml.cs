using PowerMill_Helper.Class;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// EntitySelect.xaml 的交互逻辑
    /// </summary>
    public partial class EntitySelect : UserControl
    {
        public EntitySelect()
        {
            InitializeComponent();
        }
        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            TreeViewItem treeViewItem = e.OriginalSource as TreeViewItem;
            if (treeViewItem == null || e.Handled) return;
            if (treeView.SelectedItem != null)
            {
                PMEntity PMEntity_ = (PMEntity)treeView.SelectedItem;
                if (PMEntity_.isRootTree)
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

        public delegate void OnEntitySelect_SelectSomthing(PMEntity PMEntity_);
        public event OnEntitySelect_SelectSomthing OnEntitySelect_SelectSomthingEnent;

        private void EntitySelectSomething(object sender, MouseButtonEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            if (treeView.SelectedItem != null)
            {
                PMEntity PMEntity_ = (PMEntity)treeView.SelectedItem;
                if (PMEntity_.isRootTree)
                {
                    e.Handled = true;
                }
                else
                {
                    OnEntitySelect_SelectSomthingEnent?.Invoke(PMEntity_);
                    e.Handled = true;
                }
            }
        }


    }
}
