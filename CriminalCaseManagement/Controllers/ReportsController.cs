using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string searchTerm, ReportType? typeFilter, ReportStatus? statusFilter, 
            DateTime? dateFrom, DateTime? dateTo, int pageNumber = 1)
        {
            var query = _context.Reports
                .Include(r => r.CreatedBy)
                .Include(r => r.Cases)
                .Include(r => r.Documents)
                .Include(r => r.ReportSuspects)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => r.ReporterName.Contains(searchTerm) || 
                                        r.ReporterIdNumber.Contains(searchTerm) ||
                                        r.Description.Contains(searchTerm));
            }

            if (typeFilter.HasValue)
            {
                query = query.Where(r => r.Type == typeFilter.Value);
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(r => r.Status == statusFilter.Value);
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(r => r.ReportDate >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(r => r.ReportDate <= dateTo.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination
            var pageSize = 10;
            var reports = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to view models
            var reportViewModels = reports.Select(r => new ReportViewModel
            {
                Id = r.Id,
                ReporterName = r.ReporterName,
                ReporterIdNumber = r.ReporterIdNumber,
                Type = r.Type,
                ReportDate = r.ReportDate,
                Description = r.Description,
                Location = r.Location,
                PhoneNumber = r.PhoneNumber,
                Status = r.Status,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                CreatedByFullName = r.CreatedBy?.FullName ?? "",
                CasesCount = r.Cases.Count,
                DocumentsCount = r.Documents.Count,
                SuspectsCount = r.ReportSuspects.Count
            }).ToList();

            var viewModel = new ReportListViewModel
            {
                Reports = reportViewModels,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                TypeFilter = typeFilter,
                StatusFilter = statusFilter,
                DateFrom = dateFrom,
                DateTo = dateTo
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
                .Include(r => r.Cases)
                .Include(r => r.Documents)
                .Include(r => r.ReportSuspects)
                    .ThenInclude(rs => rs.Suspect)
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
                PhoneNumber = report.PhoneNumber,
                Status = report.Status,
                Notes = report.Notes,
                CreatedAt = report.CreatedAt,
                CreatedByFullName = report.CreatedBy?.FullName ?? "",
                CasesCount = report.Cases.Count,
                DocumentsCount = report.Documents.Count,
                SuspectsCount = report.ReportSuspects.Count
            };

            return View(viewModel);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var report = new Report
                {
                    ReporterName = model.ReporterName,
                    ReporterIdNumber = model.ReporterIdNumber,
                    Type = model.Type,
                    ReportDate = model.ReportDate,
                    Description = model.Description,
                    Location = model.Location,
                    PhoneNumber = model.PhoneNumber,
                    Notes = model.Notes,
                    Status = ReportStatus.Pending,
                    CreatedById = currentUser.Id,
                    CreatedAt = DateTime.Now
                };

                _context.Add(report);
                await _context.SaveChangesAsync();

                // Handle file uploads
                if (model.Attachments != null && model.Attachments.Any())
                {
                    foreach (var file in model.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            // TODO: Implement file upload logic
                            // For now, just create a placeholder document
                            var document = new Document
                            {
                                FileName = file.FileName,
                                FileType = file.ContentType,
                                FileSize = file.Length,
                                FilePath = "", // TODO: Save file and get path
                                Type = DocumentType.Other,
                                UploadedById = currentUser.Id,
                                ReportId = report.Id,
                                UploadDate = DateTime.Now
                            };

                            _context.Add(document);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "تم إنشاء البلاغ بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
                Id = report.Id,
                ReporterName = report.ReporterName,
                ReporterIdNumber = report.ReporterIdNumber,
                Type = report.Type,
                ReportDate = report.ReportDate,
                Description = report.Description,
                Location = report.Location,
                PhoneNumber = report.PhoneNumber,
                Notes = report.Notes
            };

            return View(viewModel);
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateReportViewModel model)
        {
            if (id != model.Id)
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

                    report.ReporterName = model.ReporterName;
                    report.ReporterIdNumber = model.ReporterIdNumber;
                    report.Type = model.Type;
                    report.ReportDate = model.ReportDate;
                    report.Description = model.Description;
                    report.Location = model.Location;
                    report.PhoneNumber = model.PhoneNumber;
                    report.Notes = model.Notes;
                    report.UpdatedAt = DateTime.Now;

                    _context.Update(report);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم تحديث البلاغ بنجاح.";
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

            return View(model);
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

            var viewModel = new ReportViewModel
            {
                Id = report.Id,
                ReporterName = report.ReporterName,
                ReporterIdNumber = report.ReporterIdNumber,
                Type = report.Type,
                ReportDate = report.ReportDate,
                Description = report.Description,
                Status = report.Status,
                CreatedAt = report.CreatedAt,
                CreatedByFullName = report.CreatedBy?.FullName ?? ""
            };

            return View(viewModel);
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
                TempData["SuccessMessage"] = "تم حذف البلاغ بنجاح.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}