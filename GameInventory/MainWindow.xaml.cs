using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections;

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
        }

        private void FillDataGrid()
        {
            GameGrid.ItemsSource = Global.Search("SELECT * FROM Games").DefaultView;
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
    }
}
