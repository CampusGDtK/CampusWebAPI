using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Campus.Infrastructure.Repositories;

public class StudyProgramRepository : IRepository<StudyProgram>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public StudyProgramRepository(ApplicationDbContext _db, IMapper mapper)
    {
        this._db = _db;
        _mapper = mapper;
    }
    public async Task<StudyProgram?> GetValueById(Guid id)
    {
        return await _db.StudyPrograms
            .Include(sp => sp.Speciality)
            .FirstOrDefaultAsync(sp => sp.Id == id);
    }

    public async Task<IEnumerable<StudyProgram>> GetAll()
    {
        return await _db.StudyPrograms
            .Include(sp => sp.Speciality)
            .ToListAsync();
    }

    public async Task Create(StudyProgram entity)
    {
        await _db.StudyPrograms.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<StudyProgram?> Update(StudyProgram entity)
    {
        var studyProgram = await _db.StudyPrograms.FindAsync(entity.Id);

        if (studyProgram is null)
        {
            return null;
        }

        _mapper.Map(entity, studyProgram);
        await _db.SaveChangesAsync();
        return studyProgram;
    }

    public async Task<bool> Delete(Guid id)
    {
        var studyProgram = await _db.StudyPrograms.FindAsync(id);
        if (studyProgram is null)
        {
            return false;
        }
        _db.StudyPrograms.Remove(studyProgram);
        await _db.SaveChangesAsync();
        return true;
    }
}