using Domain.Interfaces;
using Domain.Models;
using Service.Interfaces;

namespace Service.Implementation;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        return await _unitOfWork.Employee.FindAsync(e => e.Id == id, ["Department"]);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _unitOfWork.Employee.FindAllAsync(includes: ["Department"]);
    }

    public async Task<Employee> CreateEmployee(Employee employee)
    {
        await _unitOfWork.Employee.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateEmployee(Employee employee)
    {
        await _unitOfWork.Employee.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _unitOfWork.Employee.GetByIdAsync(id);
        if (employee == null) return false;
        await _unitOfWork.Employee.DeleteAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Employee>> CreateEmployees(IEnumerable<Employee> employees)
    {
        await _unitOfWork.Employee.AddRangeAsync(employees);
        await _unitOfWork.SaveChangesAsync();
        return employees;
    }
}
