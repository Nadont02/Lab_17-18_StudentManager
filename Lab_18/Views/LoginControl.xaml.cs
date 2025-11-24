using Microsoft.Data.Sqlite;
using System.Windows;
using System.Windows.Controls;


namespace Lab_18.Views
{

    public partial class LoginControl : UserControl
    {
        private string dbPath = @"Data Source = C:\SOFT1\authdemo.db";
        public LoginControl()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = InputLogin.Text;
            string password = InputPassword.Password;
            
            using var connection = new SqliteConnection(dbPath);
            connection.Open();

            using var command = new SqliteCommand(
                "SELECT Role FROM Users WHERE Login = $l AND Password = $p", connection);
            command.Parameters.AddWithValue("$l", login);
            command.Parameters.AddWithValue("$p", password);
            var role = command.ExecuteScalar()?.ToString();
            if (role == null)
            {
                InfoText.Text = "Неверный логин или пароль";
                return;
            }
            if (role == "Admin")
                (Window.GetWindow(this) as MainWindow)?.SwitchToAdmin();
            else
                (Window.GetWindow(this) as MainWindow)?.SwitchToUser();
        }
    }
}
