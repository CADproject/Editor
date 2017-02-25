﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using Application = System.Windows.Application;

namespace CADView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Title += " " + version;

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
            Application.Current.MainWindow = this;
        }
    }
}
