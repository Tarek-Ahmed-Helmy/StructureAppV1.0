using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

internal sealed class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    public DepartmentRepository (ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
