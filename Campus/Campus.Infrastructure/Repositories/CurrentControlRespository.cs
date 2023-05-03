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
    public class CurrentControlRespository : IRepository<CurrentControl>
    {
        private readonly ApplicationDbContext _db;

        public CurrentControlRespository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Create(CurrentControl entity)
        {
            _db.CurrentControls.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            CurrentControl? currentControl = await _db.CurrentControls.FirstOrDefaultAsync(c => c.Id == id);

            if(currentControl == null)
            {
                return false;
            }

            _db.CurrentControls.Remove(currentControl);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CurrentControl>> GetAll()
        {
            return _db.CurrentControls
                .Include(x => x.Discipline)
                .Include(x => x.Student)
                .AsEnumerable();
        }

        public async Task<CurrentControl?> GetValueById(Guid id)
        {
            return await _db.CurrentControls
                .Include(x => x.Discipline)
                .Include(x => x.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<CurrentControl?> Update(CurrentControl entity)
        {
            throw new NotImplementedException();
        }
    }
}
