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
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Xml;
using WeAreDevs_API;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Net;

namespace InfurnusX
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        ExploitAPI api = new ExploitAPI();
        public Window1()
        {
            InitializeComponent();
            Topmost = true;
            Textbox.TextArea.TextView.LinkTextBackgroundBrush = Brushes.Transparent;
            Textbox.TextArea.TextView.LinkTextForegroundBrush = Brushes.Orange;
            Textbox.TextArea.TextView.LinkTextUnderline = false;
            Stream stream = File.OpenRead("./lua.xshd");
            XmlTextReader reader = new XmlTextReader(stream);
            Textbox.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }


        Storyboard storyboard = new Storyboard();
        TimeSpan halfsecond = TimeSpan.FromMilliseconds(500);
        TimeSpan second = TimeSpan.FromSeconds(1);

        private IEasingFunction Smooth
        {
            get;
            set;
        }
        = new QuarticEase
        {
            EasingMode = EasingMode.EaseInOut
        };

        public void Fade(DependencyObject Object)
        {
            DoubleAnimation FadeIn = new DoubleAnimation()
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(halfsecond),
            };
            Storyboard.SetTarget(FadeIn, Object);
            Storyboard.SetTargetProperty(FadeIn, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(FadeIn);
            storyboard.Begin();
        }

        public void FadeOut(DependencyObject Object)
        {
            DoubleAnimation FadeOut = new DoubleAnimation()
            {
                From = 1.0,
                To = 0.0,
                Duration = new Duration(halfsecond),
            };
            Storyboard.SetTarget(FadeOut, Object);
            Storyboard.SetTargetProperty(FadeOut, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(FadeOut);
            storyboard.Begin();
        }

        public void ObjectShiftPos(DependencyObject Object, Thickness Get, Thickness Set)
        {
            ThicknessAnimation ShiftAnimation = new ThicknessAnimation()
            {
                From = Get,
                To = Set,
                Duration = second,
                EasingFunction = Smooth,
            };
            Storyboard.SetTarget(ShiftAnimation, Object);
            Storyboard.SetTargetProperty(ShiftAnimation, new PropertyPath(MarginProperty));
            storyboard.Children.Add(ShiftAnimation);
            storyboard.Begin();
        }

        public void Resize()
        {
            DoubleAnimation danimationX = new DoubleAnimation();
            danimationX.From = MainBorder.Width;
            danimationX.To = 650;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            MainBorder.BeginAnimation(WidthProperty, danimationX);

            DoubleAnimation danimationY = new DoubleAnimation();
            danimationY.From = MainBorder.Height;
            danimationY.To = 344;
            danimationY.Duration = second;
            danimationY.EasingFunction = Smooth;
            MainBorder.BeginAnimation(HeightProperty, danimationY);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Back.Visibility = Visibility.Hidden;
            Textbox.Visibility = Visibility.Visible;
            SettingsTab.Visibility = Visibility.Hidden;
            UnlockFPS.Visibility = Visibility.Hidden;
            InternalUI.Visibility = Visibility.Hidden;
            Script_Hub.Visibility = Visibility.Hidden;
            DarkHub.Visibility = Visibility.Hidden;
        }

        

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            api.SendLuaScript(Textbox.Text);
        }

        private void Inject_Click(object sender, RoutedEventArgs e)
        {
            api.LaunchExploit();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Textbox.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = saveFileDialog.ShowDialog();
            saveFileDialog.Filter = "Lua Scripts (*.lua)|*.lua|Txt Scripts (*.txt)|*.txt";
            saveFileDialog.Title = "Save Scripts";
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                File.WriteAllText(saveFileDialog.FileName, Textbox.Text);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDialog.ShowDialog();
            openFileDialog.Filter = "Lua Scripts (*.lua)|*.lua|Txt Scripts (*.txt)|*.txt";
            openFileDialog.Title = "Save Scripts";
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                Textbox.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Back.Visibility = Visibility.Visible;
            Textbox.Visibility = Visibility.Hidden;
            SettingsTab.Visibility = Visibility.Visible;
            UnlockFPS.Visibility = Visibility.Visible;
            InternalUI.Visibility = Visibility.Visible;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void UnlockFPS_Click(object sender, RoutedEventArgs e)
        {
            api.SendLuaScript("setfpscap(999)");
        }

        private void InternalUI_Click(object sender, RoutedEventArgs e)
        {
            WebClient wb = new WebClient();
            string Script = wb.DownloadString("https://pastebin.com/raw/9ZsahqGX");
            api.SendLuaScript(Script);
        }

        private async void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Fade(this.MainBorder);
            ObjectShiftPos(MainBorder, MainBorder.Margin, new Thickness(0));
            await Task.Delay(1000);
            Resize();
            await Task.Delay(750);
            Exit.Visibility = Visibility.Visible;
            Minimize.Visibility = Visibility.Visible;
            Clear.Visibility = Visibility.Visible;
            Inject.Visibility = Visibility.Visible;
            Execute.Visibility = Visibility.Visible;
            Open.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            Textbox.Visibility = Visibility.Visible;
            Label1.Visibility = Visibility.Visible;
            Settings.Visibility = Visibility.Visible;
            ScriptHub.Visibility = Visibility.Visible;
            Icon.Visibility = Visibility.Hidden;
        }

        private void DarkHub_Click(object sender, RoutedEventArgs e)
        {
            api.SendLuaScript("https://pastebin.com/raw/D2NdvmSC");
        }

        private void ScriptHub_Click(object sender, RoutedEventArgs e)
        {
            Back.Visibility = Visibility.Visible;
            Textbox.Visibility = Visibility.Hidden;
            DarkHub.Visibility = Visibility.Visible;
            Script_Hub.Visibility = Visibility.Visible;
        }
    }
}
