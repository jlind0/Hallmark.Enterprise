using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using HallData.ApplicationViews;
using HallData.Utilities;
using System.Data.Common;

namespace HallData.Data
{
    /// <summary>
    /// <see cref="DbDataReader"/> extensions
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Map's a field using a member expression
        /// </summary>
        /// <typeparam name="T">View type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dr">Data Reader</param>
        /// <param name="view">The view to populate</param>
        /// <param name="expression">Member expression</param>
        /// <param name="converter">Type converter, from datareader result to model</param>
        /// <exception cref="ArgumentException">Expression must be a member expression</exception>
        /// <example>
        /// <code>
        /// public Foo MapFoo(DbDataReader dr)
        /// {
        ///     Foo foo = new Foo();
        ///     dr.MapField(foo, f => f.Id);
        ///     dr.MapField(foo, f => f.Name);
        ///     return foo;
        /// }
        /// </code>
        /// </example>
        public static void MapField<T, TValue>(this DbDataReader dr, T view, Expression<Func<T, TValue>> expression, Func<object, TValue> converter = null)
        {
            MemberExpression me = expression.GetMemberExpression();
            var path = expression.GetPropertyPath();
            if (me == null)
                throw new ArgumentException("Expression must be a member expression");
            if (converter == null)
                converter = obj =>
                {
                    if (obj == null || obj == DBNull.Value)
                        return default(TValue);
                    return (TValue)obj;
                };
            Type type = typeof(T);
            PropertyInfo property = null;
            object target = view;
            Stack<MemberExpression> expressions = new Stack<MemberExpression>();
            expressions.Push(me);
            while(me != null)
            {
                me = me.Expression.GetMemberExpression();
                if (me != null)
                    expressions.Push(me);
            }
            while(expressions.Count > 1)
            {
                me = expressions.Pop();
                var prop = type.GetProperty(me.Member.Name);
                var obj = prop.GetValue(target);
                if (obj == null)
                {
                    obj = Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(target, obj);
                }
                target = obj;
                type = prop.PropertyType;
                property = prop;
            }
            me = expressions.Pop();
            property = type.GetProperty(me.Member.Name);
            if (!dr.HasColumn(path))
                property.SetValue(target, converter(null));
            else
                property.SetValue(target, converter(dr[path]));
        }
        /// <summary>
        /// Checks for the existance of a column by name
        /// </summary>
        /// <param name="dr">Data Reader</param>
        /// <param name="columnName">Target column name</param>
        /// <returns>Indication of if the column exists</returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Maps a parameter from a view to a <see cref="DbCommand"/> using a member expression and converter
        /// </summary>
        /// <typeparam name="T">Type of view</typeparam>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <param name="cmd">Target command</param>
        /// <param name="view">The view</param>
        /// <param name="expression">Member expression</param>
        /// <param name="converter">Type converter for converting view values to command parameter values</param>
        /// <returns>Created Parameter</returns>
        /// <exception cref="ArgumentException">Expression must be a member expression</exception>
        public static DbParameter MapParameter<T, TValue>(this DbCommand cmd, T view, Expression<Func<T, TValue>> expression, Func<TValue, object> converter = null)
        {
            MemberExpression me = expression.GetMemberExpression();
            var path = expression.GetPropertyPath().Split('.');
            if (me == null)
                throw new ArgumentException("Expression must be a member expression");
            var property = typeof(T).GetProperty(me.Member.Name);
            var value = property.GetValue(view);
            int pathIndex = 0;
            while(me != null && value != null && pathIndex < path.Length)
            {
                me = me.Expression.GetMemberExpression();
                if(me != null)
                {
                    property = property.PropertyType.GetProperty(me.Member.Name);
                    value = property.GetValue(value);
                    pathIndex++;
                }
            }
            if (pathIndex == path.Length - 1)
            {
                TValue tValue = value != null ? (TValue)value : default(TValue);
                if (converter == null)
                    converter = obj => obj;
                var o = converter(tValue);
                if (o == null)
                    return null;
                return cmd.AddParameter(string.Join("_", path), o);
            }
            return null;
        }
        /// <summary>
        /// Populates columns resturned by a data reader, will only populate if columns parameter is null
        /// </summary>
        /// <param name="dr">The data reader</param>
        /// <param name="columns">A List of Columns in the data reader, pass in as null to execute method</param>
        public static void PopulateColumns(this IDataRecord dr, ref List<string> columns)
        {
            if (columns == null)
            {
                columns = new List<string>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    columns.Add(dr.GetName(i));
                }
            }
        }
        /// <summary>
        /// Maps a data reader to an instance of a type
        /// </summary>
        /// <typeparam name="T">Target Type</typeparam>
        /// <param name="dr">The data reader</param>
        /// <param name="columns">The columns to map</param>
        /// <returns>An instance of the type, null if all children are null</returns>
        public static T Map<T>(this DbDataReader dr, IEnumerable<string> columns)
        {
            bool created;
            var obj = dr.Map(typeof(T), columns.Select(c => new Tuple<string[], string>(c.Split('|').Last().Replace("#", "").Split('.'), c)), out created);
            if (obj == null)
                return default(T);
            return (T)obj;
        }
        private static object Map(this DbDataReader dr, Type type, IEnumerable<Tuple<string[], string>> columns, out bool created, int level = 1)
        {
            created = false;
            var obj = Activator.CreateInstance(type);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach(var col in columns.Where(c => c.Item1.Length == level))
            {
                var propertyName = col.Item1[level - 1];
                var property = properties.SingleOrDefault(p => p.Name.ToLower() == propertyName.ToLower());
                var val = dr[col.Item2];
                if (property != null && val != null && val != DBNull.Value)
                {
                    created = true;
                    property.SetValue(obj, val);
                }
            }
            foreach(var col in columns.Where(c => c.Item1.Length == level + 1))
            {
                var propertyName = col.Item1[level];
                var subObjProperty = properties.SingleOrDefault(p => p.Name.ToLower() == propertyName.ToLower());
                if (subObjProperty != null)
                {
                    bool subCreated;
                    var subObj = dr.Map(subObjProperty.PropertyType, columns, out subCreated, level + 2);
                    if (subCreated)
                        subObjProperty.SetValue(obj, obj);
                    created = subCreated || created;
                }
            }
            if(created)
                return obj;
            return null;
        }
    }
}
