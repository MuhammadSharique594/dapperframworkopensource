using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DapperFramework.Interfaces
{
	internal interface IQueryBuilder
	{
		string AddQuery<T>(T obj);

		string AddQuery<T>(IEnumerable<T> obj);

		string SelectWhere<T>(Expression<Predicate<T>> expression);

		string SelectAll<T>();

		string SelectFirstOrDefault<T>(Expression<Predicate<T>> expression);

		string Delete<T>(Expression<Predicate<T>> expression);

		string Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties);
	}
}
