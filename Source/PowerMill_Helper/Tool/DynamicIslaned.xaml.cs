﻿using System;
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
    /// DynamicIslaned.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicIslaned : UserControl
    {
        public DynamicIslaned()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler UserSelectProgEvent;
        private void Image_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UserSelectProgEvent?.Invoke(this, e);
        }
        public event RoutedEventHandler UserSaveProgEvent;
        private void UserSaveProg(object sender, MouseButtonEventArgs e)
        {
            UserSaveProgEvent?.Invoke(this, e);
        }
        public event RoutedEventHandler UserSaveOtherWhereEvent;
        private void UserSaveOtherWhere(object sender, MouseButtonEventArgs e)
        {
            UserSaveOtherWhereEvent?.Invoke(this, e);
        }
    }
}