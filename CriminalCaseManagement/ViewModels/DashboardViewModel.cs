namespace CriminalCaseManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalReports { get; set; }
        public int TotalCases { get; set; }
        public int TotalSuspects { get; set; }
        public int TotalInvestigators { get; set; }
        
        public int UnderInvestigationCases { get; set; }
        public int ClosedCases { get; set; }
        public int TransferredToProsecutionCases { get; set; }
        
        public List<RecentActivityViewModel> RecentActivities { get; set; } = new List<RecentActivityViewModel>();
        public List<CasesByTypeViewModel> CasesByType { get; set; } = new List<CasesByTypeViewModel>();
    }

    public class RecentActivityViewModel
    {
        public string Activity { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // Report, Case, Suspect
    }

    public class CasesByTypeViewModel
    {
        public string Type { get; set; }
        public int Count { get; set; }
    }
}