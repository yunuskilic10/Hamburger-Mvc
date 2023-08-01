using Microsoft.AspNetCore.Identity;
using MVCProjectHamburger.Models.Entities.Concrete;

namespace MVCProjectHamburger.Models.Utilities
{
    public class ForLogin
    {
        public static async void AddSuperUserAsync(UserManager<AppUser> userMan)
        {
            AppUser user = new AppUser
            {
                //Microsoft; aksi soylenmedikçe Identity kullanımında email adreslerini otomatik olarak
                //username kısmına atar...
                //Değiştirmek için; Areas => Identity=> Register => OnPostAsync() metoduna bakınız...
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.com"
            };
            await userMan.CreateAsync(user, "Admin123.");
            await userMan.AddToRoleAsync(user, "Admin");
        }
    }
}
