using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Presentation.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [Authorize(Policy = "Department.Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _departmentService.GetAllDepartments());
    }

    [HttpGet]
    [Authorize(Policy = "Department.Details")]
    public async Task<IActionResult> Details(int id)
    {
        return View(await _departmentService.GetDepartmentById(id));
    }

    [HttpGet]
    [Authorize(Policy = "Department.Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Department.Create")]
    public async Task<IActionResult> Create(Department newDepartment)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _departmentService.CreateDepartment(newDepartment);
                return RedirectToAction(nameof(Details), new { id = newDepartment.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(newDepartment);
    }

    [HttpGet]
    [Authorize(Policy = "Department.Update")]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var department = await _departmentService.GetDepartmentById(id.Value);
        if (department == null)
        {
            return NotFound();
        }
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Department.Update")]
    public async Task<IActionResult> Update(Department updatedDepartment, int id)
    {
        if (id != updatedDepartment.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _departmentService.UpdateDepartment(updatedDepartment);
                return RedirectToAction(nameof(Details), new { id = updatedDepartment.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
        }
        return View(updatedDepartment);
    }

    [HttpGet]
    [Authorize(Policy = "Department.Delete")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var department = await _departmentService.GetDepartmentById(id.Value);
        if (department == null)
        {
            return NotFound();
        }
        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Department.Delete")]
    public async Task<IActionResult> Delete(Department deletedDepartment, int id)
    {
        if (id != deletedDepartment.Id)
        {
            return BadRequest();
        }

        try
        {
            await _departmentService.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(deletedDepartment);
        }  
    }
}
