using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Infrastructure.Repositories
{
    public class AcademicDisciplineGroupRepository : IRepository<AcademicDisciplineGroup>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AcademicDisciplineGroupRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(AcademicDisciplineGroup entity)
        {
            _db.AcademicDisciplineGroups.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            AcademicDisciplineGroup? adg = await _db.AcademicDisciplineGroups.FirstOrDefaultAsync(a => a.Id == id);

            if(adg == null)
            {
                return false;
            }

            _db.AcademicDisciplineGroups.Remove(adg);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AcademicDisciplineGroup>> GetAll()
        {
            return _db.AcademicDisciplineGroups
                .Include(x => x.Academic)
                .Include(x => x.Discipline)
                .Include(x => x.Group)
                .AsEnumerable();
        }

        public async Task<AcademicDisciplineGroup?> GetValueById(Guid id)
        {
            return await _db.AcademicDisciplineGroups
                .Include(x => x.Academic)
                .Include(x => x.Discipline)
                .Include(x => x.Group)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AcademicDisciplineGroup?> Update(AcademicDisciplineGroup entity)
        {
            AcademicDisciplineGroup? adg = await _db.AcademicDisciplineGroups.FindAsync(entity.Id);
            if (adg == null)
            {
                return null;
            }

            _mapper.Map(entity, adg);
            await _db.SaveChangesAsync();
            return adg;
        }
    }
}
