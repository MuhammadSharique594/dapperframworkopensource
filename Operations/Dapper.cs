using DapperFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DapperFramework.Exception;

namespace DapperFramework.Operations
{
    public static class Dapper
    {
        private static ICrud _iCrud;

        static Dapper()
        {
            _iCrud = new Crud();
        }

        public static string SetConnectionString {
            set
            {
                _iCrud.ConnectionString = value; 
            }
        }

        public static void AddRow<T>(T obj)
        {
			_iCrud.Add(obj);
        }

	    public static void AddRows<T>(IEnumerable<T> obj)
	    {
			_iCrud.Add(obj);
	    }

		public static IEnumerable<T> SelectWhere<T>(Expression<Predicate<T>> expression)
        {
		    CheckConnectionString();
            return _iCrud.SelectWhere(expression);
		}

        public static IEnumerable<T> SelectAll<T>()
        {
		    CheckConnectionString();
            return _iCrud.SelectAll<T>();
		}

        public static T SelectFirstOrDefault<T>(Expression<Predicate<T>> expression)
        {
		    CheckConnectionString();
            return _iCrud.SelectFirstOrDefault(expression);
		}

        public static void Delete<T>(Expression<Predicate<T>> expression)
        {
            _iCrud.Delete(expression);
		}

	    public static void Update<T>(T obj, Expression<Predicate<T>> expression, params Expression<Func<T, object>>[] properties)
		{
		    _iCrud.Update(obj, expression, properties);
	    }

	    public static void Commit()
	    {
		    CheckConnectionString();
			_iCrud.Commit();
	    }

		private static void CheckConnectionString()
		{
			if (string.IsNullOrEmpty(_iCrud.ConnectionString))
			{
				throw new DapperException("Connection string is not defined!");
			}
		}
	}
}
