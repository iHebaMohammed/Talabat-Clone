using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly TalabatContext _context;

        public GenaricRepository(TalabatContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //return await _context.Set<T>().Where(item => item.Id == id).FirstOrDefaultAsync();
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecifications(specification).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecifications(specification).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> specification)
        {
            return await ApplySpecifications(specification).CountAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>() , specification);
        }

        public async Task CreateAsync(T entity)
            =>await _context.Set<T>().AddAsync(entity);
        

        public  void UpdateAsync(T entity)
            =>  _context.Set<T>().Update(entity);

        public void DeleteAsync(T entity)
            => _context.Set<T>().Remove(entity);
    }
}
