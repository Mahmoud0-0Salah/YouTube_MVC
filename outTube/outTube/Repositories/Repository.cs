using OurTube.Repositories.Interfaces.Common;
using System.Linq.Expressions;
using outTube.Data;

namespace OurTube.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public bool Create(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<T> GetAll()
        {
            try
            {
                return context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
        }

        public List<T> GetByCondition(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return context.Set<T>().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<T>();
            }
        }

        public int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public bool Update(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
