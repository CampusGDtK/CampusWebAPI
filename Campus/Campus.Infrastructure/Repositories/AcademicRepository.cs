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
    public class AcademicRepository : IRepository<Academic>
    {
        private readonly ApplicationDbContext _db;

        public AcademicRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(Academic entity)
        {
            _db.Academics.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            Academic? academic = await _db.Academics.FirstOrDefaultAsync(a => a.Id == id);

            if(academic == null)
            {
                return false;
            }

            _db.Academics.Remove(academic);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Academic>> GetAll()
        {
            return _db.Academics.Include("Cathedral").AsEnumerable();
        }

        public async Task<Academic?> GetValueById(Guid id)
        {
            return await _db.Academics.Include("Cathedral").FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Academic?> Update(Academic entity)
        {
            throw new NotImplementedException();
        }
    }
}
