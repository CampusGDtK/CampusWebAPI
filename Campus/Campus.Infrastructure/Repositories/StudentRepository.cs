using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Campus.Infrastructure.Repositories;

public class StudentRepository : IRepository<Student>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public StudentRepository(ApplicationDbContext _db, IMapper mapper)
    {
        this._db = _db;
        _mapper = mapper;
    }
    public async Task<Student?> GetValueById(Guid id)
    {
        return await _db.Students
            .Include(s => s.Group)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetAll()
    {
        return await _db.Students
            .Include(s => s.Group)
            .ToListAsync();
    }

    public async Task Create(Student entity)
    {
        await _db.Students.AddAsync(entity);
    }

    public async Task<Student?> Update(Student entity)
    {
        var student = await _db.Students.FindAsync(entity.Id);
        if (student is null)
        {
            return null;
        }
        
        _mapper.Map(entity, student);
        return student;
    }

    public async Task<bool> Delete(Guid id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null)
        {
            return false;
        }
        _db.Students.Remove(student);
        return true;
    }
}