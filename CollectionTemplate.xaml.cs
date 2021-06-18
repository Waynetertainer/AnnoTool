using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tracker
{
    /// <summary>
    /// Interaktionslogik für CollectionTemplate.xaml
    /// </summary>
    public partial class CollectionTemplate : UserControl
    {
        private bool[] itemArray;
        private MainWindow mainWindowInstance;

        public CollectionTemplate(string type, bool[] itemArray, List<string> items, MainWindow main)
        {
            InitializeComponent();
            this.itemArray = itemArray;
            mainWindowInstance = main;

            Bg1.Source = BitmapFrame.Create(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Tracker.Resources." + type + "." + type + "1.png"));

            if (type == "botanic")
            {
                mainGrid.Children.Remove(mainGrid.FindName("grid2") as UIElement);
                mainGrid.Children.Remove(mainGrid.FindName("Bg2") as UIElement);
                mainGrid.RowDefinitions.RemoveAt(1);
            }
            else
            {
                Bg2.Source = BitmapFrame.Create(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Tracker.Resources." + type + "." + type + "2.png"));
            }


            int counter = 0;
            for (int grid = 1; grid <= 2; grid++)
            {
                for (int i = 1; i < 6; i += 2)
                {
                    for (int j = 1; j < 30; j += 2)
                    {
                        if (counter >= items.Count) return;
                        if ((i == 1 || (type == "museum" && grid == 2)) && (j > 11 && j < 19)) continue;
                        string identifier = items[counter].Replace(".png", "").Split('.').Last();
                        Stream sm = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(items[counter]);
                        Image newImg = new Image();
                        if (sm != null)
                        {
                            newImg.Source = BitmapFrame.Create(sm);
                        }
                        ToolTip toolTip = new ToolTip()
                        {
                            Background = Brushes.BurlyWood,
                            BorderBrush = Brushes.BlanchedAlmond,
                            Content = identifier.Substring(2)
                        };
                        ToggleButton newBtn = new ToggleButton()
                        {
                            Style = Application.Current.Resources["ItemToggle"] as Style,
                            Content = newImg,
                            ToolTip = toolTip,
                            Name = "i" + identifier.Substring(0, 2),
                            IsChecked = itemArray[Int32.Parse(identifier.Substring(0, 2)) - 1]
                        };
                        newBtn.Opacity = itemArray[Int32.Parse(identifier.Substring(0, 2)) - 1] ? 1 : 0;
                        newBtn.SetValue(Grid.RowProperty, i);
                        newBtn.SetValue(Grid.ColumnProperty, j);
                        newBtn.Click += ButtonClicked;

                        if (grid == 1)
                        {
                            grid1.Children.Add(newBtn);
                        }
                        else
                        {
                            grid2.Children.Add(newBtn);
                        }
                        counter++;
                    }
                }
            }
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (sender as ToggleButton);
            btn.Opacity = btn.IsChecked == true ? 1 : 0;
            itemArray[Int32.Parse(btn.Name.Substring(1, 2)) - 1] = btn.IsChecked == true;
            mainWindowInstance.ChangesMade = true;
            mainWindowInstance.btnSave.IsEnabled = true;
        }
    }
}
