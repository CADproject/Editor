using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using CADController;
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

            this.RenderPanel.MouseMove+=RenderPanelMouseEvent;
        }

        private void RenderPanelMouseEvent(object sender, MouseEventArgs mouseEventArgs)
        {
            ((MainWindowViewModel) DataContext).Controller.eventHendling(ApplicationController.mouseEvents.move,
                mouseEventArgs.X, mouseEventArgs.Y);
        }
    }
}
