using Domain.Interfaces;
using Domain.Models;
using Service.Interfaces;

namespace Service.Implementation;

internal sealed class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Department> CreateDepartment(Department department)
    {
        await _unitOfWork.Department.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return department;
    }

    public Task<IEnumerable<Department>> GetAllDepartments()
    {
        return _unitOfWork.Department.GetAllAsync();
    }

    public Task<Department> GetDepartmentById(int id)
    {
        return _unitOfWork.Department.GetByIdAsync(id);
    }

    public async Task<Department> UpdateDepartment(Department department)
    {
        await _unitOfWork.Department.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return department;
    }

    public async Task<bool> DeleteDepartment(int id)
    {
        var department = await _unitOfWork.Department.GetByIdAsync(id);
        if (department == null) return false;
        await _unitOfWork.Department.DeleteAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
