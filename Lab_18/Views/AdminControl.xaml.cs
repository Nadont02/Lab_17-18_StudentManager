using Microsoft.Data.Sqlite;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace Lab_18.Views
{

    public partial class AdminControl : UserControl
    {
        private string dpPath = @"Data Source = C:\SOFT1\authdemo.db";
        public AdminControl()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using var connection = new SqliteConnection(dpPath);
            connection.Open();
            using var command = new SqliteCommand("SELECT * FROM Users",connection);
            using var reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            UsersGrid.ItemsSource = dt.DefaultView;
        }

        private void UsersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is DataRowView row)
            {
                string login = row["Login"].ToString();
                string password = row["Password"].ToString();
                int id = Convert.ToInt32(row["Id"]);
                if (login == "admin" && password == "admin") {
                    MessageBox.Show("Главного администратора нельзя удалять!");
                    return;
                }
                if (MessageBox.Show(
                    $"Удалить пользователя ID ={id}?",
                    "Удаление",
                    MessageBoxButton.YesNo)!=MessageBoxResult.Yes)
                    return;
                using var connection = new SqliteConnection(dpPath);
                connection.Open();

                using var command = new SqliteCommand(
                    "DELETE FROM Users WHERE Id = $id", connection);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                LoadUsers();
            }

        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string login = InputLogin.Text;
            string password = InputPassword.Text;
            string role = (InputRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(login) ||
                    string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(role)) {
                MessageBox.Show("Заполните все поля");
                return;
            }
            using var connection = new SqliteConnection(dpPath);
            connection.Open();
            using var command = new SqliteCommand("UNSERT INTO Users (Login, Password,Role) VALUES($l,$p,$r)", connection);
            command.Parameters.AddWithValue("$l", login);
            command.Parameters.AddWithValue("$p", password);
            command.Parameters.AddWithValue("$r", role);
            command.ExecuteNonQuery();
            InputLogin.Clear();
            InputPassword.Clear();
            InputRole.SelectedIndex = -1;
            LoadUsers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Window.GetWindow(this) as MainWindow)?.SwitchToLogin();
        }
    }
}
