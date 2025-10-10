using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.App.Views;  

namespace Grocery.App.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {   
        private readonly ICategoryService categoryService;

        public CategoriesViewModel(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        
        public ObservableCollection<Category> Categories { get; } = new();
        
        [ObservableProperty] 
        private Category? selectedCategory;

        [RelayCommand]
        public async Task LoadAsync()
        {
            if (Categories.Count > 0) return;
            var items = await categoryService.GetAllAsync();
            foreach (var c in items) Categories.Add(c);
        }

        partial void OnSelectedCategoryChanged(Category? value)
        {
            if (value is null) return;

            var navParams = new Dictionary<string, object>
            {
                { nameof(Category), value }
            };

            Shell.Current.GoToAsync(nameof(ProductCategoriesView), navParams);

            SelectedCategory = null;
        }

    }
}

