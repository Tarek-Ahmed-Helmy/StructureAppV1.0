using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IDepartmentRepository Department { get; }
    public IEmployeeRepository Employee { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Department = new DepartmentRepository(context);
        Employee = new EmployeeRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
