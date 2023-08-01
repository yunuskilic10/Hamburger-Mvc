using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProjectHamburger.Data;
using MVCProjectHamburger.Models.Entities.Concrete;
using MVCProjectHamburger.Models.Enums;
using MVCProjectHamburger.Models.ViewModels;
using System.Data;

namespace MVCProjectHamburger.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly HamburgerDbContext _context;
        public OrderController(HamburgerDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> list = _context.ShoppingCarts.Include(o => o.Order).ToList();
            return View(list);
        }
        public IActionResult Edit(int id)
        {
            ShoppingCart sc = _context.ShoppingCarts.Include(o => o.Order).Include(o => o.MenuOrder).Include(o => o.ExtraIngredientOrder).FirstOrDefault(x => x.ID == id);
            if (sc != null)
            {
                ShoppingCartVM VM = new ShoppingCartVM();
                VM.Id = id;
                VM.Number = sc.Number;
                if (sc.MenuSize != null)
                {
                    VM.MenuSize = sc.MenuSize;
                }
                VM.TotalPrice = sc.TotalPrice;
                VM.MenuOrder = sc.MenuOrder;
                VM.ExtraIngredientOrder = sc.ExtraIngredientOrder;
                VM.Order = sc.Order;
                return View(VM);

            }
            TempData["info"] = "There is no User to update";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Edit(ShoppingCartVM vm)
        {
            int deger;
            ShoppingCart cart = _context.ShoppingCarts.Include(o => o.Order).Include(o => o.MenuOrder).Include(o => o.ExtraIngredientOrder).FirstOrDefault(x => x.ID == vm.Id);

            cart.Number = vm.Number;
            cart.MenuSize = vm.MenuSize;
            if (cart.MenuSize != null)
            {
                Menu menu = _context.Menus.Find(cart.MenuOrder.MenuID);
                cart.MenuSize = EnumBelirle(int.Parse(vm.MenuSize)).ToString();
                cart.TotalPrice = vm.Number * (menu.Price + int.Parse(vm.MenuSize));
            }
            else
            {
                ExtraIngredient ex = _context.ExtraIngredients.Find(cart.ExtraIngredientOrder.ExtraIngredientID);
                cart.TotalPrice = vm.Number * ex.Price;
            }
            _context.ShoppingCarts.Update(cart);
            _context.SaveChanges();


            Order od = _context.Orders.Find(cart.OrderId);
            List<ShoppingCart> carts = _context.ShoppingCarts.Where(x => x.OrderId == cart.OrderId).ToList();
            od.TotalPrice = 0;
            foreach (ShoppingCart item in carts)
            {
                od.TotalPrice += item.TotalPrice;

            }
            _context.Orders.Update(od);
            _context.SaveChanges();


            return RedirectToAction("Index");


        }

        public ActionResult Delete(int id)
        {
            ShoppingCart sc=_context.ShoppingCarts.Find(id);
            Order od = _context.Orders.Find(sc.OrderId);

            if (sc != null)
            {
                _context.ShoppingCarts.Remove(sc);
                _context.SaveChanges();

                List<ShoppingCart> carts = _context.ShoppingCarts.Where(x => x.OrderId == sc.OrderId).ToList();
                od.TotalPrice = 0;
                foreach (ShoppingCart item in carts)
                {
                    od.TotalPrice += item.TotalPrice;

                }
                _context.Orders.Update(od);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult Accountancy()
        {
            List<Order> od=_context.Orders.ToList();
            return View(od);
        }
        private Enum EnumBelirle(int sayi)
        {
            if (sayi == 1)
            {
                return MenuSize.Small;
            }
            else if (sayi == 10)
            {
                return MenuSize.Medium;
            }
            else
            {
                return MenuSize.Large;
            }
                                
        }
    }
}
