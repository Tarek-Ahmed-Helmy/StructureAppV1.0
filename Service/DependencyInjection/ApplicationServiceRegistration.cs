using Microsoft.Extensions.DependencyInjection;
using Service.Implementation;
using Service.Interfaces;

namespace Service.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        return services;
    }
}
