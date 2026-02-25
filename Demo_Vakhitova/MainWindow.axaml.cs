using Avalonia.Controls;
using Avalonia.Interactivity;
using Demo_Vakhitova.Models;
using System;
using System.Linq;

namespace Demo_Vakhitova;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void TogglePasswordVisibility(object? sender, RoutedEventArgs e)
    {
        PasswordTextBox.PasswordChar = PasswordTextBox.PasswordChar == '*' ? '\0' : '*';
    }


    private void AuthorizeButton(object? sender, RoutedEventArgs e)
    {
        try
        {
            using ( PostgresContext db = new PostgresContext())
            {
                var user = db.Users.FirstOrDefault(it => it.Login == LoginTextBox.Text && it.Password == PasswordTextBox.Text);
                if (user != null)
                {

                    var roleId = user.RoleId;

                    switch (roleId)
                    {
                        case 1:
                        case 2:
                            UserWindow userWindow = new UserWindow();
                            userWindow.Show();
                            this.Close();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    ErrorBlock.Text = "Неверный пароль или логин";
                }
            }
        }
        catch (Exception ex)
        {
            ErrorBlock.Text = ": " + ex.Message;
        }
    }
}