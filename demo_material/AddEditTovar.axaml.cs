using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using demo_material.Models;
using MsBox.Avalonia;

namespace demo_material;

public partial class AddEditTovar : Window
{
    User localUser;
    private Tovar updatetovar;
    private string ImageName;
    private string _currentphoto;
    public AddEditTovar() //add
    {
        InitializeComponent();
        DataContext = new Tovar();
        TovarIdTextBox.IsVisible = false;
        LoadSup();
        LoadCat();
        LoadMan();

        EditBut.IsVisible = false;
        DeleteBut.IsVisible = false;
        AddBut.IsVisible = true;

    }


    public AddEditTovar(User user)
    {
        localUser = user;
    }

    public AddEditTovar(User user, Tovar tovar) //edit
    {
        InitializeComponent();
        using var context = new DiplomContext();
        localUser = user;
        updatetovar = tovar;
        DataContext = updatetovar;
        TovarIdTextBox.IsVisible = true;
        ImageBox.Source = updatetovar.GetPhoto;

        LoadSup();
        LoadCat();
        LoadMan();

      


        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;
        AddBut.IsVisible = false;

        Manufacturer.SelectedItem = updatetovar.Manufacturer.ManufacturerName;
        Supplier.SelectedItem = updatetovar.Supplier.SupplierName;
        Category.SelectedItem = updatetovar.Category.CategoryName;


    }


    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }
        else
        {
            var catalogWindow = new CatalogWindow();
            catalogWindow.Show();
            this.Close();
        }
    }


    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new DiplomContext();
            var newTovar = DataContext as Tovar;



            if (string.IsNullOrEmpty(ArticleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле артикула пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(TitleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле наименования пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(UnitBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле единицы измерения пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(CostBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле цены пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(DiscountBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле скидки пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(DescriptionBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле описания пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(QuantityBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле количества на складе пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }



            if (Supplier.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поставщик не выбран", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (Manufacturer.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Производитель не выбран", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (Category.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Категория не выбрана", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            decimal.TryParse(CostBox.Text, out decimal cost);
            int.TryParse(QuantityBox.Text, out int quantity);
            int.TryParse(DiscountBox.Text, out int discount);


            if (cost < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (discount < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Скидка меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (quantity < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }


            if (ImageBox.Source == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Картинка не выбрана", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            newTovar!.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem.ToString())!;
            newTovar!.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem.ToString())!;
            newTovar!.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem.ToString())!;


            newTovar!.Photo = "images/" + ImageName;

            context.Tovars.Add(newTovar!);
            await context.SaveChangesAsync();

            var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар создан", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await message.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var catalogWindow = new CatalogWindow(Class1._user);
                catalogWindow.Show();
                this.Close();
            }
            else
            {
                var catalogWindow = new CatalogWindow();
                catalogWindow.Show();
                this.Close();
            }
        }
        catch (Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();


        }
    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Добавить изображение",
            FileTypeChoices = new[]
            {
            FilePickerFileTypes.All
        }
        });

        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            ImageName = Guid.NewGuid().ToString() + ".png";
            var targetPath = AppDomain.CurrentDomain.BaseDirectory + "/images/" + ImageName;
            File.Copy(file.Path.LocalPath, targetPath);

        }
    }

    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DiplomContext();

        var tovarId = updatetovar.TovarId;
        var tovarToDelete = context.Tovars.Where(x => x.TovarId == tovarId).FirstOrDefault();

        context.Tovars.Remove(tovarToDelete);
        await context.SaveChangesAsync();

        var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар удален", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await message.ShowAsync();

        if (Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }
        else
        {
            var catalogWindow = new CatalogWindow();
            catalogWindow.Show();
            this.Close();
        }
    
    }


    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new DiplomContext();
            var updatetovar = DataContext as Tovar;


            if (string.IsNullOrEmpty(ArticleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле артикула пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(TitleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле наименования пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(UnitBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле единицы измерения пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(CostBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле цены пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(DiscountBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле скидки пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(DescriptionBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле описания пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(QuantityBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле количества на складе пустое", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }



            if (Supplier.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поставщик не выбран", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (Manufacturer.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Производитель не выбран", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (Category.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Категория не выбрана", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            decimal.TryParse(CostBox.Text, out decimal cost);
            int.TryParse(QuantityBox.Text, out int quantity);
            int.TryParse(DiscountBox.Text, out int discount);


            if (cost < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (discount < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Скидка меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }

            if (quantity < 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
                return;
            }




            updatetovar!.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem.ToString())!;
            updatetovar!.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem.ToString())!;
            updatetovar!.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem.ToString())!;


            if (!string.IsNullOrEmpty(ImageName))
            {
                updatetovar?.Photo = "images/" + ImageName;
            }
            else if (!string.IsNullOrEmpty(_currentphoto))
            {
                updatetovar?.Photo = _currentphoto;
            }

            context.Tovars.Update(updatetovar!);
            await context.SaveChangesAsync();

            var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар обновлен", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await message.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var catalogWindow = new CatalogWindow(Class1._user);
                catalogWindow.Show();
                this.Close();
            }
            else
            {
                var catalogWindow = new CatalogWindow();
                catalogWindow.Show();
                this.Close();
            }
        }

        
        catch (Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();


        }
    }

    private void LoadSup()
    {
        using var context = new DiplomContext();
        Supplier.ItemsSource = context.Suppliers.Select(x => x.SupplierName).ToList();
    }

    private void LoadMan()
    {
        using var context = new DiplomContext();
        Manufacturer.ItemsSource = context.Manufacturers.Select(x => x.ManufacturerName).ToList();
    }


    private void LoadCat()
    {
        using var context = new DiplomContext();
        Category.ItemsSource = context.Categories.Select(x => x.CategoryName).ToList();
    }

}