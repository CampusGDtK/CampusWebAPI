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
    public class DisciplineRepository : IRepository<Discipline>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public DisciplineRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(Discipline entity)
        {
            await _db.Disciplines.AddAsync(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _db.Disciplines.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
                return false;

            _db.Disciplines.Remove(result);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Discipline>> GetAll()
        {
            var result = _db.Disciplines.Include(x => x.Cathedral).AsEnumerable();

            return result;
        }

        public async Task<Discipline?> GetValueById(Guid id)
        {
            var result = await _db.Disciplines.Include(x => x.Cathedral).FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Discipline?> Update(Discipline entity)
        {
            var result = await _db.Disciplines.FindAsync(entity.Id);

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
