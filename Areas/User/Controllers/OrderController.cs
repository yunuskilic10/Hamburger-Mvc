using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProjectHamburger.Data;
using MVCProjectHamburger.Models.Entities.Concrete;
using MVCProjectHamburger.Models.Enums;
using MVCProjectHamburger.Models.ViewModels;

namespace MVCProjectHamburger.Areas.User.Controllers
{
    [Area("User")]

    public class OrderController : Controller
    {
        private readonly HamburgerDbContext _context;
        private readonly UserManager<AppUser> userManager;

        public OrderController(HamburgerDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this.userManager = userManager;

        }

        // GET: User/Order
        public async Task<IActionResult> Index()
        {
            var hamburgerDbContext = _context.Orders.Include(o => o.AppUser).Include(o => o.OrderMenus).Include(o => o.OrderExtraIngredients);
            return View(await hamburgerDbContext.ToListAsync());
        }

        public IActionResult GetMenu()
        {
            List<Menu> menu = _context.Menus.ToList();
            return PartialView("_GetMenuPartial", menu);
        }

        public IActionResult GetExtraIngredient()
        {
            List<ExtraIngredient> extraIngredients = _context.ExtraIngredients.ToList();
            return PartialView("_GetExtraIngredientPartial", extraIngredients);
        }


        [HttpPost]
        public IActionResult AddToOrder(int menuID, int number, int menuSize)
        {
            if (menuSize == 0)
            {
                var sortedOrders = _context.Orders.OrderBy(o => o.ID);
                Order od = sortedOrders.LastOrDefault();
                _context.ExtraIngredientOrders.Add(new ExtraIngredientOrder { AppUserID = GetUserID(), ExtraIngredientID = menuID, Number = number, OrderID = od.ID });
                _context.SaveChanges();

                var lastExtraOrder=_context.ExtraIngredientOrders.OrderBy(o => o.OrderID).LastOrDefault();

                ExtraIngredient extra = _context.ExtraIngredients.Find(menuID);
                od.TotalPrice += extra.Price * number;
                od.AppUserID= GetUserID();
                _context.Orders.Update(od);
                _context.SaveChanges();

                ShoppingCart cart = new ShoppingCart();
                cart.OrderId = od.ID;
                cart.Name=extra.Name;
                cart.TotalPrice = extra.Price * number;
                cart.Number=number;
                cart.ExtraIngredientOrderId = lastExtraOrder.ID;
                _context.ShoppingCarts.Add(cart);
                _context.SaveChanges();
                return RedirectToAction("ShoopingCart", new { id = od.ID });

            }
            else
            {

                var sortedOrders = _context.Orders.OrderBy(o => o.ID);
                Order od = sortedOrders.LastOrDefault();

                _context.MenuOrders.Add(new MenuOrder { AppUserID = GetUserID(), MenuID = menuID, Number = number, MenuSizes = (MenuSize)EnumBelirle(menuSize), OrderID = od.ID });
                _context.SaveChanges();

                var lastMenuOrder = _context.MenuOrders.OrderBy(o => o.ID).LastOrDefault();

                Menu menu = _context.Menus.Find(menuID);
                od.TotalPrice += (menu.Price + menuSize) * number;
                od.AppUserID = GetUserID();
                _context.Orders.Update(od);
                _context.SaveChanges();

                ShoppingCart cart = new ShoppingCart();
                cart.OrderId = od.ID;
                cart.Name = menu.Name;
                cart.TotalPrice = (menu.Price + menuSize) * number;
                cart.Number = number;
                cart.MenuSize = EnumBelirle(menuSize).ToString();
                cart.MenuOrderId = lastMenuOrder.ID;
                _context.ShoppingCarts.Add(cart);
                _context.SaveChanges();

                return RedirectToAction("ShoopingCart", new { id = od.ID });
            }

        }
        public IActionResult ShoopingCart(string? idd)
        {

            List<Order> orders = _context.Orders.OrderBy(x=>x.ID).ToList();
            int lastId = orders.Last().ID;

            List<ShoppingCart> orderList = _context.ShoppingCarts.Where(x => x.OrderId == lastId).ToList();
            return View(orderList);

        }
        [HttpPost]
        public IActionResult ShoopingCart()
        {
            Order od = new Order();
            od.TotalPrice = 0;
            _context.Orders.Add(od);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult NewOrder()
        {
            Order od = new Order();
            od.TotalPrice = 0;
            _context.Orders.Add(od);
            _context.SaveChanges();
            return NoContent();
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


            return RedirectToAction("ShoopingCart", new { id = cart.OrderId });


        }

        public ActionResult Delete(int id)
        {
            ShoppingCart sc = _context.ShoppingCarts.Find(id);
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
                return RedirectToAction("ShoopingCart", new { id = sc.OrderId });
            }
            else
            {
                return RedirectToAction("ShoopingCart", new { id = sc.OrderId });
            }
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

        private int GetUserID()
        {
            return int.Parse(userManager.GetUserId(User));
        }
        


    }
}
