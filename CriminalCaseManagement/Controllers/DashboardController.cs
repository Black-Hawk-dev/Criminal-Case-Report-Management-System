
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

            // Get statistics
            viewModel.Statistics = new StatisticsViewModel
            {
                TotalReports = await _context.Reports.CountAsync(),
                TotalCases = await _context.Cases.CountAsync(),
                TotalSuspects = await _context.Suspects.CountAsync(),
                TotalInvestigators = await _context.Users.CountAsync(u => u.Role == UserRole.Investigator),
                PendingReports = await _context.Reports.CountAsync(r => r.Status == ReportStatus.Pending),
                OpenCases = await _context.Cases.CountAsync(c => c.Status == CaseStatus.Open),
                ClosedCases = await _context.Cases.CountAsync(c => c.Status == CaseStatus.Closed),
                ActiveSuspects = await _context.Suspects.CountAsync(s => s.Status == SuspectStatus.Active)
            };

            // Get recent activities
            var recentReports = await _context.Reports
                .Include(r => r.CreatedBy)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync();

            var recentCases = await _context.Cases
                .Include(c => c.AssignedInvestigator)
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToListAsync();

            var recentUpdates = await _context.CaseUpdates
                .Include(cu => cu.UpdatedBy)
                .Include(cu => cu.Case)
                .OrderByDescending(cu => cu.UpdateDate)
                .Take(5)
                .ToListAsync();

            viewModel.RecentActivities = new List<RecentActivityViewModel>();

            foreach (var report in recentReports)
            {
                viewModel.RecentActivities.Add(new RecentActivityViewModel
                {
                    Id = report.Id,
                    Title = $"بلاغ جديد: {report.ReporterName}",
                    Description = report.Description,
                    Date = report.CreatedAt,
                    Type = ActivityType.ReportCreated,
                    UserName = report.CreatedBy?.FullName ?? ""
                });
            }

            foreach (var caseItem in recentCases)
            {
                viewModel.RecentActivities.Add(new RecentActivityViewModel
                {
                    Id = caseItem.Id,
                    Title = $"قضية جديدة: {caseItem.Title}",
                    Description = caseItem.Description,
                    Date = caseItem.CreatedAt,
                    Type = ActivityType.CaseOpened,
                    UserName = caseItem.AssignedInvestigator?.FullName ?? ""
                });
            }

            foreach (var update in recentUpdates)
            {
                viewModel.RecentActivities.Add(new RecentActivityViewModel
                {
                    Id = update.Id,
                    Title = $"تحديث قضية: {update.Case?.Title}",
                    Description = update.UpdateText,
                    Date = update.UpdateDate,
                    Type = ActivityType.CaseUpdated,
                    UserName = update.UpdatedBy?.FullName ?? ""
                });
            }

            viewModel.RecentActivities = viewModel.RecentActivities
                .OrderByDescending(ra => ra.Date)
                .Take(10)
                .ToList();

            // Get chart data
            viewModel.ChartData = new ChartDataViewModel
            {
                ReportsByType = await GetReportsByType(),
                CasesByStatus = await GetCasesByStatus(),
                ReportsByMonth = await GetReportsByMonth(),
                SuspectsByStatus = await GetSuspectsByStatus()
            };

            // Get investigator workload
            viewModel.InvestigatorWorkload = await GetInvestigatorWorkload();

            return View(viewModel);
        }

        private async Task<List<ChartItemViewModel>> GetReportsByType()
        {
            var reportsByType = await _context.Reports
                .GroupBy(r => r.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();

            var colors = new[] { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF", "#FF9F40", "#FF6384" };

            return reportsByType.Select((item, index) => new ChartItemViewModel
            {
                Label = GetReportTypeName(item.Type),
                Value = item.Count,
                Color = colors[index % colors.Length]
            }).ToList();
        }

        private async Task<List<ChartItemViewModel>> GetCasesByStatus()
        {
            var casesByStatus = await _context.Cases
                .GroupBy(c => c.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var colors = new[] { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF", "#FF9F40" };

            return casesByStatus.Select((item, index) => new ChartItemViewModel
            {
                Label = GetCaseStatusName(item.Status),
                Value = item.Count,
                Color = colors[index % colors.Length]
            }).ToList();
        }

        private async Task<List<ChartItemViewModel>> GetReportsByMonth()
        {
            var currentYear = DateTime.Now.Year;
            var reportsByMonth = await _context.Reports
                .Where(r => r.CreatedAt.Year == currentYear)
                .GroupBy(r => r.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var monthNames = new[] { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", 
                                   "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

            return reportsByMonth.Select(item => new ChartItemViewModel
            {
                Label = monthNames[item.Month - 1],
                Value = item.Count,
                Color = "#36A2EB"
            }).ToList();
        }

        private async Task<List<ChartItemViewModel>> GetSuspectsByStatus()
        {
            var suspectsByStatus = await _context.Suspects
                .GroupBy(s => s.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var colors = new[] { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF" };

            return suspectsByStatus.Select((item, index) => new ChartItemViewModel
            {
                Label = GetSuspectStatusName(item.Status),
                Value = item.Count,
                Color = colors[index % colors.Length]
            }).ToList();
        }

        private async Task<List<InvestigatorWorkloadViewModel>> GetInvestigatorWorkload()
        {
            var investigators = await _context.Users
                .Where(u => u.Role == UserRole.Investigator)
                .Include(u => u.AssignedCases)
                .ToListAsync();

            return investigators.Select(investigator => new InvestigatorWorkloadViewModel
            {
                InvestigatorName = investigator.FullName,
                AssignedCases = investigator.AssignedCases.Count,
                OpenCases = investigator.AssignedCases.Count(c => c.Status == CaseStatus.Open),
                ClosedCases = investigator.AssignedCases.Count(c => c.Status == CaseStatus.Closed),
                CompletionRate = investigator.AssignedCases.Any() 
                    ? (double)investigator.AssignedCases.Count(c => c.Status == CaseStatus.Closed) / investigator.AssignedCases.Count * 100 
                    : 0
            }).ToList();
        }

        private string GetReportTypeName(ReportType type)
        {
            return type switch
            {
                ReportType.Theft => "سرقة",
                ReportType.Assault => "اعتداء",
                ReportType.Fraud => "احتيال",
                ReportType.DrugTrafficking => "تهريب مخدرات",
                ReportType.CyberCrime => "جرائم إلكترونية",
                ReportType.DomesticViolence => "عنف أسري",
                ReportType.Other => "أخرى",
                _ => type.ToString()
            };
        }

        private string GetCaseStatusName(CaseStatus status)
        {
            return status switch
            {
                CaseStatus.Open => "مفتوح",
                CaseStatus.UnderInvestigation => "قيد التحقيق",
                CaseStatus.Pending => "في الانتظار",
                CaseStatus.Closed => "مغلق",
                CaseStatus.TransferredToProsecution => "محول للنيابة",
                CaseStatus.Dismissed => "مرفوض",
                _ => status.ToString()
            };
        }

        private string GetSuspectStatusName(SuspectStatus status)
        {
            return status switch
            {
                SuspectStatus.Active => "نشط",
                SuspectStatus.Arrested => "مقبوض عليه",
                SuspectStatus.Released => "مطلق سراحه",
                SuspectStatus.Wanted => "مطلوب",
                SuspectStatus.Deceased => "متوفى",
                _ => status.ToString()
            };
        }
    }
}
