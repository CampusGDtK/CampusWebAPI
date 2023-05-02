using Campus.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Campus.Infrastructure.DataBaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Academic> Academics { get; set; }

    public virtual DbSet<AcademicDisciplineGroup> AcademicDisciplineGroups { get; set; }

    public virtual DbSet<Cathedra> Cathedras { get; set; }

    public virtual DbSet<CurrentControl> CurrentControls { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Speciality> Specialities { get; set; }

    public virtual DbSet<SpecialityFaculty> SpecialityFaculties { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudyProgram> StudyPrograms { get; set; }
}
