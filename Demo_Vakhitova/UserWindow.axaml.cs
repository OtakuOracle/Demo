using Avalonia.Controls;
using Avalonia.Interactivity;
using Demo_Vakhitova.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Vakhitova;

public partial class UserWindow : Window
{
    private readonly PostgresContext _context;

    private List<Listtovar> _allTovarList;
    public UserWindow()
    {
        _context = new PostgresContext();
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        await LoadAndDisplayTovars();
    }

    private async Task LoadAndDisplayTovars()
    {
        try
        {

            _allTovarList = await _context.Listtovars
                .Include(x => x.Category)
                .Include(x => x.Postavschik)
                .Include(x => x.Proizv)
                .Include(x => x.Tovar)
                .ToListAsync();

            ListBoxTovar.ItemsSource = _allTovarList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
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

        var addNewProductWindow = new AddNewProduct(_context);

        addNewProductWindow.Closed += async (s, args) =>
        {
            await LoadAndDisplayTovars();
        };

        addNewProductWindow.Show();
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        PerformSearch();
    }

    private void PerformSearch()
    {

        if (_allTovarList == null)
        {
            ListBoxTovar.ItemsSource = null;
            return;
        }

        var searchTerm = SearchTextBox.Text?.Trim().ToLower();

        if (string.IsNullOrEmpty(searchTerm))
        {
            ListBoxTovar.ItemsSource = _allTovarList;
        }
        else
        {
            try
            {
                var filteredList = _allTovarList.Where(t =>
                    t.Tovar != null && t.Tovar.TovarName.ToLower().Contains(searchTerm) ||
                    t.Category != null && t.Category.CategoryName.ToLower().Contains(searchTerm) ||
                    t.Description.ToLower().Contains(searchTerm)
                ).ToList();

                ListBoxTovar.ItemsSource = filteredList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при поиске: {ex.Message}");
            }
        }
    }

    private void EditTovar_Click(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (ListBoxTovar.SelectedItem is Listtovar listtovar)
        {
            var editProduct = new EditTovar(listtovar);
            editProduct.Show();
            this.Close();
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

