using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tracker
{
    public class IslandTab
    {
        public TabItem IslandTabItem;
        public Island Island;
        public TabControl BuildingsControl;
        public TabItem Zoo;
        public TabItem Museum;
        public TabItem Botanical;

        public IslandTab(string name, Island island, List<string> zooItems, List<string> museumItems, List<string> botanicItems, MainWindow mainWindowInstance)
        {
            Island = island;

            BuildingsControl = new TabControl()
            {
                Background = Brushes.BurlyWood
            };

            Zoo = CreateTab("Zoo");
            Zoo.Content = new CollectionTemplate("zoo", island.Zoo, zooItems, mainWindowInstance);

            Museum = CreateTab("Museum");
            Museum.Content = new CollectionTemplate("museum", island.Museum, museumItems, mainWindowInstance);

            Botanical = CreateTab("Botanischer Garten");
            Botanical.Content = new CollectionTemplate("botanic", island.Botanic, botanicItems, mainWindowInstance);

            BuildingsControl.Items.Add(Zoo);
            BuildingsControl.Items.Add(Museum);
            BuildingsControl.Items.Add(Botanical);

            IslandTabItem = new TabItem()
            {
                Header = name,
                Style = Application.Current.Resources["CollectionTab"] as Style,
                Content = BuildingsControl
            };
            IslandTabItem.GotFocus += Selected;

        }
        private TabItem CreateTab(string headerName)
        {
            StackPanel newHeader = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            newHeader.Children.Add(new Image() { Source = new BitmapImage(new Uri("Resources/" + headerName.Replace(" ", "") + ".png", UriKind.Relative)), Width = 20 });
            newHeader.Children.Add(new TextBlock() { Text = headerName, Foreground=Brushes.SaddleBrown, Padding = new Thickness(3, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center });
            TabItem newTab = new TabItem()
            {
                Style = Application.Current.Resources["CollectionTab"] as Style,
                Header = newHeader,
            };
            return newTab;
        }

        private void Selected(object sender, RoutedEventArgs e)
        {
            if(sender as TabItem == null) return;
            MainWindow.SelectedTab = this;
        }
    }
}
