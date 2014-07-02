using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace GameInventory
{
    /// <summary>
    /// Main inventory window for searches and choosing options.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
            FillAdvancedDropdowns();
        }

        private void FillDataGrid()
        {
            GameGrid.ItemsSource = Global.Search("SELECT * FROM Games").DefaultView;
        }

        private void FillAdvancedDropdowns()
        {
            List<string> platforms = new List<string>();
            platforms.Add("Any platform");
            platforms.AddRange(Global.Platforms);
            Platform.ItemsSource = platforms;

            List<string> languages = new List<string>();
            languages.Add("Any language");
            languages.AddRange(Global.Languages);
            Language.ItemsSource = languages;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddGame addwindow = new AddGame();
            addwindow.RefreshEvent += new EventHandler(addwindow_RefreshEvent);
            addwindow.ShowDialog();
        }

        private void addwindow_RefreshEvent(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        private void LoadSelectedGameIntoGlobal(int id)
        {
            GameRepository gr = new GameRepository();
            Global.SelectedGame = gr.GetById(id);
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            if (GameGrid.SelectedItem != null)
            {
                LoadSelectedGameIntoGlobal(Global.SelectedGame.GameID);
                EditGame editwindow = new EditGame();
                editwindow.RefreshEvent += new EventHandler(editwindow_RefreshEvent);
                editwindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select a game first!");
            }

        }

        private void editwindow_RefreshEvent(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        private Boolean Confirmation()
        {
            bool response = false;
            string text = "Are you sure?";
            string caption = "Game Inventory";

            MessageBoxResult result = MessageBox.Show(text, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes: response = true;
                    break;
                case MessageBoxResult.No: response = false;
                    break;
                case MessageBoxResult.Cancel: response = false;
                    break;
            }
            return response;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (GameGrid.SelectedItem != null)
            {
                if (Confirmation() == true)
                {
                    Global.DeleteGame(Global.SelectedGame.GameID);
                    FillDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Select a game first!");
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            string query = Global.QueryBuilder(Searchbox.Text);
            
            if (AdvancedSearch.IsChecked == true)
            {
                query = ParseAdvancedSearchOptions(query);
            }

            DataTable results = Global.Search(query);
            GameGrid.ItemsSource = results.DefaultView;
        }

        private void GetSelectionID(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GameGrid.SelectedItem != null)
            {
                IList rows = GameGrid.SelectedItems;
                DataRowView row = (DataRowView)GameGrid.SelectedItems[0];
                Global.SelectedGame.GameID = Convert.ToInt32(row["GameID"]);
            }
        }

        private string ParseAdvancedSearchOptions(string query)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(query);
            if (Platform.SelectedIndex > 0)
            {
                string platformstatus = Global.AdvancedQueryBuilder("Platform", Platform.SelectedValue.ToString());
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, platformstatus + " AND ");
            }

            if (Language.SelectedIndex > 0)
            {
                string languagestatus = Global.AdvancedQueryBuilder("Language", Language.SelectedValue.ToString());
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, languagestatus + " AND ");
            }

            if (Owned.SelectedIndex > 0)
            {
                string ownstatus = Global.AdvancedBooleanQueryBuilder("Owned", Owned.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, ownstatus + " AND ");
            }

            if (Cartridge.SelectedIndex > 0)
            {
                string cartridgestatus = Global.AdvancedBooleanQueryBuilder("Cartridge", Cartridge.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, cartridgestatus + " AND ");
            }

            if (Box.SelectedIndex > 0)
            {
                string boxstatus = Global.AdvancedBooleanQueryBuilder("Box", Box.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, boxstatus + " AND ");
            }

            if (Manual.SelectedIndex > 0)
            {
                string manualstatus = Global.AdvancedBooleanQueryBuilder("Manual", Manual.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, manualstatus + " AND ");
            }

            if (Guide.SelectedIndex > 0)
            {
                string guidestatus = Global.AdvancedBooleanQueryBuilder("Guide", Guide.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, guidestatus + " AND ");
            }

            if (NewInBox.SelectedIndex > 0)
            {
                string newinboxstatus = Global.AdvancedBooleanQueryBuilder("NewInBox", Box.SelectedIndex);
                sb.Insert(sb.ToString().IndexOf("WHERE ") + 6, newinboxstatus + " AND ");
            }




            return sb.ToString();
        }
        
    }
}
