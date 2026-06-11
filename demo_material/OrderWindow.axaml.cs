using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_material.Models;
using Microsoft.EntityFrameworkCore;

namespace demo_material;

public partial class OrderWindow : Window
{
    public OrderWindow()
    {
        InitializeComponent();
        Get();
    }

    private void Get()
    {
        using var context = new DiplomContext();
        var allOrders = context.Orders
                               .Include(x => x.Status)
                               .Include(x => x.PickUpPoint)
                               .ToList();

        OrdersBox.ItemsSource = allOrders;



    }
}