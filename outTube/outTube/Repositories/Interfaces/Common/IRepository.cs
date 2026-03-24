using System.Linq.Expressions;

namespace OurTube.Repositories.Interfaces.Common
{
    public interface IRepository<T>
    {
        public List<T> GetByCondition(Expression<Func<T, bool>> predicate);
        public List<T> GetAll();
        public bool Create(T entity);
        public bool Update(T entity);
        public bool Delete(T entity);
        public int Save();
    }
}
