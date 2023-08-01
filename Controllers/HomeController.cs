using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProjectHamburger.Models;
using MVCProjectHamburger.Models.Entities.Concrete;
using System.Diagnostics;

namespace MVCProjectHamburger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            int? id = GetUserID();
           
            return View(id);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private int GetUserID()
        {
            //Bu kodda int.TryParse() yöntemi, userManager.GetUserId(User) değerini bir tamsayıya dönüştürmeye çalışır. Eğer dönüştürme başarılı olursa, result değişkenine sonucu atar ve id değişkenine result'ı nullable bir tamsayı olarak atar. Eğer dönüştürme başarısız olursa, id null değeri alır.
            //Sonrasında ise null koalesans operatörü(??) ile id değeri null ise - 1 değerini döndürürsünüz. Bu şekilde null döndürme durumunda hata almazsınız ve - 1 değeriyle devam edebilirsiniz.

            int? id = int.TryParse(userManager.GetUserId(User), out int result) ? (int?)result : null;
            return id ?? -1;
        }

    }
}