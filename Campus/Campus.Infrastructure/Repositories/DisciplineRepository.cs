﻿using Campus.Core.Domain.Entities;
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
        ApplicationDbContext _db;

        public DisciplineRepository(ApplicationDbContext db)
        {
            _db = db;
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

            _db.Disciplines.RemoveRange(result);

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

        public Task<Discipline?> Update(Discipline entity)
        {
            throw new NotImplementedException();
        }
    }
}
