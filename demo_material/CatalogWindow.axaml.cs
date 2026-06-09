using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_material.Models;
using Microsoft.EntityFrameworkCore;

namespace demo_material;

public partial class CatalogWindow : Window
{
    User localUser;

    public CatalogWindow()
    {
        InitializeComponent();
        using var context = new DiplomContext();
        Get();
    }


    public CatalogWindow(User user)
    {
        InitializeComponent();
        localUser = user;
        using var context = new DiplomContext();
        Get();


    }

    private void Get()
    {
        using var context = new DiplomContext();
        var allTovars = context.Tovars
                                .Include(x => x.Category)
                                .Include(x => x.Manufacturer)
                                .Include(x => x.Supplier)
                                .ToList();
       

        if(SearchBox.Text != null)
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


}