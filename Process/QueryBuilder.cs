using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DapperFramework.Exception;
using DapperFramework.Interfaces;

namespace DapperFramework.Process
{
	internal class QueryBuilder : IQueryBuilder
	{
		string IQueryBuilder.AddQuery<T>(T obj)
		{
			var type = typeof(T);
			var tableName = type.Name;
			var properties = type.GetProperties();
			var identityProperties = Helper.Helper.GetIdentityProperties(properties);

			var query = new StringBuilder(Helper.Helper.GetInsertQueryInitialPart(properties, tableName, identityProperties));

			query.Remove(query.Length - 2, 2);
			query.Append(") Values (");

			query.Append(Helper.Helper.GetInsertQueryAnotherPart(obj, properties));

			query.Remove(query.Length - 2, 2);
			query.Append(");");

			return query.ToString();
		}

		string IQueryBuilder.AddQuery<T>(IEnumerable<T> obj)
		{
			if (obj == null)
			{
				throw new DapperException("There is no data for insertion!");
			}

			var type = typeof(T);
			var tableName = type.Name;
			var properties = type.GetProperties();
			var identityProperties = Helper.Helper.GetIdentityProperties(properties);

			var query = new StringBuilder(Helper.Helper.GetInsertQueryInitialPart(properties, tableName, identityProperties));

			query.Remove(query.Length - 2, 2);
			query.Append(") Values ");

			var enumerable = obj as T[] ?? obj.ToArray();


			for (var i = 0; i < enumerable.Count(); i++)
			{
				query.Append("(");

				foreach (var property in properties)
				{
					if (identityProperties != null && identityProperties.Contains(property.Name))
					{
						continue;
					}

					query.Append($"{Helper.Helper.GetValue(property.PropertyType.Name, property.GetValue(enumerable[i]))}, ");
				}
				query.Remove(query.Length - 2, 2);
				query.Append("), ");
			}

			query.Remove(query.Length - 2, 2);
			query.Append(";");

			return query.ToString();
		}

		string IQueryBuilder.SelectWhere<T>(Expression<Predicate<T>> expression)
		{
			var tableName = typeof(T).Name;

			var query = new StringBuilder($"Select * FROM {tableName} ").Append(Helper.Helper.ModifyQuery(expression.ToString()));

			return query.ToString();
		}

		string IQueryBuilder.SelectAll<T>()
		{
			var tableName = typeof(T).Name;

			var query = new StringBuilder($"Select * FROM {tableName};");

			return query.ToString();
		}

		string IQueryBuilder.SelectFirstOrDefault<T>(Expression<Predicate<T>> expression)
		{
			var tableName = typeof(T).Name;

			var query = new StringBuilder($"Select TOP 1 * FROM {tableName} ").Append(Helper.Helper.ModifyQuery(expression.ToString()));

			return query.ToString();
		}

		string IQueryBuilder.Delete<T>(Expression<Predicate<T>> expression)
		{
			var tableName = typeof(T).Name;

			var query = new StringBuilder($"Delete FROM {tableName} ").Append(Helper.Helper.ModifyQuery(Helper.Helper.ModifyQueryByRemovingAliasing(expression.ToString())));

			return query.ToString();
		}

		string IQueryBuilder.Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties)
		{
			var type = obj.GetType();
			var tableName = type.Name;
			var allProperties = type.GetProperties();

			var query = new StringBuilder($"UPDATE {tableName} SET ");

			if (allProperties.Length <= 0)
			{
				throw new DapperException($"Type [{tableName}] does not contain properties.");
			}

			var innerQuery = new StringBuilder();

			foreach (var property in properties)
			{
				if (property != null)
				{
					var propertyName = property.Body.ToString().Split('.')[1];

					if (allProperties.Select(x => x.Name).Contains(propertyName))
					{
						var prop = allProperties.Single(x => x.Name.Equals(propertyName));
						var proValue = prop.GetValue(obj);

						innerQuery.Append($"{propertyName} = {Helper.Helper.GetValue(prop.PropertyType.Name, proValue)}, ");
					}
				}
			}

			if (innerQuery.Length < 0)
			{
				throw new DapperException("Oops, anonymous exception occured! \nHint: Please check your models or update calling mechanism");
			}

			innerQuery.Remove(innerQuery.Length - 2, 2);
			query.Append(innerQuery).Append($"{Helper.Helper.ModifyQuery(Helper.Helper.ModifyQueryByRemovingAliasing(expression.ToString()))};");

			return query.ToString();
		}
	}
}
