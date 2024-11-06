using Microsoft.EntityFrameworkCore;
using Patient.DAL.Entities;

namespace Patient.DAL.Context;

public class PatientDbContext(DbContextOptions<PatientDbContext> options) : DbContext(options)
{
    public virtual DbSet<PatientProfile> Patients { get; set; }

    public virtual DbSet<PatientDetail> PatientDetails { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Active> Actives { get; set; }

    public virtual DbSet<Given> Givens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи между PatientProfile и PatientDetails
        modelBuilder.Entity<PatientProfile>()
            .HasOne(p => p.PatientDetail)
            .WithMany()
            .HasForeignKey(p => p.PatientDetailId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи между PatientProfile и Gender
        modelBuilder.Entity<PatientProfile>()
            .HasOne(p => p.Gender) // У PatientProfile есть один Gender
            .WithMany() // Gender может быть привязан к многим PatientProfile (не указана коллекция)
            .HasForeignKey(p => p.GenderId)
            .OnDelete(DeleteBehavior.Restrict); // Запрет на каскадное удаление

        // Настройка связи между PatientProfile и Active
        modelBuilder.Entity<PatientProfile>()
            .HasOne(p => p.Active) // У PatientProfile есть один Active
            .WithMany() // Active может быть привязан к многим PatientProfile (не указана коллекция)
            .HasForeignKey(p => p.ActiveId)
            .OnDelete(DeleteBehavior.Restrict); // Запрет на каскадное удаление

        // Настройка связи между PatientDetails и Given
        modelBuilder.Entity<PatientDetail>()
            .HasMany(p => p.Givens) // У PatientDetails есть много Given
            .WithOne(g => g.PatientDetail) // У каждого Given есть один PatientDetails
            .HasForeignKey(g => g.PatientDetailsId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление Given при удалении PatientDetails

        // Эффективный поиск по полю Family в PatientDetails
        modelBuilder.Entity<PatientDetail>()
            .HasIndex(p => p.Family)
            .IsUnique(false); // Family не является уникальным

        // Инициализация начальных данных для Gender
        modelBuilder.Entity<Gender>().HasData(
            new Gender {Id = 1, Type = "Male"},
            new Gender {Id = 2, Type = "Female"},
            new Gender {Id = 3, Type = "Other"},
            new Gender {Id = 4, Type = "Unknown"}
        );

        // Инициализация начальных данных для Active
        modelBuilder.Entity<Active>().HasData(
            new Active {Id = 1, IsActive = true},
            new Active {Id = 2, IsActive = false}
        );
    }
}
