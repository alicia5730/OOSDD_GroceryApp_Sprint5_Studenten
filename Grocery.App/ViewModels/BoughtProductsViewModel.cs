using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;
        private readonly IProductService _productService;

        [ObservableProperty]
        private Product selectedProduct;

        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();

        public BoughtProductsViewModel(
            IBoughtProductsService boughtProductsService,
            IProductService productService)
        {
            _boughtProductsService = boughtProductsService;
            _productService = productService;

            _ = LoadProductsAsync(); // fire and forget async call
        }

        private async Task LoadProductsAsync()
        {
            var allProducts = await _productService.GetAllAsync();
            foreach (var product in allProducts)
            {
                Products.Add(product);
            }
        }

        partial void OnSelectedProductChanged(Product? oldValue, Product newValue)
        {
            _ = LoadBoughtProductsAsync(newValue);
        }

        private async Task LoadBoughtProductsAsync(Product product)
        {
            BoughtProductsList.Clear();

            // Assuming Get() is synchronous — we can wrap it in Task.Run if needed
            var list = await Task.Run(() => _boughtProductsService.Get(product.Id));

            foreach (var item in list)
            {
                BoughtProductsList.Add(item);
            }
        }

        [RelayCommand]
        public void NewSelectedProduct(Product product)
        {
            SelectedProduct = product;
        }
    }
}
