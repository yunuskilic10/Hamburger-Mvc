using System;
using System.Collections.Generic;
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
    public class ExtraIngredientController : Controller
    {
        private readonly HamburgerDbContext _context;

        public ExtraIngredientController(HamburgerDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ExtraIngredient
        public async Task<IActionResult> Index()
        {
              return _context.ExtraIngredients != null ? 
                          View(await _context.ExtraIngredients.ToListAsync()) :
                          Problem("Entity set 'HamburgerDbContext.ExtraIngredients'  is null.");
        }

        // GET: Admin/ExtraIngredient/Details/5
       

        // GET: Admin/ExtraIngredient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExtraIngredient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CoverImage,ID")] ExtraIngredient extraIngredient, IFormFile ImgName)
        {
            _context.ExtraIngredients.Add(extraIngredient);
            Guid guid = Guid.NewGuid();
            string newFileName = guid.ToString() + "_" + ImgName.FileName;
            extraIngredient.CoverImage = newFileName;

            FileStream fs = new FileStream("wwwroot/ExtraIngredientCoverImages/" + newFileName, FileMode.Create);
            await ImgName.CopyToAsync(fs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/ExtraIngredient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExtraIngredients == null)
            {
                return NotFound();
            }

            var extraIngredient = await _context.ExtraIngredients.FindAsync(id);
            if (extraIngredient == null)
            {
                return NotFound();
            }
            return View(extraIngredient);
        }

        // POST: Admin/ExtraIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,CoverImage,ID")] ExtraIngredient extraIngredient, IFormFile ImgName)
        {
            if (id != extraIngredient.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extraIngredient);
                    Guid guid = Guid.NewGuid();
                    string newFileName = guid.ToString() + "_" + ImgName.FileName;
                    extraIngredient.CoverImage = newFileName;

                    FileStream st = new FileStream("wwwroot/ExtraIngredientCoverImages/" + newFileName, FileMode.Create);
                    await ImgName.CopyToAsync(st);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtraIngredientExists(extraIngredient.ID))
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
            return View(extraIngredient);
        }

        // GET: Admin/ExtraIngredient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExtraIngredients == null)
            {
                return NotFound();
            }

            var extraIngredient = await _context.ExtraIngredients
                .FirstOrDefaultAsync(m => m.ID == id);


            foreach (ExtraIngredientOrder item in _context.ExtraIngredientOrders.ToList())
            {
                if (item.ExtraIngredientID == id)
                {
                    _context.ExtraIngredientOrders.Remove(item);
                }
            }


            foreach (ShoppingCart item in _context.ShoppingCarts.ToList())
            {
                if (item.ExtraIngredientOrderId == id)
                {
                    _context.ShoppingCarts.Remove(item);

                }
            }

            _context.SaveChanges();


            if (extraIngredient == null)
            {
                return NotFound();
            }

            return View(extraIngredient);
        }

        // POST: Admin/ExtraIngredient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExtraIngredients == null)
            {
                return Problem("Entity set 'HamburgerDbContext.ExtraIngredients'  is null.");
            }
            var extraIngredient = await _context.ExtraIngredients.FindAsync(id);
            if (extraIngredient != null)
            {
                _context.ExtraIngredients.Remove(extraIngredient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtraIngredientExists(int id)
        {
          return (_context.ExtraIngredients?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
