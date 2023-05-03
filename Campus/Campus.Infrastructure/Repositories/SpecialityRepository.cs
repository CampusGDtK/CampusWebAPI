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
    public class SpecialityRepository : IRepository<Speciality>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SpecialityRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(Speciality entity)
        {
            await _db.Specialities.AddAsync(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _db.Specialities.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return false;

            _db.Remove(result);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Speciality>> GetAll()
        {
            return _db.Specialities.AsEnumerable();
        }

        public async Task<Speciality?> GetValueById(Guid id)
        {
            return await _db.Specialities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Speciality?> Update(Speciality entity)
        {
            var result = await _db.Specialities.FindAsync(entity.Id);

            if (result is null)
            {
                return null;
            }

            _mapper.Map(entity, result);
            await _db.SaveChangesAsync();
            return result;
        }
    }
}
