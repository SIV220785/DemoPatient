using Microsoft.EntityFrameworkCore;
using Patient.DAL.Entities;

namespace Patient.DAL.Context;

public class PatientDbContext(DbContextOptions<PatientDbContext> options) : DbContext(options)
{
    public virtual DbSet<Entities.Patient> Patients { get; set; }

    public virtual DbSet<Name> Names { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Active> Actives { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Patient>()
            .HasOne(p => p.Name) // Указание на одно-ко-многим отношение
            .WithMany() // Скорее всего здесь должно быть точное указание коллекции, если таковая существует
            .HasForeignKey(p => p.NameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Entities.Patient>()
            .HasOne(p => p.Gender) // Указание на одно-ко-многим отношение
            .WithMany() // Уточнить, есть ли коллекция в Gender
            .HasForeignKey(p => p.GenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Entities.Patient>()
            .HasOne(p => p.Active)
            .WithMany() // Аналогично Gender, нужно уточнить отношения
            .HasForeignKey(p => p.ActiveId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Name>()
            .HasIndex(n => n.Family) // Настройка для оптимизации поиска по фамилии
            .IsUnique(false);
    }
}