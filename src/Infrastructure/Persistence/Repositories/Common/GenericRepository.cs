using LotTrace_MES.src.Domain.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace LotTrace_MES.src.Infrastructure.Persistence.Repositories.Common
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context; // DbContext 인스턴스
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _dbSet = context.Set<T>(); // Type T의 대한 테이블 설정
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Updated(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
