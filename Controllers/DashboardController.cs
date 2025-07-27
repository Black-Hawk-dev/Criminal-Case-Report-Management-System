
namespace CriminalCaseManagement.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel();

            // Get basic statistics
            viewModel.TotalReports = await _context.Reports.CountAsync();
            viewModel.TotalCases = await _context.Cases.CountAsync();
            viewModel.TotalSuspects = await _context.Suspects.CountAsync();
            viewModel.TotalUsers = await _context.Users.CountAsync();
            viewModel.ActiveInvestigators = await _context.Users
                .Where(u => u.Role == UserRole.Investigator && u.IsActive)
                .CountAsync();

            // Get recent reports
            viewModel.RecentReports = await _context.Reports
                .Include(r => r.CreatedBy)
                .OrderByDescending(r => r.ReportDate)
                .Take(5)
                .Select(r => new RecentReportViewModel
                {
                    Id = r.Id,
                    ReporterName = r.ReporterName,
                    Type = r.Type,
                    ReportDate = r.ReportDate,
                    Status = r.Status,
                    CreatedByFullName = r.CreatedBy.FullName
                })
                .ToListAsync();

            // Get recent cases
            viewModel.RecentCases = await _context.Cases
                .Include(c => c.AssignedInvestigator)
                .OrderByDescending(c => c.CreatedDate)
                .Take(5)
                .Select(c => new RecentCaseViewModel
                {
                    Id = c.Id,
                    CaseNumber = c.CaseNumber,
                    Title = c.Title,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    AssignedInvestigatorName = c.AssignedInvestigator != null ? c.AssignedInvestigator.FullName : null
                })
                .ToListAsync();

            // Get recent updates
            viewModel.RecentUpdates = await _context.CaseUpdates
                .Include(cu => cu.UpdatedBy)
                .Include(cu => cu.Case)
                .OrderByDescending(cu => cu.UpdateDate)
                .Take(10)
                .Select(cu => new RecentUpdateViewModel
                {
                    Id = cu.Id,
                    UpdateText = cu.UpdateText,
                    Type = cu.Type,
                    UpdateDate = cu.UpdateDate,
                    UpdatedByFullName = cu.UpdatedBy.FullName,
                    CaseNumber = cu.Case.CaseNumber
                })
                .ToListAsync();

            // Get reports by type
            viewModel.ReportsByType = await _context.Reports
                .GroupBy(r => r.Type)
                .Select(g => new ChartDataPoint
                {
                    Label = GetReportTypeName(g.Key),
                    Value = g.Count(),
                    Color = GetReportTypeColor(g.Key)
                })
                .ToListAsync();

            // Get cases by status
            viewModel.CasesByStatus = await _context.Cases
                .GroupBy(c => c.Status)
                .Select(g => new ChartDataPoint
                {
                    Label = GetCaseStatusName(g.Key),
                    Value = g.Count(),
                    Color = GetCaseStatusColor(g.Key)
                })
                .ToListAsync();

            // Get reports by month (last 6 months)
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            viewModel.ReportsByMonth = await _context.Reports
                .Where(r => r.ReportDate >= sixMonthsAgo)
                .GroupBy(r => new { r.ReportDate.Year, r.ReportDate.Month })
                .Select(g => new ChartDataPoint
                {
                    Label = $"{g.Key.Year}/{g.Key.Month:00}",
                    Value = g.Count(),
                    Color = "#28a745"
                })
                .OrderBy(c => c.Label)
                .ToListAsync();

            // Get investigator workload
            viewModel.InvestigatorWorkload = await _context.Users
                .Where(u => u.Role == UserRole.Investigator && u.IsActive)
                .Select(u => new InvestigatorWorkloadViewModel
                {
                    InvestigatorId = u.Id,
                    InvestigatorName = u.FullName,
                    ActiveCases = u.AssignedCases.Count(c => c.Status == CaseStatus.Open || c.Status == CaseStatus.UnderInvestigation),
                    CompletedCases = u.AssignedCases.Count(c => c.Status == CaseStatus.Closed),
                    TotalCases = u.AssignedCases.Count
                })
                .ToListAsync();

            return View(viewModel);
        }

        private string GetReportTypeName(ReportType type)
        {
            return type switch
            {
                ReportType.Theft => "سرقة",
                ReportType.Assault => "اعتداء",
                ReportType.Fraud => "احتيال",
                ReportType.DrugRelated => "مخدرات",
                ReportType.TrafficViolation => "مرور",
                ReportType.Other => "أخرى",
                _ => type.ToString()
            };
        }

        private string GetReportTypeColor(ReportType type)
        {
            return type switch
            {
                ReportType.Theft => "#dc3545",
                ReportType.Assault => "#fd7e14",
                ReportType.Fraud => "#ffc107",
                ReportType.DrugRelated => "#6f42c1",
                ReportType.TrafficViolation => "#17a2b8",
                ReportType.Other => "#6c757d",
                _ => "#007bff"
            };
        }

        private string GetCaseStatusName(CaseStatus status)
        {
            return status switch
            {
                CaseStatus.Open => "مفتوح",
                CaseStatus.UnderInvestigation => "قيد التحقيق",
                CaseStatus.Closed => "مغلق",
                CaseStatus.TransferredToProsecution => "محول للنيابة",
                CaseStatus.PendingEvidence => "في انتظار الأدلة",
                CaseStatus.AwaitingCourtDecision => "في انتظار قرار المحكمة",
                _ => status.ToString()
            };
        }

        private string GetCaseStatusColor(CaseStatus status)
        {
            return status switch
            {
                CaseStatus.Open => "#007bff",
                CaseStatus.UnderInvestigation => "#ffc107",
                CaseStatus.Closed => "#28a745",
                CaseStatus.TransferredToProsecution => "#6f42c1",
                CaseStatus.PendingEvidence => "#fd7e14",
                CaseStatus.AwaitingCourtDecision => "#17a2b8",
                _ => "#6c757d"
            };
        }
    }
}
