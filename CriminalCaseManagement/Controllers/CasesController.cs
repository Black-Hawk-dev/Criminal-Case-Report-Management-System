namespace CriminalCaseManagement.Controllers
{
    public class CasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cases
        public async Task<IActionResult> Index()
        {
            var cases = await _context.Cases
                .Include(c => c.Report)
                .Include(c => c.Investigator)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(cases);
        }

        // GET: Cases/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ReportId"] = await _context.Reports.Where(r => r.Status == "Pending").ToListAsync();
            ViewData["InvestigatorId"] = await _context.Investigators.Where(i => i.IsActive).ToListAsync();
            return View();
        }

        // POST: Cases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseNumber,ReportId,InvestigatorId,Notes")] Case caseItem)
        {
            if (ModelState.IsValid)
            {
                caseItem.CreatedAt = DateTime.Now;
                _context.Add(caseItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم إنشاء القضية بنجاح";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReportId"] = await _context.Reports.Where(r => r.Status == "Pending").ToListAsync();
            ViewData["InvestigatorId"] = await _context.Investigators.Where(i => i.IsActive).ToListAsync();
            return View(caseItem);
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseItem = await _context.Cases
                .Include(c => c.Report)
                .Include(c => c.Investigator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseItem == null)
            {
                return NotFound();
            }

            return View(caseItem);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseItem = await _context.Cases.FindAsync(id);
            if (caseItem == null)
            {
                return NotFound();
            }
            ViewData["ReportId"] = await _context.Reports.ToListAsync();
            ViewData["InvestigatorId"] = await _context.Investigators.Where(i => i.IsActive).ToListAsync();
            return View(caseItem);
        }

        // POST: Cases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CaseNumber,ReportId,InvestigatorId,Status,Notes")] Case caseItem)
        {
            if (id != caseItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    caseItem.UpdatedAt = DateTime.Now;
                    if (caseItem.Status == "Closed")
                    {
                        caseItem.ClosedAt = DateTime.Now;
                    }
                    _context.Update(caseItem);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تحديث القضية بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(caseItem.Id))
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
            ViewData["ReportId"] = await _context.Reports.ToListAsync();
            ViewData["InvestigatorId"] = await _context.Investigators.Where(i => i.IsActive).ToListAsync();
            return View(caseItem);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseItem = await _context.Cases
                .Include(c => c.Report)
                .Include(c => c.Investigator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caseItem == null)
            {
                return NotFound();
            }

            return View(caseItem);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseItem = await _context.Cases.FindAsync(id);
            if (caseItem != null)
            {
                _context.Cases.Remove(caseItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف القضية بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }
    }
}
