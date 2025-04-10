using Domain.Models;

namespace Service.Interfaces;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeById(int id);
    Task<IEnumerable<Employee>> GetAllEmployees();
    Task<Employee> CreateEmployee(Employee employee);
    Task<IEnumerable<Employee>> CreateEmployees(IEnumerable<Employee> employees);
    Task<Employee> UpdateEmployee(Employee employee);
    Task<bool> DeleteEmployee(int id);
}
