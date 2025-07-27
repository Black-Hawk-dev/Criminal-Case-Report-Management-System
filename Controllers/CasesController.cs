using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CriminalCaseManagement.Data;
using CriminalCaseManagement.Models.Entities;
using CriminalCaseManagement.Models.ViewModels;

namespace CriminalCaseManagement.Controllers
{
    [Authorize]
    public class CasesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CasesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cases
        public async Task<IActionResult> Index(string? searchTerm, CaseStatus? statusFilter, 
            string? investigatorFilter, DateTime? dateFrom, DateTime? dateTo, int pageNumber = 1)
        {
            var query = _context.Cases
                .Include(c => c.Report)
                .Include(c => c.AssignedInvestigator)
                .Include(c => c.Suspects)
                .Include(c => c.Documents)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.CaseNumber.Contains(searchTerm) || 
                                        c.Title.Contains(searchTerm) ||
                                        c.Description.Contains(searchTerm));
            }

            if (statusFilter.HasValue)
            {
                query = query.Where(c => c.Status == statusFilter.Value);
            }

            if (!string.IsNullOrEmpty(investigatorFilter))
            {
                query = query.Where(c => c.AssignedInvestigator.FullName.Contains(investigatorFilter));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(c => c.OpenDate >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(c => c.OpenDate <= dateTo.Value);
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();
            var pageSize = 10;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Apply pagination
            var cases = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Convert to view models
            var caseViewModels = cases.Select(c => new CaseViewModel
            {
                Id = c.Id,
                CaseNumber = c.CaseNumber,
                Title = c.Title,
                Description = c.Description,
                OpenDate = c.OpenDate,
                CloseDate = c.CloseDate,
                Status = c.Status,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ReportTitle = c.Report?.ReporterName,
                AssignedInvestigatorName = c.AssignedInvestigator?.FullName,
                SuspectsCount = c.Suspects.Count,
                DocumentsCount = c.Documents.Count,
                UpdatesCount = c.Updates.Count
            }).ToList();

            var viewModel = new CaseListViewModel
            {
                Cases = caseViewModels,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                StatusFilter = statusFilter,
                InvestigatorFilter = investigatorFilter,
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return View(viewModel);
        }

        // GET: Cases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _context.Cases
                .Include(c => c.Report)
                .Include(c => c.AssignedInvestigator)
                .Include(c => c.Suspects)
                .Include(c => c.Documents)
                .Include(c => c.Updates.OrderByDescending(u => u.UpdateDate))
                    .ThenInclude(u => u.UpdatedBy)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (caseEntity == null)
            {
                return NotFound();
            }

            var viewModel = new CaseDetailsViewModel
            {
                Id = caseEntity.Id,
                CaseNumber = caseEntity.CaseNumber,
                Title = caseEntity.Title,
                Description = caseEntity.Description,
                OpenDate = caseEntity.OpenDate,
                CloseDate = caseEntity.CloseDate,
                Status = caseEntity.Status,
                Notes = caseEntity.Notes,
                CreatedAt = caseEntity.CreatedAt,
                UpdatedAt = caseEntity.UpdatedAt,
                ReportTitle = caseEntity.Report?.ReporterName,
                ReportId = caseEntity.ReportId,
                AssignedInvestigatorName = caseEntity.AssignedInvestigator?.FullName,
                AssignedInvestigatorId = caseEntity.AssignedInvestigatorId,
                Suspects = caseEntity.Suspects.Select(s => new SuspectViewModel
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    IdNumber = s.IdNumber,
                    DateOfBirth = s.DateOfBirth,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Nationality = s.Nationality,
                    Gender = s.Gender,
                    Status = s.Status
                }).ToList(),
                Documents = caseEntity.Documents.Select(d => new DocumentViewModel
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    FileSize = d.FileSize,
                    Description = d.Description,
                    Type = d.Type,
                    UploadDate = d.UploadDate,
                    UploadedByFullName = d.UploadedBy.FullName
                }).ToList(),
                Updates = caseEntity.Updates.Select(u => new CaseUpdateViewModel
                {
                    Id = u.Id,
                    UpdateText = u.UpdateText,
                    UpdateDate = u.UpdateDate,
                    Type = u.Type,
                    Notes = u.Notes,
                    UpdatedByFullName = u.UpdatedBy.FullName
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: Cases/Create
        public async Task<IActionResult> Create()
        {
            // Get available reports
            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending || r.Status == ReportStatus.UnderInvestigation)
                .Select(r => new { r.Id, Title = $"{r.ReporterName} - {r.Type} - {r.ReportDate:yyyy/MM/dd}" })
                .ToListAsync();

            // Get available investigators
            var investigators = await _userManager.GetUsersInRoleAsync("Investigator");

            ViewBag.Reports = new SelectList(reports, "Id", "Title");
            ViewBag.Investigators = new SelectList(investigators, "Id", "FullName");

            return View();
        }

        // POST: Cases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCaseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var caseEntity = new Case
                {
                    CaseNumber = await GenerateCaseNumber(),
                    Title = model.Title,
                    Description = model.Description,
                    OpenDate = model.OpenDate,
                    Status = model.Status,
                    Notes = model.Notes,
                    ReportId = model.ReportId,
                    AssignedInvestigatorId = model.AssignedInvestigatorId,
                    CreatedAt = DateTime.Now
                };

                _context.Add(caseEntity);
                await _context.SaveChangesAsync();

                // Update report status if assigned
                if (model.ReportId.HasValue)
                {
                    var report = await _context.Reports.FindAsync(model.ReportId.Value);
                    if (report != null)
                    {
                        report.Status = ReportStatus.Assigned;
                        _context.Update(report);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إنشاء القضية بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            // Reload view data for validation errors
            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending || r.Status == ReportStatus.UnderInvestigation)
                .Select(r => new { r.Id, Title = $"{r.ReporterName} - {r.Type} - {r.ReportDate:yyyy/MM/dd}" })
                .ToListAsync();

            var investigators = await _userManager.GetUsersInRoleAsync("Investigator");

            ViewBag.Reports = new SelectList(reports, "Id", "Title");
            ViewBag.Investigators = new SelectList(investigators, "Id", "FullName");

            return View(model);
        }

        // GET: Cases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _context.Cases.FindAsync(id);
            if (caseEntity == null)
            {
                return NotFound();
            }

            var viewModel = new CreateCaseViewModel
            {
                Id = caseEntity.Id,
                Title = caseEntity.Title,
                Description = caseEntity.Description,
                OpenDate = caseEntity.OpenDate,
                CloseDate = caseEntity.CloseDate,
                Status = caseEntity.Status,
                Notes = caseEntity.Notes,
                ReportId = caseEntity.ReportId,
                AssignedInvestigatorId = caseEntity.AssignedInvestigatorId
            };

            // Get available reports
            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending || r.Status == ReportStatus.UnderInvestigation || r.Id == caseEntity.ReportId)
                .Select(r => new { r.Id, Title = $"{r.ReporterName} - {r.Type} - {r.ReportDate:yyyy/MM/dd}" })
                .ToListAsync();

            // Get available investigators
            var investigators = await _userManager.GetUsersInRoleAsync("Investigator");

            ViewBag.Reports = new SelectList(reports, "Id", "Title");
            ViewBag.Investigators = new SelectList(investigators, "Id", "FullName");

            return View(viewModel);
        }

        // POST: Cases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCaseViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var caseEntity = await _context.Cases.FindAsync(id);
                    if (caseEntity == null)
                    {
                        return NotFound();
                    }

                    caseEntity.Title = model.Title;
                    caseEntity.Description = model.Description;
                    caseEntity.OpenDate = model.OpenDate;
                    caseEntity.CloseDate = model.CloseDate;
                    caseEntity.Status = model.Status;
                    caseEntity.Notes = model.Notes;
                    caseEntity.ReportId = model.ReportId;
                    caseEntity.AssignedInvestigatorId = model.AssignedInvestigatorId;
                    caseEntity.UpdatedAt = DateTime.Now;

                    _context.Update(caseEntity);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم تحديث القضية بنجاح.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(id))
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

            // Reload view data for validation errors
            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending || r.Status == ReportStatus.UnderInvestigation || r.Id == model.ReportId)
                .Select(r => new { r.Id, Title = $"{r.ReporterName} - {r.Type} - {r.ReportDate:yyyy/MM/dd}" })
                .ToListAsync();

            var investigators = await _userManager.GetUsersInRoleAsync("Investigator");

            ViewBag.Reports = new SelectList(reports, "Id", "Title");
            ViewBag.Investigators = new SelectList(investigators, "Id", "FullName");

            return View(model);
        }

        // GET: Cases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caseEntity = await _context.Cases
                .Include(c => c.Report)
                .Include(c => c.AssignedInvestigator)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (caseEntity == null)
            {
                return NotFound();
            }

            return View(caseEntity);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caseEntity = await _context.Cases.FindAsync(id);
            if (caseEntity != null)
            {
                _context.Cases.Remove(caseEntity);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم حذف القضية بنجاح.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Cases/AddUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdate(CaseUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }

                var update = new CaseUpdate
                {
                    CaseId = model.CaseId,
                    UpdateText = model.UpdateText,
                    Type = model.Type,
                    Notes = model.Notes,
                    UpdatedById = currentUser.Id,
                    UpdateDate = DateTime.Now
                };

                _context.Add(update);

                // Update case status if provided
                if (model.NewStatus.HasValue)
                {
                    var caseEntity = await _context.Cases.FindAsync(model.CaseId);
                    if (caseEntity != null)
                    {
                        caseEntity.Status = model.NewStatus.Value;
                        caseEntity.UpdatedAt = DateTime.Now;
                        _context.Update(caseEntity);
                    }
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "تم إضافة التحديث بنجاح.";
            }

            return RedirectToAction(nameof(Details), new { id = model.CaseId });
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }

        private async Task<string> GenerateCaseNumber()
        {
            var year = DateTime.Now.Year;
            var lastCase = await _context.Cases
                .Where(c => c.CaseNumber.StartsWith($"CASE-{year}-"))
                .OrderByDescending(c => c.CaseNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastCase != null)
            {
                var parts = lastCase.CaseNumber.Split('-');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"CASE-{year}-{nextNumber:D4}";
        }
    }
}