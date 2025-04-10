using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using Presentation.ViewModels;
using Service.Interfaces;

namespace Presentation.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IMapper mapper)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "Employee.Index")]
    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployees();
        var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(employees);
        return View(mappedEmployees);
    }

    [HttpGet]
    [Authorize(Policy = "Employee.Details")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var employee = await _employeeService.GetEmployeeById(id.Value);
        if (employee == null)
        {
            return NotFound();
        }
        var mappedEmployee = _mapper.Map<Employee, EmployeeVM>(employee);
        return View(mappedEmployee);
    }

    [HttpGet]
    [Authorize(Policy = "Employee.Create")]
    public async Task<IActionResult> Create()
    {
        var departments = await _departmentService.GetAllDepartments();
        EmployeeVM employeeVM = new()
        {
            DepartmentList = departments.Select(d => new SelectListItem
            {
                Text = d.DepartmentName,
                Value = d.Id.ToString()
            })
        };
        return View(employeeVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Employee.Create")]
    public async Task<IActionResult> Create(EmployeeVM newEmployeeVM)
    {
        if (ModelState.IsValid)
        {
            var newEmployee = _mapper.Map<EmployeeVM, Employee>(newEmployeeVM);
            await _employeeService.CreateEmployee(newEmployee);
            return RedirectToAction(nameof(Details), new { id = newEmployee.Id });
        }
        return View(newEmployeeVM);
    }

    [HttpGet]
    [Authorize(Policy = "Employee.Update")]
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var employee = await _employeeService.GetEmployeeById(id.Value);
        if (employee == null)
        {
            return NotFound();
        }
        var departments = await _departmentService.GetAllDepartments();
        var mappedEmployee = _mapper.Map<Employee, EmployeeVM>(employee);
        mappedEmployee.DepartmentList = departments.Select(d => new SelectListItem
        {
            Text = d.DepartmentName,
            Value = d.Id.ToString()
        });
        return View(mappedEmployee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Employee.Update")]
    public async Task<IActionResult> Update(EmployeeVM updatedEmployeeVM, int id)
    {
        if (id != updatedEmployeeVM.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var mappedEmp = _mapper.Map<EmployeeVM, Employee>(updatedEmployeeVM);
                await _employeeService.UpdateEmployee(mappedEmp);
                return RedirectToAction(nameof(Details), new { id = mappedEmp.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updatedEmployeeVM);
            }
        }
        return View(updatedEmployeeVM);
    }

    [HttpGet]
    [Authorize(Policy = "Employee.Delete")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var employee = await _employeeService.GetEmployeeById(id.Value);
        if (employee == null)
        {
            return NotFound();
        }
        var mappedEmployee = _mapper.Map<Employee, EmployeeVM>(employee);
        var departments = await _departmentService.GetAllDepartments();
        mappedEmployee.DepartmentList = departments.Select(d => new SelectListItem
        {
            Text = d.DepartmentName,
            Value = d.Id.ToString()
        });
        return View(mappedEmployee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Employee.Delete")]
    public async Task<IActionResult> Delete(EmployeeVM deletedEmployeeVM, int id )
    {
        if (id != deletedEmployeeVM.Id)
        {
            return BadRequest();
        }

        try
        {
            await _employeeService.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(deletedEmployeeVM);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = "Employee.UploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return RedirectToAction("Index");
        }

        var employeesList = new List<Employee>();

        using (var stream = new MemoryStream())
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            file.CopyTo(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var employee = new Employee
                    {
                        FirstName = worksheet.Cells[row, 1].Value?.ToString(),
                        LastName = worksheet.Cells[row, 2].Value?.ToString(),
                        Age = int.TryParse(worksheet.Cells[row, 3].Value?.ToString(), out int age) ? age : null,
                        Salary = int.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out int salary) ? salary : 0,
                        Title = worksheet.Cells[row, 5].Value?.ToString(),
                        HireDate = DateTime.TryParse(worksheet.Cells[row, 6].Value?.ToString(), out DateTime hireDate) ? hireDate : DateTime.Now,
                        DepartmentId = int.TryParse(worksheet.Cells[row, 7].Value?.ToString(), out int deptId) ? deptId : null
                    };
                    employeesList.Add(employee);
                }
            }
        }
        await _employeeService.CreateEmployees(employeesList);
        return RedirectToAction(nameof(Index));
    }

}