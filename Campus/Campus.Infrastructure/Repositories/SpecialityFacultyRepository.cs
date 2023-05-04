using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Infrastructure.Repositories
{
    public class SpecialityFacultyRepository : IRepository<SpecialityFaculty>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SpecialityFacultyRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(SpecialityFaculty entity)
        {
            _db.SpecialityFaculties.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            SpecialityFaculty? specialityFaculty = await _db.SpecialityFaculties.FindAsync(id);

            if(specialityFaculty == null)
            {
                return false;
            }

            _db.SpecialityFaculties.Remove(specialityFaculty);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SpecialityFaculty>> GetAll()
        {
            return _db.SpecialityFaculties
                .Include(x => x.Faculty)
                .Include(x => x.Speciality)
                .AsEnumerable();
        }

        public async Task<SpecialityFaculty?> GetValueById(Guid id)
        {
            return await _db.SpecialityFaculties
                .Include(x => x.Faculty)
                .Include(x => x.Speciality)
                .FirstOrDefaultAsync(specialityFaculty => specialityFaculty.Id == id);
        }

        public async Task<SpecialityFaculty?> Update(SpecialityFaculty entity)
        {
            SpecialityFaculty? specialityFaculty = await _db.SpecialityFaculties.FindAsync(entity.Id);
            if(specialityFaculty == null)
            {
                return null;
            }

            _mapper.Map(entity, specialityFaculty);
            await _db.SaveChangesAsync();
            return specialityFaculty;
        }
    }
}
