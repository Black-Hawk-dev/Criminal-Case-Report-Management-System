using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CriminalCaseManagement.Data;
using CriminalCaseManagement.Models.Entities;
using CriminalCaseManagement.Models.ViewModels;

namespace CriminalCaseManagement.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reports
        public async Task<IActionResult> Index(string searchTerm, ReportType? filterType, ReportStatus? filterStatus, 
            DateTime? filterDateFrom, DateTime? filterDateTo, int pageNumber = 1)
        {
            var query = _context.Reports
                .Include(r => r.CreatedBy)
                .Include(r => r.RelatedCases)
                .Include(r => r.RelatedSuspects)
                .Include(r => r.Attachments)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => r.ReporterName.Contains(searchTerm) || 
                                        r.ReporterIdNumber.Contains(searchTerm) ||
                                        r.Description.Contains(searchTerm));
            }

            if (filterType.HasValue)
            {
                query = query.Where(r => r.Type == filterType.Value);
            }

            if (filterStatus.HasValue)
            {
                query = query.Where(r => r.Status == filterStatus.Value);
            }

            if (filterDateFrom.HasValue)
            {
                query = query.Where(r => r.ReportDate >= filterDateFrom.Value);
            }

            if (filterDateTo.HasValue)
            {
                query = query.Where(r => r.ReportDate <= filterDateTo.Value);
            }

            // Order by date descending
            query = query.OrderByDescending(r => r.ReportDate);

            // Pagination
            int pageSize = 10;
            int totalCount = await query.CountAsync();
            var reports = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Convert to ViewModels
            var reportViewModels = reports.Select(r => new ReportViewModel
            {
                Id = r.Id,
                ReporterName = r.ReporterName,
                ReporterIdNumber = r.ReporterIdNumber,
                Type = r.Type,
                ReportDate = r.ReportDate,
                Description = r.Description,
                Location = r.Location,
                Status = r.Status,
                CreatedByFullName = r.CreatedBy.FullName,
                CreatedAt = r.CreatedAt,
                RelatedCasesCount = r.RelatedCases.Count,
                RelatedSuspectsCount = r.RelatedSuspects.Count,
                AttachmentsCount = r.Attachments.Count
            }).ToList();

            var viewModel = new ReportListViewModel
            {
                Reports = reportViewModels,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                FilterType = filterType,
                FilterStatus = filterStatus,
                FilterDateFrom = filterDateFrom,
                FilterDateTo = filterDateTo
            };

            return View(viewModel);
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedBy)
                .Include(r => r.RelatedCases)
                .Include(r => r.RelatedSuspects)
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            var viewModel = new ReportViewModel
            {
                Id = report.Id,
                ReporterName = report.ReporterName,
                ReporterIdNumber = report.ReporterIdNumber,
                Type = report.Type,
                ReportDate = report.ReportDate,
                Description = report.Description,
                Location = report.Location,
                Status = report.Status,
                CreatedByFullName = report.CreatedBy.FullName,
                CreatedAt = report.CreatedAt,
                RelatedCasesCount = report.RelatedCases.Count,
                RelatedSuspectsCount = report.RelatedSuspects.Count,
                AttachmentsCount = report.Attachments.Count
            };

            return View(viewModel);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View(new CreateReportViewModel());
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                
                var report = new Report
                {
                    ReporterName = viewModel.ReporterName,
                    ReporterIdNumber = viewModel.ReporterIdNumber,
                    Type = viewModel.Type,
                    ReportDate = viewModel.ReportDate,
                    Description = viewModel.Description,
                    Location = viewModel.Location,
                    Status = ReportStatus.New,
                    CreatedById = currentUser.Id,
                    CreatedAt = DateTime.Now
                };

                _context.Add(report);
                await _context.SaveChangesAsync();

                // Handle file uploads if any
                if (viewModel.Attachments != null && viewModel.Attachments.Any())
                {
                    foreach (var file in viewModel.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            // Save file logic here
                            // This is a simplified version - you'll need to implement file storage
                            var document = new Document
                            {
                                FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                                OriginalFileName = file.FileName,
                                ContentType = file.ContentType,
                                FileSize = file.Length,
                                FilePath = "/uploads/" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                                Type = DocumentType.Other,
                                UploadedById = currentUser.Id,
                                ReportId = report.Id
                            };

                            _context.Add(document);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            var viewModel = new CreateReportViewModel
            {
                ReporterName = report.ReporterName,
                ReporterIdNumber = report.ReporterIdNumber,
                Type = report.Type,
                ReportDate = report.ReportDate,
                Description = report.Description,
                Location = report.Location
            };

            return View(viewModel);
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateReportViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var report = await _context.Reports.FindAsync(id);
                    if (report == null)
                    {
                        return NotFound();
                    }

                    report.ReporterName = viewModel.ReporterName;
                    report.ReporterIdNumber = viewModel.ReporterIdNumber;
                    report.Type = viewModel.Type;
                    report.ReportDate = viewModel.ReportDate;
                    report.Description = viewModel.Description;
                    report.Location = viewModel.Location;

                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(id))
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
            return View(viewModel);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedBy)
                .FirstOrDefaultAsync(r => r.Id == id);

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
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}