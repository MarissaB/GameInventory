using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GameInventory;

/// <summary>
/// Summary description for Repository
/// </summary>
public class Repository<T> : IRepository<T>
    where T : class
    
{
    public Repository()
    {
        this._dbContext = new GamingEntities();
        this._dbSet = this._dbContext.Set<T>();
    }

    public virtual int Count
    {
        get { return _dbSet.Count(); }
    }

    public virtual IQueryable<T> All()
    {
        return _dbSet.AsQueryable();
    }

    public virtual T GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!String.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return orderBy(query).AsQueryable();
        }
        else
        {
            return query.AsQueryable();
        }
    }

    public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsQueryable();
    }

    public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50)
    {
        int skipCount = index * size;
        var resetSet = filter != null ? _dbSet.Where(filter).AsQueryable() : _dbSet.AsQueryable();
        resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
        total = resetSet.Count();
        return resetSet.AsQueryable();
    }

    public bool Contains(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Count(predicate) > 0;
    }

    public virtual T Find(params object[] keys)
    {
        return _dbSet.Find(keys);
    }

    public virtual T Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public virtual T Insert(T entity)
    {
        var newEntry = _dbSet.Add(entity);
        return newEntry;
    }

    public virtual void Delete(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    public virtual void Delete(Expression<Func<T, bool>> predicate)
    {
        var entitiesToDelete = Filter(predicate);
        foreach (var entity in entitiesToDelete)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }

    public virtual bool Update(T entity)
    {
        var entry = _dbContext.Entry(entity);
        var e = _dbSet.Attach(entity);
        entry.State = EntityState.Modified;
        //SaveChanges();
        return (e != null);
    }

    public int SaveChanges()
    {
        return this._dbContext.SaveChanges();
    }

    #region Private

    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    #endregion
}
