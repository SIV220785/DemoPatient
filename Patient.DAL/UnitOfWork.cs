using Patient.DAL.Context;
using Patient.DAL.Entities;
using Patient.DAL.Interfaces;
using Patient.DAL.Repository;

namespace Patient.DAL;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;

    private readonly PatientDbContext _context;

    public IGenericRepository<PatientProfile> PatientProfiles { get; }
    public IGenericRepository<PatientDetail> PatientDetails { get; }
    public IGenericRepository<Gender> Genders { get; }
    public IGenericRepository<Active> Actives { get; }
    public IGenericRepository<Given> Givens { get; }

    public UnitOfWork(PatientDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;

        PatientProfiles = new GenericRepository<PatientProfile>(_context);
        PatientDetails = new GenericRepository<PatientDetail>(_context);
        Genders = new GenericRepository<Gender>(_context);
        Actives = new GenericRepository<Active>(_context);
        Givens = new GenericRepository<Given>(_context);
    }

    public int Complete() => _context.SaveChanges();

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
