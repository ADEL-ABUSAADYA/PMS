using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PMS.Models;


namespace PMS.Data.Repositories
{
    public interface IRepository<Entity> where Entity : BaseModel 
    {
        DbSet<Entity> Query();
        void Add(Entity entity, CancellationToken cancellationToken = default);
        Task<bool> SaveIncludeAsync(Entity entity, params string[] properties);
        void SaveInclude(Entity entity, params string[] properties);
        void Delete(Entity entity);
        void HardDelete(Entity entity);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllWithDeleted();
        IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
        Entity GetByID(Guid id);

        Task<Entity> GetByIDAsync(Guid id);
        void SaveChanges();
        Task<Guid> AddAsync(Entity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<Entity> entities);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
