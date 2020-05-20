using ModernWpf;
using ModernWpf.Controls;
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
        private readonly string Version = "Version 1.0";
        public string FileName { get; set; } = "Untitled.txt";
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
            StatusBarLeft.Content = $"No file saved.";
            this.SetTitle("Untitled");
        }
        private void SetTitle(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                this.Title = $"WritePad {Version}";
            }
            else
            {
                this.Title = $"WritePad {Version} - {text}";
            }
        }

        private void MenuItemOpenFile_Click(object sender, RoutedEventArgs e)
        {
            //This Code Opens a File
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Open File";
            dialog.CheckFileExists = true;
            dialog.Filter = "*.txt|*.txt|*.*|*.*";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            Editor.Text = System.IO.File.ReadAllText(dialog.FileName);
            this.FileName = dialog.FileName;
            this.SetTitle(Argus.IO.FileSystemUtilities.ExtractFileName(dialog.FileName));
            this.StatusBarLeft.Content = dialog.FileName;
        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            //This Code Saves a File
            var dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Title = "Save As";
            dialog.OverwritePrompt = true;
            dialog.Filter = ".txt|.txt|.|.";

            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            System.IO.File.WriteAllText(dialog.FileName, Editor.Text);
            this.FileName = dialog.FileName;
            this.SetTitle(Argus.IO.FileSystemUtilities.ExtractFileName(dialog.FileName));
            this.StatusBarLeft.Content = dialog.FileName;
        }

        private async void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Editor.Text))
            {
                var dlg = new ContentDialog
                {
                    Content = "Do you want to save your document?",
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Don't Save"
                };
                var result = await dlg.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    this.MenuSave_Click(sender, e);
                }
            }
            Environment.Exit(0);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            // If they haven't saved, they need a save dialog.  Otherwise, we'll save.
            if (string.IsNullOrWhiteSpace(this.FileName) || string.Equals(this.FileName, "untitled.txt", StringComparison.OrdinalIgnoreCase))
            {
                MenuSaveAs_Click(sender, e);
                return;
            }

            // Write the text to the previously saved file.
            System.IO.File.WriteAllText(this.FileName, Editor.Text);
        }

        private async void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Editor.Text))
            {
                var dlg = new ContentDialog
                {
                    Content = "Do you want to save your document?",
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Don't Save"
                };

                var result = await dlg.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    this.MenuSave_Click(sender, e);
                }
            }

            Editor.Text = "";
            this.FileName = "Untitled.txt";
            SetTitle(this.FileName);
            this.StatusBarLeft.Content = "No file saved.";
        }

        private void MenuCut_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Cut();
        }

        private void MenuCopy_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Copy();
        }

        private void MenuPaste_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Paste();
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Delete();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Editor.Text))
            {
                e.Cancel = true;

                var dlg = new ContentDialog
                {
                    Content = "Do you want to save your document?",
                    PrimaryButtonText = "Save",
                    SecondaryButtonText = "Don't Save"
                };

                var result = await dlg.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    this.MenuSave_Click(sender, null);
                }
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// Converts a hex color into a SolidColorBrush that WPF can use.
        /// </summary>
        /// <param name="hexCode"></param>
        public SolidColorBrush ToSolidColorBrush(string hexCode)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString(hexCode);
        }

        private void MenuLightTheme_Click(object sender, RoutedEventArgs e)
        {
            ModernWpf.ThemeManager.SetRequestedTheme(this, ElementTheme.Light);
            MenuMain.Background = Brushes.White;
            MenuMain.Foreground = Brushes.Black;
            TitleBar.SetBackground(this, Brushes.GhostWhite);
            Editor.Background = Brushes.LightGray;
            Editor.Foreground = Brushes.Black;

            MenuDarkTheme.IsChecked = false;
            MenuLightTheme.IsChecked = true;
        }

        private void MenuDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            ModernWpf.ThemeManager.SetRequestedTheme(this, ElementTheme.Dark);
            MenuMain.Background = ToSolidColorBrush("#171717");
            MenuMain.Foreground = Brushes.White;
            TitleBar.SetBackground(this, ToSolidColorBrush("#1B1B1B"));
            Editor.Background = ToSolidColorBrush("#1E1E1E");
            Editor.Foreground = ToSolidColorBrush("#E6E6E6");

            MenuDarkTheme.IsChecked = true;
            MenuLightTheme.IsChecked = false;
        }

    }
}
