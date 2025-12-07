using System.Linq.Expressions;

namespace HomeworkPortal.Repositories
{
    // T generic: User, Category, Assignment gibi herhangi bir entity olabilir
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);                 // Id ile tek kayıt
        Task<List<T>> GetAllAsync();                   // Tüm kayıtlar
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate); // filtreli liste
        Task AddAsync(T entity);                       // Ekle
        void Update(T entity);                         // Güncelle
        void Remove(T entity);                         // Sil
        Task<int> SaveChangesAsync();                  // DB'ye yaz
    }
}
