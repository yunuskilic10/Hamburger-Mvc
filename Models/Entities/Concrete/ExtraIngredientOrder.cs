using MVCProjectHamburger.Models.Entities.Abstract;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class ExtraIngredientOrder:BaseRelation
    {
        public ExtraIngredientOrder()
        {
            ShoppingCarts = new List<ShoppingCart>();
        }
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int Number { get; set; }
        public int ExtraIngredientID { get; set; }
        public int OrderID { get; set; }

        public ExtraIngredient? ExtraIngredient { get; set; }
        public Order Order { get; set; }

        public List<ShoppingCart> ShoppingCarts { get; set; }

    }
}
