namespace CriminalCaseManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalReports { get; set; }
        public int TotalCases { get; set; }
        public int TotalSuspects { get; set; }
        public int TotalInvestigators { get; set; }
        public int PendingReports { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public List<RecentReport> RecentReports { get; set; } = new List<RecentReport>();
        public List<RecentCase> RecentCases { get; set; } = new List<RecentCase>();
        public List<CaseStatusChart> CaseStatusChart { get; set; } = new List<CaseStatusChart>();
    }

    public class RecentReport
    {
        public int Id { get; set; }
        public string ReporterName { get; set; }
        public string ReportType { get; set; }
        public DateTime ReportDate { get; set; }
        public string Status { get; set; }
    }

    public class RecentCase
    {
        public int Id { get; set; }
        public string CaseNumber { get; set; }
        public string InvestigatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }

    public class CaseStatusChart
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}