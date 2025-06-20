﻿using PowerMill_Helper.Class;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cursors = System.Windows.Input.Cursors;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using TreeView = System.Windows.Controls.TreeView;
using UserControl = System.Windows.Controls.UserControl;

namespace PowerMill_Helper.Tool
{
    /// <summary>
    /// MacroLib.xaml 的交互逻辑
    /// </summary>
    public partial class MacroLib : UserControl
    {
        public MacroLib(MainCS mainCS_, PowerMILL.PluginServices PmServices_)
        {
            InitializeComponent();
            PmServices = PmServices_;
            MCS = mainCS_;
            this.DataContext = MCS;

            string MacorLibFolderslistStr = ConfigINI.ReadSetting(MCS.ConfigInitPath, "MacorLib", "USERMACROFOLDERS", "");
            if (MacorLibFolderslistStr != "")
            {
                foreach (string item in MacorLibFolderslistStr.Split(',')) MCS.MacorLibFolders.Add(item);
                MCS.DrawMacorLibTreeView();
            }
            MCS.AutoClodeMacroLibPage = ConfigINI.ReadSetting(MCS.ConfigInitPath, "MacorLib", "AutoClodeMacroLibPage", "1") == "1" ? true : false;

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

        #region 选择文件夹节点 单击展开文件夹
        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
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
        #endregion

        #region 选择实例节点
        public delegate void OnTreeview_SelectSomthing(NamePath namePath);
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
        #endregion


    }
}
