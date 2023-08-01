using MVCProjectHamburger.Models.Entities.Abstract;
using MVCProjectHamburger.Models.Enums;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class Menu: BaseEntity
    {
        public Menu()
        {
            MenuOrders = new List<MenuOrder>();
        }
       
        public ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
