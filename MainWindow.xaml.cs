using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Tracker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Island> islands = new List<Island>();
        private List<string> zooItems = new List<string>();
        private List<string> museumItems = new List<string>();
        private List<string> botanicItems = new List<string>();

        public bool ChangesMade;

        public static IslandTab SelectedTab;
        public static string SaveName = "";

        public MainWindow()
        {
            InitializeComponent();
            string[] resources = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            zooItems = resources.Where(i => i.Contains("zoo.Items")).OrderBy(i => i).ToList();
            museumItems = resources.Where(i => i.Contains("museum.Items")).OrderBy(i => i).ToList();
            botanicItems = resources.Where(i => i.Contains("botanic.Items")).OrderBy(i => i).ToList();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            IslandControl.Items.Clear();

            SelectedTab = null;
            LoadWindow w = new LoadWindow();
            w.ShowDialog();
            btnRemove.IsEnabled = false;
            if (File.Exists(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + SaveName))
            {
                TextReader reader = null;
                try
                {
                    var serializer = new XmlSerializer(typeof(List<Island>));
                    reader = new StreamReader(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + SaveName);
                    islands = (List<Island>)serializer.Deserialize(reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
                foreach (Island island in islands)
                {
                    IslandTab newIslandTab = new IslandTab(island.Name, island, zooItems, museumItems, botanicItems, this);
                    IslandControl.Items.Add(newIslandTab.IslandTabItem);
                }
                if (IslandControl.Items.Count > 0)
                {
                    (IslandControl.Items[0] as TabItem).Focus();
                    btnRemove.IsEnabled = true;
                }

            }
            Title = SaveName;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(List<Island>));
                writer = new StreamWriter(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + SaveName);
                serializer.Serialize(writer, islands);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            ChangesMade = false;
            btnSave.IsEnabled = false;
        }

        private void ClearInput(object sender, RoutedEventArgs e)
        {
            islandInput.Clear();
            islandInput.GotFocus -= ClearInput;
        }

        private void InputLostFocus(object sender, RoutedEventArgs e)
        {
            if (islandInput.Text == "")
            {
                islandInput.Text = "Neue Insel";
                islandInput.GotFocus += ClearInput;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Island newIsland = new Island()
            {
                Name = islandInput.Text
            };
            IslandTab newIslandTab = new IslandTab(islandInput.Text, newIsland, zooItems, museumItems, botanicItems, this);
            islands.Add(newIsland);
            IslandControl.Items.Add(newIslandTab.IslandTabItem);
            islandInput.Text = "Neue Insel";
            islandInput.GotFocus += ClearInput;
            newIslandTab.IslandTabItem.Focus();
            ChangesMade = true;
            btnSave.IsEnabled = true;
            btnRemove.IsEnabled = true;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTab == null) return;

            islands.Remove(SelectedTab.Island);
            IslandControl.Items.Remove(SelectedTab.IslandTabItem);
            SelectedTab = null;
            if (IslandControl.Items.Count > 0)
            {
                (IslandControl.Items[0] as TabItem).Focus();
                btnRemove.IsEnabled = true;
            }
            else
            {
                btnRemove.IsEnabled = false;
            }

            ChangesMade = true;
            btnSave.IsEnabled = true;
        }

        private void ClosingAsk(object sender, CancelEventArgs e)
        {
            if (ChangesMade)
            {
                MessageBoxResult result =
                    MessageBox.Show(
                        "Es gibt ungespeicherte Änderungen.\nTrotzdem schließen?",
                        "Warning",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
