using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DapperFramework.Interfaces
{
    internal interface ICrud
    {
        string ConnectionString { get; set; }
        void Add<T>(T obj);
	    void Add<T>(IEnumerable<T> obj);
		IEnumerable<T> SelectWhere<T>(Expression<Predicate<T>> expression);
        IEnumerable<T> SelectAll<T>();
        T SelectFirstOrDefault<T>(Expression<Predicate<T>> expression);
        void Delete<T>(Expression<Predicate<T>> expression);
	    void Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties);
	    void Commit();

    }
}
