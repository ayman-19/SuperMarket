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
    public class WorkOnsController : Controller
    {
        private readonly ItiContext _context;

        public WorkOnsController(ItiContext context)
        {
            _context = context;
        }

        // GET: WorkOns
        public async Task<IActionResult> Index()
        {
            var itiContext = await _context.WorkOns.Include(w => w.EssnNavigation).Include(w => w.PnoNavigation).ToListAsync();
            var emps = new List<Empolyee>();
            foreach (var item in itiContext)
                emps.Add(item.EssnNavigation);
            ViewData["emp"] = emps;
            return View( itiContext);
        }

        // GET: WorkOns/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.WorkOns == null)
            {
                return NotFound();
            }

            var workOn = await _context.WorkOns
                .Include(w => w.EssnNavigation)
                .Include(w => w.PnoNavigation)
                .FirstOrDefaultAsync(m => m.Essn == id);
            if (workOn == null)
            {
                return NotFound();
            }

            return View(workOn);
        }

        // GET: WorkOns/Create
        public IActionResult Create()
        {
            ViewData["Essn"] = new SelectList(_context.Empolyees, "Ssn", "Ssn");
            ViewData["Pno"] = new SelectList(_context.Projects, "Pnumber", "Pnumber");
            return View();
        }

        // POST: WorkOns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkOn workOn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workOn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Essn"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", workOn.Essn);
            ViewData["Pno"] = new SelectList(_context.Projects, "Pnumber", "Pnumber", workOn.Pno);
            return View(workOn);
        }

        // GET: WorkOns/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.WorkOns == null)
            {
                return NotFound();
            }

            var workOn = await _context.WorkOns.FindAsync(id);
            if (workOn == null)
            {
                return NotFound();
            }
            ViewData["Essn"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", workOn.Essn);
            ViewData["Pno"] = new SelectList(_context.Projects, "Pnumber", "Pnumber", workOn.Pno);
            return View(workOn);
        }

        // POST: WorkOns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Essn,Pno,Hour")] WorkOn workOn)
        {
            if (id != workOn.Essn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOnExists(workOn.Essn))
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
            ViewData["Essn"] = new SelectList(_context.Empolyees, "Ssn", "Ssn", workOn.Essn);
            ViewData["Pno"] = new SelectList(_context.Projects, "Pnumber", "Pnumber", workOn.Pno);
            return View(workOn);
        }

        // GET: WorkOns/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.WorkOns == null)
            {
                return NotFound();
            }

            var workOn = await _context.WorkOns
                .Include(w => w.EssnNavigation)
                .Include(w => w.PnoNavigation)
                .FirstOrDefaultAsync(m => m.Essn == id);
            if (workOn == null)
            {
                return NotFound();
            }

            return View(workOn);
        }

        // POST: WorkOns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.WorkOns == null)
            {
                return Problem("Entity set 'ItiContext.WorkOns'  is null.");
            }
            var workOn = await _context.WorkOns.FindAsync(id);
            if (workOn != null)
            {
                _context.WorkOns.Remove(workOn);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOnExists(string id)
        {
          return (_context.WorkOns?.Any(e => e.Essn == id)).GetValueOrDefault();
        }
    }
}
