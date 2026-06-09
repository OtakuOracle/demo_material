using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_material.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace demo_material;

public partial class CatalogWindow : Window
{
    User localUser;

    public CatalogWindow()
    {
        InitializeComponent();
        using var context = new DiplomContext();
        Get();
        LoadBox();
        FioTextBlock.Text = "Гость";
        Visibility(3);
    }


    public CatalogWindow(User user)
    {
        InitializeComponent();
        localUser = user;
        using var context = new DiplomContext();
        Get();
        LoadBox();
        FioTextBlock.Text = user.FullName;
        Visibility(user.RoleId);
    }

    public void Visibility(int roleId)
    {

        switch (roleId)
        {
            case 1: SearchBox.IsVisible = true; SortDiscount.IsVisible = true; SortCost.IsVisible = true; SortQuantity.IsVisible = true; Filter.IsVisible = true;  break;
            case 2: SearchBox.IsVisible = true; SortDiscount.IsVisible = true; SortCost.IsVisible = true; SortQuantity.IsVisible = true; Filter.IsVisible = true; break;
        }
    }

    private void Get()
    {
        using var context = new DiplomContext();
        var allTovars = context.Tovars
                                .Include(x => x.Category)
                                .Include(x => x.Manufacturer)
                                .Include(x => x.Supplier)
                                .ToList();


        if (Filter.SelectedItem != null && Filter.SelectedIndex != 0)
        {
            allTovars = allTovars.Where(x => x.Manufacturer.ManufacturerName == Filter.SelectedItem.ToString()).ToList();
        }

        switch (SortQuantity.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Quantity).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Quantity).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Quantity).ToList();
                break;
        }


        switch (SortCost.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Cost).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Cost).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Cost).ToList();
                break;
        }

        switch (SortDiscount.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Discount).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Discount).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Discount).ToList();
                break;
        }



        if (SearchBox.Text != null)
        {
            allTovars = allTovars.Where(x => 
                x.Article.ToLower().Contains(SearchBox.Text) ||
                x.Title.ToLower().Contains(SearchBox.Text) || 
                x.Unit.ToLower().Contains(SearchBox.Text) || 
                x.Description.ToLower().Contains(SearchBox.Text) || 
                x.Category.CategoryName.ToLower().Contains(SearchBox.Text) || 
                x.Supplier.SupplierName.ToLower().Contains(SearchBox.Text) ||
                x.Manufacturer.ManufacturerName.ToLower().Contains(SearchBox.Text)
                ).ToList();

        }
        TovarsBox.ItemsSource = allTovars;

    }

    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        Get(); // Обновляем каталог при вводе текста в SearchBox
    }

    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении фильтра

    }

    private void SortQuantity_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении фильтра

    }

    private void SortCost_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении фильтра

    }

    private void SortDiscount_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); // Обновляем каталог при изменении фильтра

    }

    private void LoadBox()
    {
        using var context = new DiplomContext();
        var man = context.Manufacturers.Select(x => x.ManufacturerName).ToList();
        man.Add("Все производители");
        Filter.ItemsSource = man.OrderByDescending(x => x == "Все производители");
        Filter.SelectedIndex = 0;

    }

    

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();

    }

}