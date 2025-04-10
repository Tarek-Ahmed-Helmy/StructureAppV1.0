namespace Service.Interfaces;

public interface IServiceManager
{
    IEmployeeService EmployeeService { get; }
    IDepartmentService DepartmentService { get; }
}
