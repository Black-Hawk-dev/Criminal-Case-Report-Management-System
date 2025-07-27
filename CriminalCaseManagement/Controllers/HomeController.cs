using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriminalCaseManagement.Models;
using CriminalCaseManagement.Data;
using CriminalCaseManagement.ViewModels;

namespace CriminalCaseManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = new DashboardViewModel
        {
            TotalReports = await _context.Reports.CountAsync(),
            TotalCases = await _context.Cases.CountAsync(),
            TotalSuspects = await _context.Suspects.CountAsync(),
            TotalInvestigators = await _context.Investigators.CountAsync(),
            PendingReports = await _context.Reports.CountAsync(r => r.Status == "Pending"),
            OpenCases = await _context.Cases.CountAsync(c => c.Status == "Open"),
            ClosedCases = await _context.Cases.CountAsync(c => c.Status == "Closed"),
            RecentReports = await _context.Reports
                .OrderByDescending(r => r.ReportDate)
                .Take(5)
                .Select(r => new RecentReport
                {
                    Id = r.Id,
                    ReporterName = r.ReporterName,
                    ReportType = r.ReportType,
                    ReportDate = r.ReportDate,
                    Status = r.Status
                })
                .ToListAsync(),
            RecentCases = await _context.Cases
                .Include(c => c.Investigator)
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new RecentCase
                {
                    Id = c.Id,
                    CaseNumber = c.CaseNumber,
                    InvestigatorName = c.Investigator != null ? c.Investigator.Name : "غير محدد",
                    CreatedAt = c.CreatedAt,
                    Status = c.Status
                })
                .ToListAsync(),
            CaseStatusChart = await _context.Cases
                .GroupBy(c => c.Status)
                .Select(g => new CaseStatusChart
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync()
        };

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
