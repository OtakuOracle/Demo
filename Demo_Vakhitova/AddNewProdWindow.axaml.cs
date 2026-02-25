using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Demo_Vakhitova.Models;

namespace Demo_Vakhitova;

public partial class AddNewProdWindow : Window
{
    public AddNewProdWindow()
    {
        InitializeComponent();
    }


    private void AddProduct_OnClick(object? sender RoutedEventArgs e)
    {
        var newListtovar = new Listtovar
        {
            Description = Description_TextBox.Text,
            Count = Count_TextBox.Text,
            Unity = Unity_TextBox.Text,
            Kolvo = Kolvo_TextBox.Text,
            Discountnow = Discountnow_TextBox.Text,

        }

    }



}