﻿using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LoichDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LoichDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            List<T> list = await _dbSet.Where(expression).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<T>> FindListAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }
        
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
