using MVCProjectHamburger.Models.Entities.Abstract;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class ShoppingCart:BaseRelation
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Name { get; set; }
        public int TotalPrice { get; set; }
        public int Number { get; set; }
        public string? MenuSize { get; set; }


        public int? MenuOrderId { get; set; }
        public MenuOrder? MenuOrder { get; set; }
        public int? ExtraIngredientOrderId { get; set; }
        public ExtraIngredientOrder? ExtraIngredientOrder { get; set; }
    }
}
