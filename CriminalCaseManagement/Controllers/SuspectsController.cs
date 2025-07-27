namespace CriminalCaseManagement.Controllers
{
    public class SuspectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuspectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suspects
        public async Task<IActionResult> Index()
        {
            var suspects = await _context.Suspects
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(suspects);
        }

        // GET: Suspects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suspects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IdNumber,DateOfBirth,Address,Phone,Nationality")] Suspect suspect)
        {
            if (ModelState.IsValid)
            {
                suspect.CreatedAt = DateTime.Now;
                _context.Add(suspect);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إضافة المتهم بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(suspect);
        }

        // GET: Suspects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suspect = await _context.Suspects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suspect == null)
            {
                return NotFound();
            }

            return View(suspect);
        }

        // GET: Suspects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suspect = await _context.Suspects.FindAsync(id);
            if (suspect == null)
            {
                return NotFound();
            }
            return View(suspect);
        }

        // POST: Suspects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IdNumber,DateOfBirth,Address,Phone,Nationality,Status")] Suspect suspect)
        {
            if (id != suspect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    suspect.UpdatedAt = DateTime.Now;
                    _context.Update(suspect);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تحديث بيانات المتهم بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuspectExists(suspect.Id))
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
            return View(suspect);
        }

        // GET: Suspects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suspect = await _context.Suspects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suspect == null)
            {
                return NotFound();
            }

            return View(suspect);
        }

        // POST: Suspects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suspect = await _context.Suspects.FindAsync(id);
            if (suspect != null)
            {
                _context.Suspects.Remove(suspect);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف المتهم بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SuspectExists(int id)
        {
            return _context.Suspects.Any(e => e.Id == id);
        }
    }
}
