using Grocery.App.ViewModels;
using Grocery.Core.Interfaces.Services;

namespace Grocery.App.Views;

public partial class CategoriesView : ContentPage
{
    public CategoriesView()
    {
        InitializeComponent();
        BindingContext = new CategoriesViewModel(
            Application.Current!.Handler!.MauiContext!.Services.GetService<ICategoryService>()!);

    }

    public CategoriesView(CategoriesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as CategoriesViewModel)?.LoadCommand.Execute(null);
    }
}