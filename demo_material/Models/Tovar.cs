using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace demo_material.Models;

public partial class Tovar
{
    public int TovarId { get; set; }

    public string Article { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int Cost { get; set; }
    public decimal Total
    {
        get
        {
            var a = Cost / 100;
            var b = Discount * a;
            var c = Cost - b;
            return (decimal)c;
        }
    }

    public int SupplierId { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public int Discount { get; set; }

    public string ColourDiscount
    {
        get
        {
            if(Discount > 12)
            {
                return "#F4A460";
            }
            else
            {
                return "";
            }
        }
    }


    public int Quantity { get; set; }

    public string ColourQuantity
    {
        get
        {
            if(Quantity == 0)
            {
                return "Blue";
            }
            else
            {
                return "";
            }
        }
    }

    public bool isAdmin
    {
        get
        {
            return Class1.isAdmin;
        }
    }

    public string Description { get; set; } = null!;

    public string? Photo { get; set; }

    public Bitmap GetPhoto
    {
        get
        {
            if (Photo != null && Photo != "")
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Photo);
            }
            else
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/images/not.png");
            }
        }
    }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<OrderTovar> OrderTovars { get; set; } = new List<OrderTovar>();

    public virtual Supplier Supplier { get; set; } = null!;
}
