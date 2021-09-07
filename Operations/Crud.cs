using Dapper;
using DapperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DapperFramework.Process;

namespace DapperFramework.Operations
{
    internal class Crud : ICrud
    {
	    private readonly StringBuilder _bulkQuery;

	    public Crud()
	    {
			_bulkQuery = new StringBuilder();
	    }

        public string ConnectionString { get; set; }

		IEnumerable<T> ICrud.SelectWhere<T>(Expression<Predicate<T>> expression)
        {
			using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(QueriesGenerator.SelectWhere(expression));
            }
        }

        IEnumerable<T> ICrud.SelectAll<T>()
        {
			using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(QueriesGenerator.SelectAll<T>());
            }
        }

        T ICrud.SelectFirstOrDefault<T>(Expression<Predicate<T>> expression)
        {
			using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var data = connection.Query<T>(QueriesGenerator.SelectFirstOrDefault(expression));
	            var enumerable = data as T[] ?? data.ToArray();

	            return enumerable.AsList().Count > 0 ? enumerable.AsList()[0] : default(T);
            }
        }

	    void ICrud.Add<T>(T obj)
	    {
		    _bulkQuery.Append(QueriesGenerator.AddQuery(obj));
	    }

	    void ICrud.Add<T>(IEnumerable<T> obj)
	    {
		    var enumerable = obj as T[] ?? obj.ToArray();
		    _bulkQuery.Append(QueriesGenerator.AddQuery(enumerable));
	    }

		void ICrud.Delete<T>(Expression<Predicate<T>> expression)
        {
	        _bulkQuery.Append(QueriesGenerator.Delete(expression));
        }
	
        void ICrud.Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties)
        {
	        _bulkQuery.Append(QueriesGenerator.Update(obj, expression, properties));
        }

	    public void Commit()
	    {
			using (var connection = new SqlConnection(ConnectionString))
		    {
			    connection.Open();
			    connection.Query(_bulkQuery.ToString());
		    }
		}
    }
}
