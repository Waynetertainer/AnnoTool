using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Path = System.IO.Path;

namespace Tracker
{
    /// <summary>
    /// Interaktionslogik für LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {

        private List<string> files;
        private Button selectedButton;
        public LoadWindow()
        {
            InitializeComponent();
            files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml").ToList();
            for (var i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Split(Path.DirectorySeparatorChar).Last();
                Button newButton = new Button()
                {
                    Style = Application.Current.Resources["LoadButton"] as Style,
                    FontFamily = new FontFamily("Perpetua"),
                    FontWeight = FontWeights.UltraBold,
                    Content = files[i]
                };
                newButton.Click += SelectChange;
                FilePanel.Children.Add(newButton);
            }

            LoadName.Text = GetNewGame();
        }


        private void SelectChange(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            LoadName.Text = btn.Content.ToString();
            if (selectedButton != null)
            {
                selectedButton.IsEnabled = true;
            }
            selectedButton = btn;
            btn.IsEnabled = false;
        }

        private void ClosingEv(object sender, CancelEventArgs e)
        {
            if (LoadName.Text == "")
            {
                LoadName.Text = GetNewGame() + ".xml";
            }

            if (!LoadName.Text.EndsWith(".xml"))
            {
                LoadName.Text += ".xml";
            }
            MainWindow.SaveName = LoadName.Text;
        }

        private void NewSave(object sender, RoutedEventArgs e)
        {

            if (File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + LoadName.Text) ||
                File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + LoadName.Text + ".xml"))
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Die Datei wirklich überschreiben?",
                        "Warning",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                File.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + LoadName.Text);
                Close();

            }
            else
            {
                Close();
            }
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + LoadName.Text))
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Die Datei wirklich löschen?",
                        "Warning",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + LoadName.Text);
                    files.Remove(LoadName.Text);
                    FilePanel.Children.Remove(selectedButton);
                    selectedButton = null;
                }
            }
        }

        private void TextChangedByUser(object sender, RoutedEventArgs e)
        {
            DeleteBtn.IsEnabled = false;
            OpenBtn.IsEnabled = false;
            if (selectedButton != null) selectedButton.IsEnabled = true;
            selectedButton = null;
            foreach (UIElement button in FilePanel.Children)
            {
                Button btn = button as Button;
                if (btn.Content.ToString() == LoadName.Text)
                {
                    selectedButton = btn;
                    btn.IsEnabled = false;

                    DeleteBtn.IsEnabled = true;
                    OpenBtn.IsEnabled = true;
                    break;
                }
            }
        }

        private string GetNewGame()
        {
            if (files.All(f => f != "Neues Spiel.xml"))
            {
                return "Neues Spiel.xml";
            }
            else
            {
                int i = 1;
                while (files.Any(f => f == "Neues Spiel" + i + ".xml"))
                {
                    i++;
                }

                return "Neues Spiel" + i + ".xml";
            }
        }
    }
}
