using Domain.Models;

namespace Service.Interfaces;

public interface IDepartmentService
{
    Task<Department> GetDepartmentById(int id);
    Task<IEnumerable<Department>> GetAllDepartments();
    Task<Department> CreateDepartment(Department department);
    Task<Department> UpdateDepartment(Department department);
    Task<bool> DeleteDepartment(int id);
}
