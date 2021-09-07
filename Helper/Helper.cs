using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DapperFramework.Helper
{
    internal static class Helper
    {
        internal static string GetValue(string type, object value)
        {
            if (type.ToLower().Contains("string") || type.ToLower().Contains("datetime"))
            {
                return $"'{value}'";
            }

	        return value.ToString();
        }

        internal static string ModifyQuery(string query)
        {
            var and = ExpressionType.AndAlso.ToString();
            var or = ExpressionType.OrElse.ToString();

            return query.Replace("=>", CoreDictionary.Dictionary["=>"])
                        .Replace("==", CoreDictionary.Dictionary["=="])
                        .Replace("\"", CoreDictionary.Dictionary["\""])
                        .Replace(or, CoreDictionary.Dictionary[or])
                        .Replace(and, CoreDictionary.Dictionary[and]);
        }

	    internal static string ModifyQueryByRemovingAliasing(string modifyingExpression)
	    {
		    if (string.IsNullOrEmpty(modifyingExpression))
		    {
			    return string.Empty;
		    }

		    var varLength = modifyingExpression.IndexOf("=>", StringComparison.Ordinal);
		    var extraVar = modifyingExpression.Substring(0, varLength - 1);
		    modifyingExpression = modifyingExpression.Replace(extraVar + ".", string.Empty);

		    return modifyingExpression.Replace(extraVar, string.Empty);
		}

	    internal static string GetInsertQueryInitialPart(PropertyInfo[] properties, string tableName, string[] identityProperties)
	    {
		    var query = new StringBuilder($"INSERT INTO {tableName} (");

		    foreach (var property in properties)
		    {
			    var propertyName = property.Name;

			    if (identityProperties != null && identityProperties.Contains(propertyName))
			    {
				    continue;
			    }

			    query.Append($"{propertyName}, ");
		    }

		    return query.ToString();
	    }

	    internal static string GetInsertQueryAnotherPart<T>(T obj, PropertyInfo[] properties)
	    {
		    var query = new StringBuilder();

		    for (var i = 1; i < properties.Length; i++)
		    {
			    query.Append($"{GetValue(properties[i].PropertyType.Name, properties[i].GetValue(obj))}, ");
		    }

		    return query.ToString();
	    }

	    internal static string[] GetIdentityProperties(PropertyInfo[] properties)
	    {
		    var attributes = properties.Select(x => new {x.Name, Attribute = x.GetCustomAttribute<Attributes.IdentityAttribute>(true) }).Where(x => x.Attribute != null);
		    var names = attributes.Select(x => x.Name);

		    return names.ToArray();
	    }
	}

    internal class CoreDictionary
    {
        internal static Dictionary<string, string> Dictionary = new Dictionary<string, string>
        {
            { "=>", "WHERE"},
            { "=> ", "SET "},
            { "==", "="},
            { "\"", "'"},
            { ExpressionType.AndAlso.ToString(), "AND"},
            { ExpressionType.OrElse.ToString(), "OR"}
        };
    }
}
