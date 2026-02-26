using Avalonia.Controls;
using Avalonia.Interactivity;
using Demo_Vakhitova.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input; 
using Avalonia.Controls.Primitives; 

namespace Demo_Vakhitova;

public partial class UserWindow : Window
{
    private readonly PostgresContext _context;

    private List<Listtovar> _allTovarList;
    private List<Listtovar> _displayedTovarList; 

    public UserWindow()
    {
        _context = new PostgresContext();
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        await LoadAndDisplayTovars();
        await LoadSuppliersIntoFilter(); 
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

            ApplyFiltersAndSort();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
        }
    }

    private async Task LoadSuppliersIntoFilter()
    {
        try
        {
            var suppliers = await _context.Postavschiks.OrderBy(s => s.PostavschikName).ToListAsync();

            var existingItems = SupplierFilterComboBox.Items.OfType<ComboBoxItem>().ToList();
            foreach (var item in existingItems.Where(i => !i.Tag?.ToString().Equals("AllSuppliers") ?? true))
            {
                SupplierFilterComboBox.Items.Remove(item);
            }

            if (!SupplierFilterComboBox.Items.OfType<ComboBoxItem>().Any(i => i.Tag?.ToString() == "AllSuppliers"))
            {
                SupplierFilterComboBox.Items.Insert(0, new ComboBoxItem { Content = "Все поставщики", Tag = "AllSuppliers" });
            }

            foreach (var supplier in suppliers)
            {
                SupplierFilterComboBox.Items.Add(new ComboBoxItem { Content = supplier.PostavschikName, Tag = supplier.IdPostavschik });
            }

            SupplierFilterComboBox.SelectedIndex = 0; 
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка загрузки поставщиков: {ex.Message}");
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
        ApplyFiltersAndSort(); 
    }

    private void ResetSearchButton_Click(object? sender, RoutedEventArgs e)
    {
        SearchTextBox.Text = "";
        SupplierFilterComboBox.SelectedIndex = 0; 
        ApplyFiltersAndSort(); 
    }

    private void SortComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFiltersAndSort(); 
    }

    private void SupplierFilterComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFiltersAndSort(); 
    }

    private void ApplyFiltersAndSort()
    {
        if (_allTovarList == null) return;

        //Фильтрация по тексту поиска
        var searchTerm = SearchTextBox.Text?.Trim().ToLower();
        IEnumerable<Listtovar> filteredList = _allTovarList;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            filteredList = filteredList.Where(t =>
                t.Tovar != null && t.Tovar.TovarName.ToLower().Contains(searchTerm) ||
                t.Category != null && t.Category.CategoryName.ToLower().Contains(searchTerm) ||
                t.Description != null && t.Description.ToLower().Contains(searchTerm)
            );
        }

        //Фильтрация по поставщику
        if (SupplierFilterComboBox.SelectedItem is ComboBoxItem selectedSupplierItem)
        {
            var supplierId = selectedSupplierItem.Tag?.ToString();

            if (!string.IsNullOrEmpty(supplierId) && !supplierId.Equals("AllSuppliers", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(supplierId, out int id))
                {
                    filteredList = filteredList.Where(t => t.IdPostavschik == id);
                }
            }
        }

        //Применение сортировки
        var selectedSortOption = SortComboBox.SelectedItem as ComboBoxItem;
        var sortOrder = selectedSortOption?.Tag?.ToString();

        switch (sortOrder)
        {
            case "Ascending":
                _displayedTovarList = filteredList.OrderBy(t => t.Kolvo).ToList();
                break;
            case "Descending":
                _displayedTovarList = filteredList.OrderByDescending(t => t.Kolvo).ToList();
                break;
            default: 
                _displayedTovarList = filteredList.ToList(); 
                break;
        }

        ListBoxTovar.ItemsSource = _displayedTovarList; 
    }


    private void EditTovar_Click(object? sender, TappedEventArgs e)
    {
        if (ListBoxTovar.SelectedItem is Listtovar listtovar)
        {
            var editProduct = new EditTovar(listtovar);
            editProduct.Closed += async (s, args) =>
            {
                await LoadAndDisplayTovars(); 
            };
            editProduct.Show();
            this.Close();
        }
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
