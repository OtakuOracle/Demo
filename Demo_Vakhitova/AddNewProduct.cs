using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Demo_Vakhitova.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo_Vakhitova;

public partial class AddNewProduct : Window
{
    private readonly PostgresContext _context;
    public int? SelectedTovarId { get; set; }
    public int? SelectedCategoryId { get; set; }
    public int? SelectedProizvId { get; set; }
    public int? SelectedPostavschikId { get; set; }

    public AddNewProduct(PostgresContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        await LoadComboBoxData();
    }

    private async Task LoadComboBoxData()
    {
        try
        {
            Name_ComboBox.ItemsSource = await _context.Tovars
                .Select(x => x.TovarName)
                .Distinct()
                .ToListAsync();

            Category_ComboBox.ItemsSource = await _context.Categories
                .Select(x => x.CategoryName)
                .Distinct()
                .ToListAsync();

            Proizv_ComboBox.ItemsSource = await _context.Proizvs
                .Select(x => x.ProizvName)
                .Distinct()
                .ToListAsync();

            Postavschik_ComboBox.ItemsSource = await _context.Postavschiks
                .Select(x => x.PostavschikName)
                .Distinct()
                .ToListAsync();
        }
        catch (Exception ex)
        {

            await MessageBox.ShowAsync($": {ex.Message}", "Ошибка загрузки данных");
        }
    }

    private async void SelectedName_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Name_ComboBox.SelectedItem != null)
        {
            var selectedName = Name_ComboBox.SelectedItem.ToString()!;
            SelectedTovarId = (await _context.Tovars
                .FirstOrDefaultAsync(t => t.TovarName == selectedName))?.TovarId;
        }
    }

    private async void SelectedCategory_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Category_ComboBox.SelectedItem != null)
        {
            var selectedCategory = Category_ComboBox.SelectedItem.ToString()!;
            SelectedCategoryId = (await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == selectedCategory))?.CategoryId;
        }
    }

    private async void SelectedProizv_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Proizv_ComboBox.SelectedItem != null)
        {
            var selectedProizv = Proizv_ComboBox.SelectedItem.ToString()!;
            SelectedProizvId = (await _context.Proizvs
                .FirstOrDefaultAsync(p => p.ProizvName == selectedProizv))?.ProizvId;
        }
    }

    private async void SelectedPostavschik_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Postavschik_ComboBox.SelectedItem != null)
        {
            var selectedPostavschik = Postavschik_ComboBox.SelectedItem.ToString()!;
            SelectedPostavschikId = (await _context.Postavschiks
                .FirstOrDefaultAsync(p => p.PostavschikName == selectedPostavschik))?.PostavschikId;
        }
    }



    private async void AddProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Description_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Count_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Unity_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Kolvo_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Discountnow_TextBox.Text))
        {
            await MessageBox.ShowAsync("Пожалуйста, заполните все текстовые поля.", "Ошибка");
            return;
        }

        if (!int.TryParse(Count_TextBox.Text, out int count) ||
            !int.TryParse(Kolvo_TextBox.Text, out int kolvo) ||
            !int.TryParse(Discountnow_TextBox.Text, out int discount))
        {
            await MessageBox.ShowAsync("Проверьте данные", "Ошибка");
            return;
        }

        if (!SelectedTovarId.HasValue || !SelectedCategoryId.HasValue ||
            !SelectedProizvId.HasValue || !SelectedPostavschikId.HasValue)
        {
            await MessageBox.ShowAsync("Пожалуйста, выберите товар, категорию, производителя и поставщика.", "Ошибка");
            return;
        }

        try
        {
            var newListtovar = new Listtovar
            {
                Description = Description_TextBox.Text,
                Count = count,
                Unity = Unity_TextBox.Text,
                Kolvo = kolvo,
                Discountnow = discount,
                TovarId = SelectedTovarId.Value,
                PostavschikId = SelectedPostavschikId.Value,
                ProizvId = SelectedProizvId.Value,
                CategoryId = SelectedCategoryId.Value
            };

            _context.Listtovars.Add(newListtovar);
            await _context.SaveChangesAsync();

            await MessageBox.ShowAsync("Продукт успешно добавлен!", "Успех");

            Close();
        }
        catch (Exception ex)
        {
            await MessageBox.ShowAsync($"Ошибка при добавлении продукта:\n{ex.Message}", "Ошибка");
        }
    }
}
