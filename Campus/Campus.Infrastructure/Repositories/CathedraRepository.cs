using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Infrastructure.Repositories
{
    public class CathedraRepository : IRepository<Cathedra>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CathedraRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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

        public async Task<Cathedra?> Update(Cathedra entity)
        {
            Cathedra? cathedra = await _db.Cathedras.FindAsync(entity.Id);
            if(cathedra == null)
            {
                return null;
            }

            _mapper.Map(entity, cathedra);
            await _db.SaveChangesAsync();
            return cathedra;
        }
    }
}
