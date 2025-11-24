using System.Windows;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Lab_17_18_StudentManager;

public partial class MainWindow : Window
{
    private const string ConnectionString = @"Data Source = C:\SOFT1\students.db";
    public MainWindow()
    {
        InitializeComponent();
        try
        {

            LoadData();
        } catch (Exception ex) {
            MessageBox.Show(
                $"Ошибка при загрузке данных:{ex.Message}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
    private void LoadData()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        using var command = new SqliteCommand("SELECT ID, Name FROM Students ORDER BY ID", connection);
        using var reader = command.ExecuteReader();
        var dt = new DataTable();
        dt.Load(reader);
        DataGridPeople.ItemsSource = dt.DefaultView;
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var name = InputName.Text?.Trim();
        if (string.IsNullOrEmpty(name)) {
            MessageBox.Show(
                "Введите имя перед добавлением.",
                "Внимание",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }
        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();
            using var command = new SqliteCommand($"INSERT INTO Students (Name) VALUES ($name);", connection);
            command.Parameters.AddWithValue("$name", name);
            command.ExecuteNonQuery();
            LoadData();
            InputName.Clear();
        }
        catch (Exception ex) {
            MessageBox.Show(
                $"Ошибка при добавлении:{ex.Message}",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        } 
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        
    }
}