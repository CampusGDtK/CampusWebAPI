using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Infrastructure
{
    public static class DependencyInjection
    {
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Academic>, AcademicRepository>();
            services.AddScoped<IRepository<AcademicDisciplineGroup>, AcademicDisciplineGroupRepository>();
            services.AddScoped<IRepository<Cathedra>, CathedraRepository>();
            services.AddScoped<IRepository<CurrentControl>, CurrentControlRepository>();
            services.AddScoped<IRepository<Discipline>, DisciplineRepository>();
            services.AddScoped<IRepository<Faculty>, FacultyRepository>();
            services.AddScoped<IRepository<Group>, GroupRepository>();
            services.AddScoped<IRepository<Speciality>, SpecialityRepository>();
            services.AddScoped<IRepository<SpecialityFaculty>, SpecialityFacultyRepository>();
            services.AddScoped<IRepository<Student>, StudentRepository>();
            services.AddScoped<IRepository<StudyProgram>, StudyProgramRepository>();

            return services;
        }
    }
}
