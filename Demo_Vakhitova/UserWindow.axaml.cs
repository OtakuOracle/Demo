using Avalonia.Controls;
using Avalonia.Interactivity;
using Demo_Vakhitova.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo_Vakhitova;

public partial class UserWindow : Window
{
    public static readonly PostgresContext context = new PostgresContext();
    public List<Listtovar> listtovar = context.Listtovars
        .Include(x => x.Category)
        .Include(x => x.Postavschik)
        .Include(x => x.Proizv)
        .Include(x => x.Tovar).ToList();
    
    public UserWindow()
    {
        InitializeComponent();
        ListBoxTovar.ItemsSource = listtovar;

    }


    private void Button_GoBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }


    private void AddNewProduct_Click(object? sender, RoutedEventArgs e)
    {
        var addNewProduct = new AddNewProduct();
        addNewProduct.Show();
    }

}
