using Patient.DAL.Entities;

namespace Patient.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<PatientProfile> PatientProfiles { get; }

    IGenericRepository<PatientDetail> PatientDetails { get; }

    IGenericRepository<Gender> Genders { get; }

    IGenericRepository<Active> Actives { get; }

    IGenericRepository<Given> Givens { get; }

    int Complete();

    Task<int> CompleteAsync();
}
