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
    public class CathedraRepository : IRepository<Cathedra>
    {
        private readonly ApplicationDbContext _db;

        public CathedraRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Cathedra entity)
        {
            _db.Cathedras.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            Cathedra? cathedra = await _db.Cathedras.FirstOrDefaultAsync(c => c.Id == id);

            if(cathedra == null)
            {
                return false;
            }

            _db.Cathedras.Remove(cathedra);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Cathedra>> GetAll()
        {
            return _db.Cathedras.Include(x => x.Faculty).AsEnumerable();
        }

        public async Task<Cathedra?> GetValueById(Guid id)
        {
            return await _db.Cathedras.Include(x => x.Faculty).FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Cathedra?> Update(Cathedra entity)
        {
            throw new NotImplementedException();
        }
    }
}
