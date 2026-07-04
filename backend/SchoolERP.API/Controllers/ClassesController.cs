using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Domain.Entities;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public ClassesController(SchoolERPDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var classes = await _context.Classes
            .Include(c => c.Sections).ThenInclude(s => s.Students)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var dtos = classes.Select(c => new ClassDto
        {
            Id = c.Id,
            Name = c.Name,
            AcademicYear = c.AcademicYear,
            StudentCount = c.Sections.Sum(s => s.Students.Count),
            Sections = c.Sections.Select(s => new SectionDto
            {
                Id = s.Id, Name = s.Name, ClassId = c.Id,
                MaxStudents = s.MaxStudents,
                CurrentStudents = s.Students.Count
            }).ToList()
        });

        return Ok(ApiResponse<IEnumerable<ClassDto>>.Ok(dtos));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateClassDto dto)
    {
        var cls = new Class { Name = dto.Name, AcademicYear = dto.AcademicYear };
        await _context.Classes.AddAsync(cls);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { cls.Id }, "Class created"));
    }

    [HttpPost("{classId:guid}/sections")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> AddSection(Guid classId, [FromBody] CreateSectionDto dto)
    {
        var section = new Section { Name = dto.Name, ClassId = classId, MaxStudents = dto.MaxStudents };
        await _context.Sections.AddAsync(section);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { section.Id }, "Section created"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var cls = await _context.Classes.FindAsync(id);
        if (cls is null) return NotFound();
        cls.IsDeleted = true;
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Class deleted"));
    }
}

// Inline simple DTOs
public record CreateClassDto(string Name, int AcademicYear);
public record CreateSectionDto(string Name, int MaxStudents);
