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
    public class AcademicDisciplineGroupRespository : IRepository<AcademicDisciplineGroup>
    {
        private readonly ApplicationDbContext _db;

        public AcademicDisciplineGroupRespository(ApplicationDbContext db)
        {
            _db = db;
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
                .Include("Academic")
                .Include("Discipline")
                .Include("Group")
                .AsEnumerable();
        }

        public async Task<AcademicDisciplineGroup?> GetValueById(Guid id)
        {
            return await _db.AcademicDisciplineGroups
                .Include("Academic")
                .Include("Discipline")
                .Include("Group")
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AcademicDisciplineGroup?> Update(AcademicDisciplineGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
