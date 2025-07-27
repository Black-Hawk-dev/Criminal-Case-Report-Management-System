namespace CriminalCaseManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is logged in
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new DashboardViewModel
            {
                TotalReports = await _context.Reports.CountAsync(r => r.IsActive),
                TotalCases = await _context.Cases.CountAsync(),
                TotalSuspects = await _context.Suspects.CountAsync(s => s.IsActive),
                TotalInvestigators = await _context.Users.CountAsync(u => u.Role == UserRole.Investigator && u.IsActive),
                
                UnderInvestigationCases = await _context.Cases.CountAsync(c => c.Status == CaseStatus.UnderInvestigation),
                ClosedCases = await _context.Cases.CountAsync(c => c.Status == CaseStatus.Closed),
                TransferredToProsecutionCases = await _context.Cases.CountAsync(c => c.Status == CaseStatus.TransferredToProsecution)
            };

            // Get recent activities (last 10)
            var recentReports = await _context.Reports
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.ReportDate)
                .Take(5)
                .Select(r => new RecentActivityViewModel
                {
                    Activity = $"بلاغ جديد: {r.ReporterName} - {r.Type}",
                    Date = r.ReportDate,
                    Type = "Report"
                })
                .ToListAsync();

            var recentCases = await _context.Cases
                .Include(c => c.Report)
                .OrderByDescending(c => c.CreatedDate)
                .Take(5)
                .Select(c => new RecentActivityViewModel
                {
                    Activity = $"قضية جديدة: {c.CaseNumber} - {c.Title}",
                    Date = c.CreatedDate,
                    Type = "Case"
                })
                .ToListAsync();

            viewModel.RecentActivities = recentReports.Concat(recentCases)
                .OrderByDescending(r => r.Date)
                .Take(10)
                .ToList();

            // Get cases by type
            viewModel.CasesByType = await _context.Cases
                .Include(c => c.Report)
                .GroupBy(c => c.Report.Type)
                .Select(g => new CasesByTypeViewModel
                {
                    Type = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToListAsync();

            return View(viewModel);
        }
    }
}
