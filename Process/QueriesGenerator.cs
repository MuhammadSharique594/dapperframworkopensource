using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperFramework.Interfaces;

namespace DapperFramework.Process
{
	internal static class QueriesGenerator
	{
		private static readonly IQueryBuilder _queryBuilder;

		static QueriesGenerator()
		{
			_queryBuilder = new QueryBuilder();
		}

		internal static string AddQuery<T>(T obj)
		{
			return _queryBuilder.AddQuery(obj);
		}

		internal static string AddQuery<T>(IEnumerable<T> obj)
		{
			return _queryBuilder.AddQuery(obj);
		}

		internal static string SelectWhere<T>(Expression<Predicate<T>> expression)
		{
			return _queryBuilder.SelectWhere(expression);
		}

		internal static string SelectAll<T>()
		{
			return _queryBuilder.SelectAll<T>();
		}

		internal static string SelectFirstOrDefault<T>(Expression<Predicate<T>> expression)
		{
			return _queryBuilder.SelectFirstOrDefault(expression);
		}

		internal static string Delete<T>(Expression<Predicate<T>> expression)
		{
			return _queryBuilder.Delete(expression);
		}

		internal static string Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties)
		{
			return _queryBuilder.Update(obj, expression, properties);
		}
	}
}