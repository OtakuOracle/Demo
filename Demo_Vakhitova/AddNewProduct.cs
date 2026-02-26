using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Demo_Vakhitova.Models;.
using Demo_Vakhitova.Data;
//using Avalonia.Controls.Platform.Dialogs

namespace Demo_Vakhitova;

public partial class AddProductWindow : Window
{
    public int? SelectedTovarId { get; set; }
    public int? SelectedCategoryId { get; set; }
    public int? SelectedProizvId { get; set; }
    public int? SelectedPostavschikId { get; set; }

    public AddProductWindow()
    {
        InitializeComponent();
        LoadComboBoxData();
    }

    private void LoadComboBoxData()
    {
        try
        {
            Name_ComboBox.ItemsSource = PostgresContext.Instance.Tovars
                .Select(x => x.TovarName).ToList();
            Category_ComboBox.ItemsSource = PostgresContext.Instance.Categories
                .Select(x => x.CategoryName).ToList();
            Proizv_ComboBox.ItemsSource = PostgresContext.Instance.Proizvs
                .Select(x => x.ProizvName).ToList();
            Postavschik_ComboBox.ItemsSource = PostgresContext.Instance.Postavschiks
                .Select(x => x.PostavschikName).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
        }
    }

    private void SelectedName_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Name_ComboBox.SelectedItem != null)
        {
            var selectedName = Name_ComboBox.SelectedItem.ToString()!;
            SelectedTovarId = PostgresContext.Instance.Tovars
                .FirstOrDefault(t => t.TovarName == selectedName)?.TovarId;
        }
    }

    private void SelectedCategory_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Category_ComboBox.SelectedItem != null)
        {
            var selectedCategory = Category_ComboBox.SelectedItem.ToString()!;
            SelectedCategoryId = PostgresContext.Instance.Categories
                .FirstOrDefault(c => c.CategoryName == selectedCategory)?.CategoryId;
        }
    }

    private void SelectedProizv_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Proizv_ComboBox.SelectedItem != null)
        {
            var selectedProizv = Proizv_ComboBox.SelectedItem.ToString()!;
            SelectedProizvId = PostgresContext.Instance.Proizvs
                .FirstOrDefault(p => p.ProizvName == selectedProizv)?.ProizvId;
        }
    }

    private void SelectedPostavschik_ComboBox(object? sender, SelectionChangedEventArgs e)
    {
        if (Postavschik_ComboBox.SelectedItem != null)
        {
            var selectedPostavschik = Postavschik_ComboBox.SelectedItem.ToString()!;
            SelectedPostavschikId = PostgresContext.Instance.Postavschiks
                .FirstOrDefault(p => p.PostavschikName == selectedPostavschik)?.PostavschikId;
        }
    }

    private void AddProduct_OnClick(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Description_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Count_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Unity_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Kolvo_TextBox.Text) ||
            string.IsNullOrWhiteSpace(Discountnow_TextBox.Text))
        {
            MessageBox.Show("Заполните все текстовые поля!", "Ошибка валидации");
            return;
        }

        if (!int.TryParse(Count_TextBox.Text, out int count) ||
            !int.TryParse(Kolvo_TextBox.Text, out int kolvo) ||
            !int.TryParse(Discountnow_TextBox.Text, out int discount))
        {
            MessageBox.Show("Проверьте корректность", "Ошибка числовых данных");
            return;
        }

        if (!SelectedTovarId.HasValue || !SelectedCategoryId.HasValue ||
            !SelectedProizvId.HasValue || !SelectedPostavschikId.HasValue)
        {
            MessageBox.Show("Выберите все значения", "Ошибка выбора");
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

            PostgresContext.Instance.Listtovars.Add(newListtovar);
            PostgresContext.Instance.SaveChanges();

            MessageBox.Show(" Товар добавлен!", "Успех");

            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($" Ошибка при сохранении в БД:\n{ex.Message}", "Ошибка базы данных");
        }
    }


}