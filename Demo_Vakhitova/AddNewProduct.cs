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
    private string? _imageName = null;
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
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка загрузки данных", "Ошибка", ButtonEnum.Ok);
            var result = await box.ShowAsync();
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
            var box = MessageBoxManager.GetMessageBoxStandard("Заполните все поля", "Ошибка", ButtonEnum.Ok);
            var result = await box.ShowAsync();
        }

        if (!int.TryParse(Count_TextBox.Text, out int cena) ||
            !int.TryParse(Kolvo_TextBox.Text, out int kolich) ||
            !int.TryParse(Discountnow_TextBox.Text, out int disc))
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Проверьте корректность ввода числовых значений", "Ошибка ввода", ButtonEnum.Ok);
            var result = await box.ShowAsync();
        }

        if (!SelectedTovarId.HasValue || !SelectedCategoryId.HasValue ||
            !SelectedProizvId.HasValue || !SelectedPostavschikId.HasValue)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Пожалуйста, выберите товар, категорию, производителя и поставщика", "Ошибка выбора", ButtonEnum.Ok);
            var result = await box.ShowAsync();
        }

        try
        {
            var count = int.Parse(Count_TextBox.Text);
            var kolvo = int.Parse(Kolvo_TextBox.Text);
            var discount = int.Parse(Discountnow_TextBox.Text);
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
                CategoryId = SelectedCategoryId.Value,
                Photo = _imageName
            };

            _context.Listtovars.Add(newListtovar);
            await _context.SaveChangesAsync();

            var box = MessageBoxManager.GetMessageBoxStandard("Успешное добавление", "Добавлен", ButtonEnum.Ok);
            var result = await box.ShowAsync();
            Close();
        }
        catch (Exception ex)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка добавления", "Не добавлен", ButtonEnum.Ok);
            var result = await box.ShowAsync();
        }
    }
        private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        try
        {
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                Title = "Выбор изображения",
                FileTypeFilter = new[] { Avalonia.Platform.Storage.FilePickerFileTypes.ImageAll }
            });

            if (files.Count > 0)
            {
                _imageName = files[0].Name;
                using var stream = await files[0].OpenReadAsync();
                ImageBox.Source = new Avalonia.Media.Imaging.Bitmap(stream);
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                $"Не удалось загрузить изображение: {ex.Message}",
                MsBox.Avalonia.Enums.ButtonEnum.Ok,
                MsBox.Avalonia.Enums.Icon.Error
            ).ShowAsync();
        }
    }
}
