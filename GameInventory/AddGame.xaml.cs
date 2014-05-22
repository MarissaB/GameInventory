using System;
using System.Windows;
using System.Windows.Controls;

namespace GameInventory
{
    /// <summary>
    /// Window to input new game data.
    /// </summary>
    public partial class AddGame : Window
    {
        public event EventHandler RefreshEvent;

        protected void OnRefresh()
        {
            if (this.RefreshEvent != null)
                this.RefreshEvent(this, EventArgs.Empty);
        }

        public AddGame()
        {
            InitializeComponent();
            Platforms.ItemsSource = Global.Platforms;
            Languages.ItemsSource = Global.Languages;
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool CheckValues()
        {
            bool valid;
            if (NameBox.Text == "Name..." || NameBox.Text == string.Empty)
            {
                MessageBox.Show("Enter a name for the game!");
                valid = false;
            }

            if (Platforms.SelectedIndex == -1)
            {
                MessageBox.Show("Select a platform for the game!");
                valid = false;
            }
            else
            {
                valid = true;
            }
            return valid;
        }

        private Game AddValuesAsGame()
        {
            Game frogger = new Game();

            frogger.Name = NameBox.Text;
            frogger.Platform = Platforms.SelectedValue.ToString();
            frogger.Language = Languages.Text;
            frogger.Owned = Owned.IsChecked;
            frogger.Cartridge = Cartridge.IsChecked;
            frogger.Box = Box.IsChecked;
            frogger.Manual = Manual.IsChecked;
            frogger.Guide = Guide.IsChecked;
            frogger.NewInBox = NewInBox.IsChecked;

            if (Accessory.Text == "Accessories...")
            {
                frogger.Accessory = string.Empty;
            }
            else { frogger.Accessory = Accessory.Text; }

            if (Notes.Text == "Notes...")
            {
                frogger.Notes = string.Empty;
            }
            else { frogger.Notes = Notes.Text; }

            return frogger;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (CheckValues() == true)
            {
                Game tetris = AddValuesAsGame();
                Global.AddGame(tetris);
                this.OnRefresh();
                this.Close();
            }
        }
    }
}
