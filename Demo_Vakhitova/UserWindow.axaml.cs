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
    private readonly PostgresContext _context = new PostgresContext();

    private List<Listtovar> _allTovarList;

    public UserWindow()
    {
        InitializeComponent();
        LoadAndDisplayTovars();
    }

    private void LoadAndDisplayTovars()
    {
        try
        {
            _allTovarList = _context.Listtovars
                .Include(x => x.Category)
                .Include(x => x.Postavschik)
                .Include(x => x.Proizv)
                .Include(x => x.Tovar).ToList();

            ListBoxTovar.ItemsSource = _allTovarList;
        }
        catch (Exception ex)
        {
            ShowMessageBox($"Ошибка загрузки товаров: {ex.Message}", "Ошибка");
        }
    }

    private void Button_GoBack(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close(); 
    }

    private void AddNewProduct_Click(object? sender, RoutedEventArgs e)
    {
        var addNewProduct = new AddProductWindow(); 
        addNewProduct.Show();

        addNewProduct.Closed += (s, args) =>
        {
            LoadAndDisplayTovars();
        };
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        PerformSearch(); 
    }

    private void PerformSearch()
    {
        if (_allTovarList == null)
        {
            return;
        }

        var searchTerm = SearchTextBox.Text?.Trim().ToLower();

        if (string.IsNullOrEmpty(searchTerm))
        {
            ListBoxTovar.ItemsSource = _allTovarList;
        }
        else
        {
            var filteredList = _allTovarList.Where(t =>
                t.Tovar.TovarName.ToLower().Contains(searchTerm) ||
                t.Category.CategoryName.ToLower().Contains(searchTerm) ||
                t.Description.ToLower().Contains(searchTerm)
            ).ToList();

            ListBoxTovar.ItemsSource = filteredList;
        }
    }

    private void ResetSearchButton_Click(object? sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = ""; 
        PerformSearch(); 
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (_context != null)
        {
            _context.Dispose(); 
        }
    }
}
