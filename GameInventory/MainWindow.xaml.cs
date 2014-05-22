using System.Windows;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
            string connectionstring = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
            string commandstring = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                commandstring = "SELECT * FROM Games";
                SqlCommand cmd = new SqlCommand(commandstring, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Game");
                sda.Fill(dt);
                GameGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            // Pop up with window template to add new game
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
            string connectionstring = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
            string commandstring = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                commandstring = Global.QueryBuilder(Searchbox.Text);
                SqlCommand cmd = new SqlCommand(commandstring, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Game");
                sda.Fill(dt);
                GameGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}
