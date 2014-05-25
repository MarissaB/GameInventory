using System;
using System.Windows;
using System.Windows.Controls;

namespace GameInventory
{
    /// <summary>
    /// Window to data for existing game.
    /// </summary>
    public partial class EditGame : Window
    {
        public event EventHandler RefreshEvent;

        protected void OnRefresh()
        {
            if (this.RefreshEvent != null)
                this.RefreshEvent(this, EventArgs.Empty);
        }

        public EditGame()
        {
            InitializeComponent();

            Platforms.ItemsSource = Global.Platforms;
            Languages.ItemsSource = Global.Languages;

            PopulateFields(Global.SelectedGame);
        }

        private void PopulateFields(Game tetris)
        {
            NameBox.Text = Global.SelectedGame.Name;
            Platforms.SelectedValue = Global.SelectedGame.Platform;
            Languages.SelectedValue = Global.SelectedGame.Language;
            Owned.IsChecked = Global.SelectedGame.Owned;
            Cartridge.IsChecked = Global.SelectedGame.Cartridge;
            Box.IsChecked = Global.SelectedGame.Box;
            Manual.IsChecked = Global.SelectedGame.Manual;
            Guide.IsChecked = Global.SelectedGame.Guide;
            NewInBox.IsChecked = Global.SelectedGame.NewInBox;
            
            if (Global.SelectedGame.Accessory != string.Empty)
            {
                Accessory.Text = Global.SelectedGame.Accessory;
            }
            else { Accessory.Text = "Accessories..."; }

            if (Global.SelectedGame.Notes != string.Empty)
            {
                Notes.Text = Global.SelectedGame.Notes;
            }
            else { Notes.Text = "Notes..."; }
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

        private void AddValuesAsGame()
        {
            Global.SelectedGame.Name = NameBox.Text;
            Global.SelectedGame.Platform = Platforms.SelectedValue.ToString();
            Global.SelectedGame.Language = Languages.Text;
            Global.SelectedGame.Owned = Owned.IsChecked;
            Global.SelectedGame.Cartridge = Cartridge.IsChecked;
            Global.SelectedGame.Box = Box.IsChecked;
            Global.SelectedGame.Manual = Manual.IsChecked;
            Global.SelectedGame.Guide = Guide.IsChecked;
            Global.SelectedGame.NewInBox = NewInBox.IsChecked;

            if (Accessory.Text == "Accessories...")
            {
                Global.SelectedGame.Accessory = string.Empty;
            }
            else { Global.SelectedGame.Accessory = Accessory.Text; }

            if (Notes.Text == "Notes...")
            {
                Global.SelectedGame.Notes = string.Empty;
            }
            else { Global.SelectedGame.Notes = Notes.Text; }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (CheckValues() == true)
            {
                AddValuesAsGame();
                Global.EditGame(Global.SelectedGame);
                this.OnRefresh();
                this.Close();
            }
        }
    }
}
