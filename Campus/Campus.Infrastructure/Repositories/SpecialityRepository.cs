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

        public SpecialityRepository(ApplicationDbContext db)
        {
            _db = db;
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

        public Task<Speciality?> Update(Speciality entity)
        {
            throw new NotImplementedException();
        }
    }
}
