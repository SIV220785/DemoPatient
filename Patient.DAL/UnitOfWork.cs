using Patient.DAL.Context;
using Patient.DAL.Entities;
using Patient.DAL.Interfaces;
using Patient.DAL.Repository;

namespace Patient.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PatientDbContext _context;

        public UnitOfWork(PatientDbContext context)
        {
            _context = context;

            Patients = new GenericRepository<Entities.Patient>(_context);
            Names = new GenericRepository<Name>(_context);
            Genders = new GenericRepository<Gender>(_context);
            Actives = new GenericRepository<Active>(_context);
        }

        public IGenericRepository<Entities.Patient> Patients { get; }

        public IGenericRepository<Name> Names { get; }

        public IGenericRepository<Gender> Genders { get; }

        public IGenericRepository<Active> Actives { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
