namespace CriminalCaseManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index(string searchString, ReportType? reportType, DateTime? fromDate, DateTime? toDate)
        {
            // Check if user is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var reports = _context.Reports.Include(r => r.CreatedBy).Where(r => r.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                reports = reports.Where(r => r.ReporterName.Contains(searchString) || 
                                           r.ReporterIdNumber.Contains(searchString) ||
                                           r.Description.Contains(searchString));
            }

            if (reportType.HasValue)
            {
                reports = reports.Where(r => r.Type == reportType.Value);
            }

            if (fromDate.HasValue)
            {
                reports = reports.Where(r => r.ReportDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                reports = reports.Where(r => r.ReportDate <= toDate.Value);
            }

            ViewBag.SearchString = searchString;
            ViewBag.ReportType = reportType;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(await reports.OrderByDescending(r => r.ReportDate).ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedBy)
                .Include(r => r.Attachments)
                .Include(r => r.Cases)
                .Include(r => r.SuspectReports)
                    .ThenInclude(sr => sr.Suspect)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReporterName,ReporterIdNumber,Type,Description,Location")] Report report)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                report.CreatedByUserId = HttpContext.Session.GetInt32("UserId").Value;
                report.ReportDate = DateTime.Now;
                report.IsActive = true;

                _context.Add(report);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم إضافة البلاغ بنجاح";
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReporterName,ReporterIdNumber,Type,Description,Location,ReportDate,CreatedByUserId")] Report report)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    report.IsActive = true;
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "تم تحديث البلاغ بنجاح";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                // Soft delete
                report.IsActive = false;
                _context.Update(report);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم حذف البلاغ بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
