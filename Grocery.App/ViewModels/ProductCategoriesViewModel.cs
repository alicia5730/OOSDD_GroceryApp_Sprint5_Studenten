using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(Category), nameof(Category))]
    public partial class ProductCategoriesViewModel : BaseViewModel
    {
        private readonly IProductCategoryService _productCategoryService;

        [ObservableProperty]
        private Category category;

        [ObservableProperty]
        private string categoryName;

        public ObservableCollection<Product> Products { get; set; } = new();

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        partial void OnCategoryChanged(Category value)
        {
            if (value == null) return;
            CategoryName = value.Name;
            _ = LoadProductsAsync(value.Id);
        }

        private async Task LoadProductsAsync(int categoryId)
        {
            Products.Clear();
            var products = await _productCategoryService.GetByCategoryIdAsync(categoryId);
            foreach (var product in products)
                Products.Add(product);
        }
    }
}