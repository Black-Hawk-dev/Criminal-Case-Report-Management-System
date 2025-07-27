using CriminalCaseManagement.Models.Entities;

namespace CriminalCaseManagement.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Statistics
        public int TotalReports { get; set; }
        public int TotalCases { get; set; }
        public int TotalSuspects { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveInvestigators { get; set; }

        // Recent activities
        public List<RecentReportViewModel> RecentReports { get; set; } = new List<RecentReportViewModel>();
        public List<RecentCaseViewModel> RecentCases { get; set; } = new List<RecentCaseViewModel>();
        public List<RecentUpdateViewModel> RecentUpdates { get; set; } = new List<RecentUpdateViewModel>();

        // Charts data
        public List<ChartDataPoint> ReportsByType { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> CasesByStatus { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> ReportsByMonth { get; set; } = new List<ChartDataPoint>();

        // Investigator workload
        public List<InvestigatorWorkloadViewModel> InvestigatorWorkload { get; set; } = new List<InvestigatorWorkloadViewModel>();
    }

    public class RecentReportViewModel
    {
        public int Id { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public ReportType Type { get; set; }
        public DateTime ReportDate { get; set; }
        public ReportStatus Status { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
    }

    public class RecentCaseViewModel
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public CaseStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? AssignedInvestigatorName { get; set; }
    }

    public class RecentUpdateViewModel
    {
        public int Id { get; set; }
        public string UpdateText { get; set; } = string.Empty;
        public UpdateType Type { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByFullName { get; set; } = string.Empty;
        public string CaseNumber { get; set; } = string.Empty;
    }

    public class ChartDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Color { get; set; } = "#007bff";
    }

    public class InvestigatorWorkloadViewModel
    {
        public string InvestigatorId { get; set; } = string.Empty;
        public string InvestigatorName { get; set; } = string.Empty;
        public int ActiveCases { get; set; }
        public int CompletedCases { get; set; }
        public int TotalCases { get; set; }
        public double CompletionRate => TotalCases > 0 ? (double)CompletedCases / TotalCases * 100 : 0;
    }

    public class StatisticsViewModel
    {
        public int TotalReports { get; set; }
        public int TotalCases { get; set; }
        public int TotalSuspects { get; set; }
        public int TotalUsers { get; set; }
        public int ReportsThisMonth { get; set; }
        public int CasesThisMonth { get; set; }
        public int ClosedCasesThisMonth { get; set; }
        public double AverageCaseResolutionTime { get; set; }
    }
}