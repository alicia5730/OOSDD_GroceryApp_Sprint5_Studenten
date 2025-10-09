using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        private readonly IFileSaverService _fileSaverService;
        private string searchText = "";

        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = new();
        public ObservableCollection<Product> AvailableProducts { get; set; } = new();

        [ObservableProperty]
        GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        [ObservableProperty]
        string myMessage;

        public GroceryListItemsViewModel(
            IGroceryListItemsService groceryListItemsService,
            IProductService productService,
            IFileSaverService fileSaverService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            _fileSaverService = fileSaverService;

            _ = LoadAsync(groceryList.Id); // fire async on init
        }

        private async Task LoadAsync(int id)
        {
            MyGroceryListItems.Clear();

            var listItems = _groceryListItemsService.GetAllOnGroceryListId(id);
            foreach (var item in listItems)
                MyGroceryListItems.Add(item);

            await GetAvailableProductsAsync();
        }

        private async Task GetAvailableProductsAsync()
        {
            AvailableProducts.Clear();

            var allProducts = await _productService.GetAllAsync();

            foreach (Product p in allProducts)
            {
                bool alreadyInList = MyGroceryListItems.Any(g => g.ProductId == p.Id);
                bool matchesSearch = string.IsNullOrWhiteSpace(searchText)
                                     || p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase);

                if (!alreadyInList && p.Stock > 0 && matchesSearch)
                    AvailableProducts.Add(p);
            }
        }

        partial void OnGroceryListChanged(GroceryList value)
        {
            _ = LoadAsync(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> parameter = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, parameter);
        }

        [RelayCommand]
        public async Task AddProduct(Product product)
        {
            if (product == null) return;

            GroceryListItem item = new(0, GroceryList.Id, product.Id, 1);
            _groceryListItemsService.Add(item);

            product.Stock--;
            await _productService.UpdateAsync(product);

            AvailableProducts.Remove(product);
            await GetAvailableProductsAsync();
        }

        [RelayCommand]
        public async Task ShareGroceryList(CancellationToken cancellationToken)
        {
            if (GroceryList == null || MyGroceryListItems == null) return;
            string jsonString = JsonSerializer.Serialize(MyGroceryListItems);

            try
            {
                await _fileSaverService.SaveFileAsync("Boodschappen.json", jsonString, cancellationToken);
                await Toast.Make("Boodschappenlijst is opgeslagen.").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"Opslaan mislukt: {ex.Message}").Show(cancellationToken);
            }
        }

        [RelayCommand]
        public async Task PerformSearch(string searchText)
        {
            this.searchText = searchText;
            await GetAvailableProductsAsync();
        }

        [RelayCommand]
        public async Task IncreaseAmount(int productId)
        {
            GroceryListItem? item = MyGroceryListItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return;
            if (item.Amount >= item.Product.Stock) return;

            item.Amount++;
            _groceryListItemsService.Update(item);
            item.Product.Stock--;

            await _productService.UpdateAsync(item.Product);
            await GetAvailableProductsAsync();
        }

        [RelayCommand]
        public async Task DecreaseAmount(int productId)
        {
            GroceryListItem? item = MyGroceryListItems.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return;
            if (item.Amount <= 0) return;

            item.Amount--;
            _groceryListItemsService.Update(item);
            item.Product.Stock++;

            await _productService.UpdateAsync(item.Product);
            await GetAvailableProductsAsync();
        }
    }
}
