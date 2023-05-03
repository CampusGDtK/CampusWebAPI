using System.Reflection;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Campus.Core;

public static class DependencyInjection
{
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly);
        
        return services;
    }
}