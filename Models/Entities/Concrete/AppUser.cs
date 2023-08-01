using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCProjectHamburger.Models.Entities.Concrete
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
            MenuOrders = new List<MenuOrder>();
        }
        [ForeignKey("AppUserID")]
        public ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
