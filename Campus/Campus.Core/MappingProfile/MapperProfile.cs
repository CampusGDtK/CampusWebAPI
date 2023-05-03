using AutoMapper;
using Campus.Core.Domain.Entities;

namespace Campus.Core.MappingProfile;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Student, Student>();
        CreateMap<Group, Group>();
        CreateMap<Faculty, Faculty>();
        CreateMap<AcademicDisciplineGroup, AcademicDisciplineGroup>();
        CreateMap<Academic, Academic>();
        CreateMap<Cathedra, Cathedra>();
        CreateMap<CurrentControl, CurrentControl>();
        CreateMap<Discipline, Discipline>();
        CreateMap<Speciality, Speciality>();
        CreateMap<StudyProgram, StudyProgram>();
    }
    
}