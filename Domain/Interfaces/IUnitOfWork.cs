namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employee { get; }
    IDepartmentRepository Department { get; }
    Task<int> SaveChangesAsync();
}
