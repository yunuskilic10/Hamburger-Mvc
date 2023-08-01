using MVCProjectHamburger.Models.Entities.Concrete;

namespace MVCProjectHamburger.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? MenuSize { get; set; }
        public int TotalPrice { get; set; }

        public Order Order { get; set; }
        public ExtraIngredientOrder? ExtraIngredientOrder { get; set; }
        public MenuOrder? MenuOrder { get; set; }
    }
}
