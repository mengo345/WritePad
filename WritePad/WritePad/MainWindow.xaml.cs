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

namespace WritePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string Version = "0.1 Prerelease";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateStatusBarCharacterLength()
        {
            StatusBarRight.Content = $"Characters: {Editor.Text.Length}";
        }

        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            this.UpdateStatusBarCharacterLength();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateStatusBarCharacterLength();
            StatusBarLeft.Content = $"Version: {this.Version}";
        }
    }
}
