using assignment01.Areas.ProductManagement.Models;
using assignment01.Controllers;
using assignment01.Data;
using assignment01.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment01.Areas.ProductManagement.Controllers;

[Area("ProductManagement")]
[Route("Categories")]
[Authorize(Roles = "Admin")]
public class CategoryController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ErrorLog _errorLog;

    // dependency injection
    public CategoryController(ApplicationDbContext context, ErrorLog errorLog)
    {
        _context = context;
        _errorLog = errorLog;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost("Add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Category category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorLog.LogError(ex);
                throw new Exception(ex.Message, ex);
            }
        }

        return View(category);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("CategoryId,CategoryName,CategoryDescription")]
        Category category)
    {
        if (id != category.CategoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!(await CategoryExists(category.CategoryId)))
                {
                    return NotFound();
                }

                _errorLog.LogError(ex);
                throw new Exception(ex.Message, ex);
            }


            return RedirectToAction("Index");
        }

        return View(category);
    }


    private async Task<bool> CategoryExists(int id)
    {
        try
        {
            return await _context.Categories.AnyAsync(e => e.CategoryId == id);
        }
        catch (Exception ex)
        {
            _errorLog.LogError(ex);
            throw new Exception(ex.Message, ex);
        }
    }
}