using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProjectHamburger.Data;
using MVCProjectHamburger.Models.Entities.Concrete;

namespace MVCProjectHamburger.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MenuController : Controller
    {
        private readonly HamburgerDbContext _context;

        public MenuController(HamburgerDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Menu
        public async Task<IActionResult> Index()
        {
              return _context.Menus != null ? 
                          View(await _context.Menus.ToListAsync()) :
                          Problem("Entity set 'HamburgerDbContext.Menus'  is null.");
        }

        // GET: Admin/Menu/Details/5
       

        // GET: Admin/Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CoverImage,ID")] Menu menu, IFormFile ImageName)
        {
            _context.Menus.Add(menu);
            Guid guid = Guid.NewGuid();
            string newFileName = guid.ToString() + "_" + ImageName.FileName;
            menu.CoverImage = newFileName;

            FileStream st = new FileStream("wwwroot/HamburgerCoverImages/" + newFileName, FileMode.Create);
            await ImageName.CopyToAsync(st);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,CoverImage,ID")] Menu menu,IFormFile ImgName)
        {
            if (id != menu.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    Guid guid = Guid.NewGuid();
                    string newFileName = guid.ToString() + "_" + ImgName.FileName;
                    menu.CoverImage = newFileName;

                    FileStream st = new FileStream("wwwroot/HamburgerCoverImages/" + newFileName, FileMode.Create);
                    await ImgName.CopyToAsync(st);
                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        // GET: Admin/Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.ID == id);


            foreach (MenuOrder item in _context.MenuOrders.ToList())
            {
                if (item.MenuID == id)
                {
                    _context.MenuOrders.Remove(item);
                    
                }
            }

            foreach (ShoppingCart item in _context.ShoppingCarts.ToList())
            {
                if (item.MenuOrder.MenuID == id)
                {
                    _context.ShoppingCarts.Remove(item);
                   
                }
            }

            _context.SaveChanges();

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Admin/Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'HamburgerDbContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return (_context.Menus?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
