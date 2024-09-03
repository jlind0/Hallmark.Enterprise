using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HallData.Utilities;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// Sort Context for server side sorting
	/// </summary>
	/// <example>
	/// <code>
	/// SortContext sort = SortContext.CreateOrderBy("Name").OrderByDescending("Bar.Name");
	/// </code>
	/// </example>
	[JsonObject]
	public class SortContext : IEnumerable<SortDescriptor>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SortContext()
		{
			Sorts = new List<SortDescriptor>();
		}

		/// <summary>
		/// The sorts in the context
		/// </summary>
		[JsonProperty]
		public List<SortDescriptor> Sorts { get; set; }

		/// <summary>
		/// Fluent Order By Ascending method
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>This Sort Context</returns>
		public SortContext OrderBy(string propertyName)
		{
			Sorts.Add(new SortDescriptor(propertyName));
			return this;
		}

		/// <summary>
		/// Fluent Order By Descending method
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>This Sort Context</returns>
		public SortContext OrderByDescending(string propertyName)
		{
			Sorts.Add(new SortDescriptor(propertyName, SortDirection.Descending));
			return this;
		}

		/// <summary>
		/// Fluent create Sort Context with Order By Ascending
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>New Sort Context</returns>
		public static SortContext CreateOrderBy(string propertyName)
		{
			SortContext context = new SortContext();
			return context.OrderBy(propertyName);
		}

		/// <summary>
		/// Fluent create Sort Context with Order By Descending
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>New Sort Contact</returns>
		public static SortContext CreateOrderByDescending(string propertyName)
		{
			SortContext context = new SortContext();
			return context.OrderByDescending(propertyName);
		}

		/// <summary>
		/// Gets the enumerator of <see cref="SortDescriptor"/>s
		/// </summary>
		/// <returns>Sort Descriptors</returns>
		public IEnumerator<SortDescriptor> GetEnumerator()
		{
			foreach (var sort in Sorts)
			{
				yield return sort;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Returns JSON string representation of Sort Contact
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString()
		{
			return "\"Sorts\":[" + string.Join(",", Sorts.Select(s => s.ToString()).ToArray()) + "]";
		}
	}

	/// <summary>
	/// Strongly typed Sort Context
	/// </summary>
	/// <typeparam name="TView">The type of the view being sorted</typeparam>
	/// <example>
	/// <code>
	/// SortContext&lt;FooResult&gt; sort = SortContext&lt;FooResult&gt;.CreateOrderBy(f => f.Name).OrderByDescending(f => f.Bar.Name);
	/// </code>
	/// </example>
	[JsonObject]
	public class SortContext<TView> : SortContext
	{
		/// <summary>
		/// Fluent order by ascending method
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <returns>This Sort Context</returns>
		public SortContext<TView> OrderBy(Expression<Func<TView, dynamic>> propertyExpression)
		{
			string path = propertyExpression.GetPropertyPath();
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("propertyExpression must represent a member");
			Sorts.Add(new SortDescriptor(path));
			return this;
		}

		/// <summary>
		/// Fluent order by descending method
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <returns>This Sort Context</returns>
		public SortContext<TView> OrderByDescending(Expression<Func<TView, dynamic>> propertyExpression)
		{
			string path = propertyExpression.GetPropertyPath();
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("propertyExpression must represent a member");
			Sorts.Add(new SortDescriptor(path, SortDirection.Descending));
			return this;
		}

		/// <summary>
		/// Fluent create sort context with order by ascending
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <returns>New Sort Context</returns>
		public static SortContext<TView> CreateOrderBy(Expression<Func<TView, dynamic>> propertyExpression)
		{
			SortContext<TView> context = new SortContext<TView>();
			return context.OrderBy(propertyExpression);
		}

		/// <summary>
		/// Fluent create sort context with order by descending
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <returns>New sort context</returns>
		public static SortContext<TView> CreateOrderByDescending(Expression<Func<TView, dynamic>> propertyExpression)
		{
			SortContext<TView> context = new SortContext<TView>();
			return context.OrderByDescending(propertyExpression);
		}

		/// <summary>
		/// Fluent Order By Ascending method
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>This Sort Context</returns>
		public new SortContext<TView> OrderBy(string propertyName)
		{
			Sorts.Add(new SortDescriptor(propertyName));
			return this;
		}

		/// <summary>
		/// Fluent Order By Descending method
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>This Sort Context</returns>
		public new SortContext<TView> OrderByDescending(string propertyName)
		{
			Sorts.Add(new SortDescriptor(propertyName, SortDirection.Descending));
			return this;
		}

		/// <summary>
		/// Fluent create Sort Context with Order By Ascending
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>New Sort Context</returns>
		public static new SortContext<TView> CreateOrderBy(string propertyName)
		{
			SortContext<TView> context = new SortContext<TView>();
			return context.OrderBy(propertyName);
		}

		/// <summary>
		/// Fluent create Sort Context with Order By Descending
		/// </summary>
		/// <param name="propertyName">Target property name</param>
		/// <returns>New Sort Contact</returns>
		public static new SortContext<TView> CreateOrderByDescending(string propertyName)
		{
			SortContext<TView> context = new SortContext<TView>();
			return context.OrderByDescending(propertyName);
		}

		/// <summary>
		/// Gets the enumerator of <see cref="SortDescriptor"/>s
		/// </summary>
		/// <returns>Sort Descriptors</returns>
		public IEnumerator<SortDescriptor> GetEnumerator()
		{
			foreach (var sort in Sorts)
			{
				yield return sort;
			}
		}
	}

	/// <summary>
	/// Description of a sort
	/// </summary>
	[JsonObject]
	public sealed class SortDescriptor
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SortDescriptor() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="property"><see cref="SortDescriptor.Property"/></param>
		/// <param name="direction"><see cref="SortDescriptor.Direction"/></param>
		public SortDescriptor(string property, SortDirection direction = SortDirection.Ascending)
		{
			this.Property = property;
			this.Direction = direction;
		}

		/// <summary>
		/// Direction of the Sort
		/// </summary>
		[JsonProperty]
		public SortDirection Direction { get; private set; }

		/// <summary>
		/// Property to sort
		/// </summary>
		[JsonProperty]
		public string Property { get; private set; }

		/// <summary>
		/// Returns JSON for the SortDescriptor
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString()
		{
			return "{" + string.Format("\"Property\":\"{0}\", \"Direction\":\"{1}\"", Property.Replace("\"", "&quot;"), (int)Direction) + "}";
		}
	}

	/// <summary>
	/// Sort directions
	/// </summary>
	public enum SortDirection
	{
		/// <summary>
		/// Ascending
		/// </summary>
		Ascending,
		/// <summary>
		/// Descending
		/// </summary>
		Descending
	}
}
