﻿using AutoMapper;
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
    public class FacultyRepository : IRepository<Faculty>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public FacultyRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task Create(Faculty entity)
        {
            await _db.Faculties.AddAsync(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _db.Faculties.FirstOrDefaultAsync(x => x.Id == id);

            if(result == null)
                return false;

            _db.Faculties.Remove(result);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Faculty>> GetAll()
        {
            var result = _db.Faculties.Include(x => x.SpecialityFaculties).ThenInclude(x => x.Speciality).AsEnumerable();

            return result;
        }

        public async Task<Faculty?> GetValueById(Guid id)
        {
            var result =  await _db.Faculties.FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Faculty?> Update(Faculty entity)
        {
            var result = await _db.Faculties.FindAsync(entity.Id);

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
