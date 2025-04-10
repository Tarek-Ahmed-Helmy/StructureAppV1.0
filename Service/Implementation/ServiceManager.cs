using Domain.Interfaces;
using Service.Interfaces;

namespace Service.Implementation;

// our service instances are only going to be created when we access them for the first time, and not before that
public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IEmployeeService> _lazyEmployeeService;
    private readonly Lazy<IDepartmentService> _lazyDepartmentService;

    public ServiceManager(IUnitOfWork unitOfWork)
    {
        _lazyEmployeeService = new Lazy<IEmployeeService>(() => new EmployeeService(unitOfWork));
        _lazyDepartmentService = new Lazy<IDepartmentService>(() => new DepartmentService(unitOfWork));
    }

    public IEmployeeService EmployeeService => _lazyEmployeeService.Value;

    public IDepartmentService DepartmentService => _lazyDepartmentService.Value;
}
