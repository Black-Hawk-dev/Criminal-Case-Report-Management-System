using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriminalCaseManagement.Data;
using CriminalCaseManagement.Models;

namespace CriminalCaseManagement.Controllers
{
    public class InvestigatorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvestigatorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Investigators
        public async Task<IActionResult> Index()
        {
            var investigators = await _context.Investigators
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
            return View(investigators);
        }

        // GET: Investigators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Investigators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Rank,Department,BadgeNumber,Email,Phone")] Investigator investigator)
        {
            if (ModelState.IsValid)
            {
                investigator.CreatedAt = DateTime.Now;
                _context.Add(investigator);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة المحقق بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(investigator);
        }

        // GET: Investigators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investigator == null)
            {
                return NotFound();
            }

            return View(investigator);
        }

        // GET: Investigators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators.FindAsync(id);
            if (investigator == null)
            {
                return NotFound();
            }
            return View(investigator);
        }

        // POST: Investigators/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Rank,Department,BadgeNumber,Email,Phone,IsActive")] Investigator investigator)
        {
            if (id != investigator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investigator);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تحديث بيانات المحقق بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestigatorExists(investigator.Id))
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
            return View(investigator);
        }

        // GET: Investigators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigator = await _context.Investigators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (investigator == null)
            {
                return NotFound();
            }

            return View(investigator);
        }

        // POST: Investigators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investigator = await _context.Investigators.FindAsync(id);
            if (investigator != null)
            {
                _context.Investigators.Remove(investigator);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف المحقق بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InvestigatorExists(int id)
        {
            return _context.Investigators.Any(e => e.Id == id);
        }
    }
}