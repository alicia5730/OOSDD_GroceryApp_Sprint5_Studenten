using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; set; } = new();

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            _ = LoadProductsAsync(); // fire and forget
        }

        private async Task LoadProductsAsync()
        {
            var products = await _productService.GetAllAsync();
            foreach (var p in products)
            {
                Products.Add(p);
            }
        }
    }
}
