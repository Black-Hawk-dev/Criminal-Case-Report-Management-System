using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CriminalCaseManagement.Data;
using CriminalCaseManagement.Models;
using CriminalCaseManagement.ViewModels;

namespace CriminalCaseManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Reports
        public async Task<IActionResult> Index(string searchTerm, string statusFilter, string typeFilter, DateTime? dateFrom, DateTime? dateTo, int page = 1)
        {
            var query = _context.Reports.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => r.ReporterName.Contains(searchTerm) || r.ReporterIdNumber.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(r => r.Status == statusFilter);
            }

            if (!string.IsNullOrEmpty(typeFilter))
            {
                query = query.Where(r => r.ReportType == typeFilter);
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(r => r.ReportDate >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(r => r.ReportDate <= dateTo.Value);
            }

            // Pagination
            int pageSize = 10;
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var reports = await query
                .OrderByDescending(r => r.ReportDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ReportListViewModel
            {
                Reports = reports.Select(r => new ReportViewModel
                {
                    Id = r.Id,
                    ReporterName = r.ReporterName,
                    ReporterIdNumber = r.ReporterIdNumber,
                    ReportType = r.ReportType,
                    ReportDate = r.ReportDate,
                    Description = r.Description,
                    Location = r.Location,
                    Status = r.Status
                }).ToList(),
                SearchTerm = searchTerm,
                StatusFilter = statusFilter,
                TypeFilter = typeFilter,
                DateFrom = dateFrom,
                DateTo = dateTo,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            var viewModel = new ReportViewModel
            {
                AvailableReportTypes = new List<string>
                {
                    "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى"
                }
            };
            return View(viewModel);
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var report = new Report
                {
                    ReporterName = viewModel.ReporterName,
                    ReporterIdNumber = viewModel.ReporterIdNumber,
                    ReportType = viewModel.ReportType,
                    ReportDate = viewModel.ReportDate,
                    Description = viewModel.Description,
                    Location = viewModel.Location,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                // Handle file uploads
                if (viewModel.Attachments != null && viewModel.Attachments.Count > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "reports");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePaths = new List<string>();
                    foreach (var file in viewModel.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            filePaths.Add(fileName);
                        }
                    }

                    report.Attachments = string.Join(",", filePaths);
                }

                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableReportTypes = new List<string>
            {
                "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى"
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
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

            var viewModel = new ReportViewModel
            {
                Id = report.Id,
                ReporterName = report.ReporterName,
                ReporterIdNumber = report.ReporterIdNumber,
                ReportType = report.ReportType,
                ReportDate = report.ReportDate,
                Description = report.Description,
                Location = report.Location,
                Status = report.Status,
                AvailableReportTypes = new List<string>
                {
                    "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى"
                }
            };

            return View(viewModel);
        }

        // POST: Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReportViewModel viewModel)
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
                    report.ReportType = viewModel.ReportType;
                    report.ReportDate = viewModel.ReportDate;
                    report.Description = viewModel.Description;
                    report.Location = viewModel.Location;
                    report.Status = viewModel.Status;
                    report.UpdatedAt = DateTime.Now;

                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(viewModel.Id))
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

            viewModel.AvailableReportTypes = new List<string>
            {
                "سرقة", "اعتداء", "احتيال", "تهديد", "إتلاف ممتلكات", "مخدرات", "أخرى"
            };
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
                .FirstOrDefaultAsync(m => m.Id == id);
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