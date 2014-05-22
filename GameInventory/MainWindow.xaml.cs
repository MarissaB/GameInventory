using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;

namespace GameInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            GameGrid.ItemsSource = Global.Search("SELECT * FROM Games").DefaultView;
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            // Pop up with window template to edit selected game in grid
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            // Pop up with messagebox confirmation to delete selected game in grid
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            string query = Global.QueryBuilder(Searchbox.Text);
            DataTable results = Global.Search(query);
            GameGrid.ItemsSource = results.DefaultView;
        }
    }
}
