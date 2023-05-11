using System.Reflection;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Core.ServiceContracts;
using Campus.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Campus.Core;

public static class DependencyInjection
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly);
        services.AddScoped<IAcademicService, AcademicService>();
        services.AddScoped<IADGService, ADGService>();
        services.AddScoped<ICathedraService, CathedraService>();
        services.AddScoped<IDisciplineService, DisciplineService>();
        services.AddScoped<IFacultyService, FacultyService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IMarkingService, MarkingService>();
        services.AddScoped<ISpecialityService, SpecialityService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStudyProgramService, StudyProgramService>();
        services.AddScoped<ISyllabusService, SyllabusService>();

        return services;
    }
}