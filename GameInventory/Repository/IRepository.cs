using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;

public interface IRepository<T>
{
    /// <summary>
    ///   Get the total objects count.
    /// </summary>
    int Count { get; }

    /// <summary>
    ///   Gets all objects from database
    /// </summary>
    IQueryable<T> All();

    /// <summary>
    ///   Gets object by primary key.
    /// </summary>
    /// <param name="id"> primary key </param>
    /// <returns> </returns>
    T GetById(object id);

    /// <summary>
    ///   Gets objects via optional filter, sort order, and includes
    /// </summary>
    /// <param name="filter"> </param>
    /// <param name="orderBy"> </param>
    /// <param name="includeProperties"> </param>
    /// <returns> </returns>
    IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

    /// <summary>
    ///   Gets objects from database by filter.
    /// </summary>
    /// <param name="predicate"> Specified a filter </param>
    IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

    /// <summary>
    ///   Gets objects from database with filting and paging.
    /// </summary>
    /// <param name="filter"> Specified a filter </param>
    /// <param name="total"> Returns the total records count of the filter. </param>
    /// <param name="index"> Specified the page index. </param>
    /// <param name="size"> Specified the page size </param>
    IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);

    /// <summary>
    ///   Gets the object(s) is exists in database by specified filter.
    /// </summary>
    /// <param name="predicate"> Specified the filter expression </param>
    bool Contains(Expression<Func<T, bool>> predicate);

    /// <summary>
    ///   Find object by keys.
    /// </summary>
    /// <param name="keys"> Specified the search keys. </param>
    T Find(params object[] keys);

    /// <summary>
    ///   Find object by specified expression.
    /// </summary>
    /// <param name="predicate"> </param>
    T Find(Expression<Func<T, bool>> predicate);

    /// <summary>
    ///   Insert a new object to database.
    /// </summary>
    /// <param name="entity"> Specified a new object to create. </param>
    T Insert(T entity);

    /// <summary>
    ///   Deletes the object by primary key
    /// </summary>
    /// <param name="id"> </param>
    void Delete(object id);

    /// <summary>
    ///   Delete the object from database.
    /// </summary>
    /// <param name="entity"> Specified a existing object to delete. </param>
    void Delete(T entity);

    /// <summary>
    ///   Delete objects from database by specified filter expression.
    /// </summary>
    /// <param name="predicate"> </param>
    void Delete(Expression<Func<T, bool>> predicate);

    /// <summary>
    ///   Update object changes and save to database.
    /// </summary>
    /// <param name="entity"> Specified the object to save. </param>
    bool Update(T entity);

    /// <summary>
    /// Validate object
    /// </summary>
    /// <param name="entity"> Specified the object to validate. </param>
    //IValidationResult Validate(T entity);

    /// <summary>
    /// Save any changes to the database
    /// </summary>
    /// <returns></returns>
    int SaveChanges();
}