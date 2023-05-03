using AutoMapper;
using Campus.Core.Domain.Entities;

namespace Campus.Core.MappingProfile;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Student, Student>();
    }
    
}