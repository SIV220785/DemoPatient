using Patient.DAL.Entities;

namespace Patient.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Entities.Patient> Patients { get; }

    IGenericRepository<Name> Names { get; }

    IGenericRepository<Gender> Genders { get; }

    IGenericRepository<Active> Actives { get; }

    int Complete();
}