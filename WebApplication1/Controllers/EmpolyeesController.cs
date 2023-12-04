using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmpolyeesController : Controller
    {
        private readonly ItiContext _context;

        public EmpolyeesController(ItiContext context)
        {
            _context = context;
        }


        public void addemp()
        {
        }
        // GET: Empolyees
        public async Task<IActionResult> Index()
        {
            var itiContext = _context.Empolyees.Include(e => e.DnoNavigation).Include(e => e.SupersonNavigation);
            return View(await itiContext.ToListAsync());
        }

        // GET: Empolyees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Empolyees == null)
            {
                return NotFound();
            }

            var empolyee = await _context.Empolyees
                .Include(e => e.DnoNavigation)
                .Include(e => e.SupersonNavigation)
                .FirstOrDefaultAsync(m => m.Ssn == id);
            if (empolyee == null)
            {
                return NotFound();
            }

            return View(empolyee);
        }

        // GET: Empolyees/Create
        public IActionResult Create()
        {
            ViewData["Dno"] = new SelectList(_context.Departments, "Dnum", "Dnum");
            ViewData["Superson"] = new SelectList(_context.Empolyees, "Ssn", "Ssn");
            return View();
        }

        // POST: Empolyees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empolyee empolyee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empolyee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dno"] = new SelectList(_context.Departments, "Dnum", "Dnum", empolyee.Dno);
            ViewData["Superson"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", empolyee.Superson);
            return View(empolyee);
        }

        // GET: Empolyees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Empolyees == null)
            {
                return NotFound();
            }

            var empolyee = await _context.Empolyees.FindAsync(id);
            if (empolyee == null)
            {
                return NotFound();
            }
            ViewData["Dno"] = new SelectList(_context.Departments, "Dnum", "Dnum", empolyee.Dno);
            ViewData["Superson"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", empolyee.Superson);
            return View(empolyee);
        }

        // POST: Empolyees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Fname,Lname,Ssn,Date,Address,Sex,Salary,Superson,Dno")] Empolyee empolyee)
        {
            if (id != empolyee.Ssn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empolyee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpolyeeExists(empolyee.Ssn))
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
            ViewData["Dno"] = new SelectList(_context.Departments, "Dnum", "Dnum", empolyee.Dno);
            ViewData["Superson"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", empolyee.Superson);
            return View(empolyee);
        }

        // GET: Empolyees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Empolyees == null)
            {
                return NotFound();
            }

            var empolyee = await _context.Empolyees
                .Include(e => e.DnoNavigation)
                .Include(e => e.SupersonNavigation)
                .FirstOrDefaultAsync(m => m.Ssn == id);
            if (empolyee == null)
            {
                return NotFound();
            }

            return View(empolyee);
        }

        // POST: Empolyees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Empolyees == null)
            {
                return Problem("Entity set 'ItiContext.Empolyees'  is null.");
            }
            var empolyee = await _context.Empolyees.FindAsync(id);
            if (empolyee != null)
            {
                _context.Empolyees.Remove(empolyee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpolyeeExists(string id)
        {
          return (_context.Empolyees?.Any(e => e.Ssn == id)).GetValueOrDefault();
        }
    }
}
