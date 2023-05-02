using Campus.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Campus.Infrastructure.DataBaseContext;

public class ApplicationDbContext : DbContext
{
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Academic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Academic_pkey");

            entity.ToTable("Academic");

            entity.HasIndex(e => e.CathedralId, "academic_cathedralid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Cathedral).WithMany(p => p.Academics)
                .HasForeignKey(d => d.CathedralId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("academic_cathedralid_foreign");
        });

        modelBuilder.Entity<AcademicDisciplineGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AcademicDisciplineGroup_pkey");

            entity.ToTable("AcademicDisciplineGroup");

            entity.HasIndex(e => e.AcademicId, "academicdisciplinegroup_academicid_index");

            entity.HasIndex(e => e.DisciplineId, "academicdisciplinegroup_disciplineid_index");

            entity.HasIndex(e => e.GroupId, "academicdisciplinegroup_groupid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.HasOne(d => d.Academic).WithMany(p => p.AcademicDisciplineGroups)
                .HasForeignKey(d => d.AcademicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("academicdisciplinegroup_academicid_foreign");

            entity.HasOne(d => d.Discipline).WithMany(p => p.AcademicDisciplineGroups)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("academicdisciplinegroup_disciplineid_foreign");

            entity.HasOne(d => d.Group).WithMany(p => p.AcademicDisciplineGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("academicdisciplinegroup_groupid_foreign");
        });

        modelBuilder.Entity<Cathedra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cathedra_pkey");

            entity.ToTable("Cathedra");

            entity.HasIndex(e => e.FacultyId, "cathedra_facultyid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Head).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Faculty).WithMany(p => p.Cathedras)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cathedra_facultyid_foreign");
        });

        modelBuilder.Entity<CurrentControl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CurrentControl_pkey");

            entity.ToTable("CurrentControl");

            entity.HasIndex(e => e.DisciplineId, "currentcontrol_disciplineid_index");

            entity.HasIndex(e => e.StudentId, "currentcontrol_studentid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Detail).HasColumnType("jsonb");
            entity.Property(e => e.Mark).HasColumnType("jsonb");

            entity.HasOne(d => d.Discipline).WithMany(p => p.CurrentControls)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("currentcontrol_disciplineid_foreign");

            entity.HasOne(d => d.Student).WithMany(p => p.CurrentControls)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("currentcontrol_studentid_foreign");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Discipline_pkey");

            entity.ToTable("Discipline");

            entity.HasIndex(e => e.CathedralId, "discipline_cathedralid_index");

            entity.HasIndex(e => e.Name, "discipline_name_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Cathedral).WithMany(p => p.Disciplines)
                .HasForeignKey(d => d.CathedralId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("discipline_cathedralid_foreign");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Faculty_pkey");

            entity.ToTable("Faculty");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Dean).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Group_pkey");

            entity.ToTable("Group");

            entity.HasIndex(e => e.CuratorId, "group_curatorid_index");

            entity.HasIndex(e => e.FacultyId, "group_facultyid_index");

            entity.HasIndex(e => e.StudyProgramId, "group_studyprogramid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Curator).WithMany(p => p.Groups)
                .HasForeignKey(d => d.CuratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_curatorid_foreign");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Groups)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_facultyid_foreign");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.Groups)
                .HasForeignKey(d => d.StudyProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("group_studyprogramid_foreign");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Speciality_pkey");

            entity.ToTable("Speciality");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<SpecialityFaculty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SpecialityFaculty_pkey");

            entity.ToTable("SpecialityFaculty");

            entity.HasIndex(e => e.FacultyId, "specialityfaculty_facultyid_index");

            entity.HasIndex(e => e.SpecialityId, "specialityfaculty_specialityid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.HasOne(d => d.Faculty).WithMany(p => p.SpecialityFaculties)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialityfaculty_facultyid_foreign");

            entity.HasOne(d => d.Speciality).WithMany(p => p.SpecialityFaculties)
                .HasForeignKey(d => d.SpecialityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialityfaculty_specialityid_foreign");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Student_pkey");

            entity.ToTable("Student");

            entity.HasIndex(e => e.GroupId, "student_groupid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_groupid_foreign");
        });

        modelBuilder.Entity<StudyProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("StudyProgram_pkey");

            entity.ToTable("StudyProgram");

            entity.HasIndex(e => e.SpecialityId, "studyprogram_specialityid_index");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Speciality).WithMany(p => p.StudyPrograms)
                .HasForeignKey(d => d.SpecialityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("studyprogram_specialityid_foreign");
        });
    }
}
