using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Infrastructure.Repository.Shared;
using Infrastructure.Repository.Generic.Interface;


namespace Infrastructure.Repository.Generic.Concrete.Entity
{
    public abstract class GenericRepository<C, T> : IGenericRepository<T> where C : DbContext, new()
                                                          where T : class, new()
                                                           
    {
        protected C context;
        
        public GenericRepository()
        {
            context = SharedDBContext<C>.SharedContext();
            //context.Set<T>().RemoveRange(GetAll());
            //context.SaveChanges();
        }

        public C Context
        {
            set
            {
                if (context == null)
                    throw new NullReferenceException("Repository context must be non-null");
                context = value;
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public virtual void Add(T instance)
        {
            context.Set<T>().Add(instance);
        }

        public virtual void Modify(T instance)
        {
            context.Entry(instance).State = EntityState.Modified;
        }

        public virtual T FindFirst(Func<T, bool> filter)
        {
            return context.Set<T>().Where(filter).FirstOrDefault();
        }

        public virtual IEnumerable<T> FindAll(Func<T, bool> filter)
        {
            return context.Set<T>().Where(filter);
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }

        public virtual void Delete(T instance)
        {
            context.Set<T>().Remove(instance);
        }
    }
}
