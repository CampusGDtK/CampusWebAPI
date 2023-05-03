﻿using AutoMapper;
using Campus.Core.Domain.Entities;
using Campus.Core.Domain.RepositoryContracts;
using Campus.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Infrastructure.Repositories
{
    public class GroupRepository : IRepository<Group>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GroupRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(Group entity)
        {
            await _db.Groups.AddAsync(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _db.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if(result == null)
                return false;

            _db.Groups.Remove(result);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Group>> GetAll()
        {
            return _db.Groups.Include(x => x.Curator).Include(x => x.Faculty).Include(x => x.StudyProgram).AsEnumerable();
        }

        public async Task<Group?> GetValueById(Guid id)
        {
            return await _db.Groups.Include(x => x.Curator).Include(x => x.Faculty).Include(x => x.StudyProgram).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group?> Update(Group entity)
        {
            var result = await _db.Groups.FindAsync(entity.Id);

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
