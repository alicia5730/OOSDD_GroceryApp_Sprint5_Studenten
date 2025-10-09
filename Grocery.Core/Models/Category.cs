using CommunityToolkit.Mvvm.ComponentModel;


namespace Grocery.Core.Models
{
    public class Category : Model
    {
        public Category(int id, string name) : base(id, name){}
        public override string ToString() => Name;

    }

}

