namespace CriminalCaseManagement.Models.ViewModels
{
    public class DashboardViewModel
    {
        public StatisticsViewModel Statistics { get; set; } = new StatisticsViewModel();
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new List<RecentActivityViewModel>();
        public ChartDataViewModel ChartData { get; set; } = new ChartDataViewModel();
        public List<InvestigatorWorkloadViewModel> InvestigatorWorkload { get; set; } = new List<InvestigatorWorkloadViewModel>();
    }

    public class StatisticsViewModel
    {
        public int TotalReports { get; set; }
        public int TotalCases { get; set; }
        public int TotalSuspects { get; set; }
        public int TotalInvestigators { get; set; }
        public int PendingReports { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public int ActiveSuspects { get; set; }
    }

    public class RecentActivityViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public ActivityType Type { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class ChartDataViewModel
    {
        public List<ChartItemViewModel> ReportsByType { get; set; } = new List<ChartItemViewModel>();
        public List<ChartItemViewModel> CasesByStatus { get; set; } = new List<ChartItemViewModel>();
        public List<ChartItemViewModel> ReportsByMonth { get; set; } = new List<ChartItemViewModel>();
        public List<ChartItemViewModel> SuspectsByStatus { get; set; } = new List<ChartItemViewModel>();
    }

    public class ChartItemViewModel
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    public class InvestigatorWorkloadViewModel
    {
        public string InvestigatorName { get; set; } = string.Empty;
        public int AssignedCases { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public double CompletionRate { get; set; }
    }

    public enum ActivityType
    {
        ReportCreated,
        CaseOpened,
        CaseUpdated,
        SuspectAdded,
        DocumentUploaded,
        CaseClosed
    }
}
