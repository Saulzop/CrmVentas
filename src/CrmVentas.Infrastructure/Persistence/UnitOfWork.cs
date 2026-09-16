using CrmVentas.Application.Common.Interfaces;

namespace CrmVentas.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> GuardarCambiosAsync() => _context.SaveChangesAsync();
}
