using MVCProjectHamburger.Models.Entities.Abstract;
using MVCProjectHamburger.Models.Enums;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class MenuOrder : BaseRelation
    {
        public MenuOrder()
        {
            ShoppingCarts = new List<ShoppingCart>();
        }
        public int AppUserID { get; set; }
        public AppUser AppUser { get; set; }
        public int OrderID { get; set; }

        public int Number { get; set; }
        public MenuSize MenuSizes { get; set; }

        public int MenuID { get; set; }
        public Menu Menu { get; set; }
        public Order Order { get; set; }

        public List<ShoppingCart> ShoppingCarts { get; set; }
    }
}
