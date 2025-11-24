using System.Windows;
using Lab_18.Views;

namespace Lab_18;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new LoginControl();
    }
    public void SwitchToAdmin()
    {
        MainContent.Content = new AdminControl();
    }
    public void SwitchToUser()
    {
        MainContent.Content = new UsersControl();
    }
    public void SwitchToLogin()
    {
        MainContent.Content = new LoginControl();
    }
}