﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SmartBILL.ViewModels;

namespace SmartBILL.Views
{
    /// <summary>
    /// Interaction logic for PartyChallanView.xaml
    /// </summary>
    public partial class PartyChallanView : UserControl
    {
        public PartyChallanView()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
                DataContext = new PartyChallanViewModel();
        }
    }
}
