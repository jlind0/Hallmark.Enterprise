using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HallData.Utilities;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// Represents a filter context for a query
	/// </summary>
	/// <example>
	/// <code>
	/// FilterContext filter = FilterContext.CreateContext().Equals("Name", "Blah").Like("Bar.Name", "%Blah%");
	/// </code>
	/// </example>
	[JsonObject]
	public class FilterContext : IEnumerable<FilterDescriptor>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public FilterContext()
		{
			Filters = new List<FilterDescriptor>();
		}

		/// <summary>
		/// The filters in the context
		/// </summary>
		[JsonProperty]
		public List<FilterDescriptor> Filters { get; set; }

		/// <summary>
		/// A raw search string to filter against
		/// </summary>
		[JsonProperty]
		public string SearchCriteria { get; set; }

		/// <summary>
		/// Fluent filter syntax
		/// </summary>
		/// <param name="property">Target property</param>
		/// <param name="compareValue">Target compare value</param>
		/// <param name="operation">Target operation</param>
		/// <returns>This filter context</returns>
		public FilterContext Filter(string property, object compareValue, FilterOperation operation = FilterOperation.Like)
		{
			Filters.Add(new FilterDescriptor(property, compareValue, operation));
			return this;
		}

		/// <summary>
		/// Fluent filter syntax for <see cref="FilterOperation.Like"/>
		/// </summary>
		/// <param name="property">Target property</param>
		/// <param name="compareValue">Target compare value</param>
		/// <returns>This filter context</returns>
		public FilterContext Like(string property, object compareValue)
		{
			return Filter(property, compareValue);
		}

		/// <summary>
		/// Fluent filter syntax for <see cref="FilterOperation.Equals"/>
		/// </summary>
		/// <param name="property">Target property</param>
		/// <param name="compareValue">Target compare value</param>
		/// <returns>This filter context</returns>
		public FilterContext Equals(string property, object compareValue)
		{
			return Filter(property, compareValue, FilterOperation.Equals);
		}

		/// <summary>
		/// Gets the enumerator
		/// </summary>
		/// <returns>The <see cref="FilterDescriptor"/>s</returns>
		public IEnumerator<FilterDescriptor> GetEnumerator()
		{
			foreach (var filter in Filters)
			{
				yield return filter;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Returns a JSON represention of the FilterContext
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString()
		{
			return "\"Filters\":[" + string.Join(",", Filters.Select(t => t.ToString()).ToArray()) + "]";
		}

		/// <summary>
		/// Creates a new FilterContext
		/// </summary>
		/// <returns>New FilterContext</returns>
		public static FilterContext CreateContext()
		{
			return new FilterContext();
		}
	}

	/// <summary>
	/// A strongly typed FilterContext
	/// </summary>
	/// <typeparam name="TView">The view being filtered</typeparam>
	/// <example>
	/// <code>
	/// FilterContext&lt;FooResult&gt; filter = FilterContext&lt;FooResult&gt;.CreateContext().Equals(f => f.Name, "Blah").Like(f => f.Bar.Name, "%Blah%");
	/// </code>
	/// </example>
	[JsonObject]
	public class FilterContext<TView> : FilterContext
	{
		/// <summary>
		/// Fluent filter method
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <param name="compareValue">Target compare value</param>
		/// <param name="operation">Target operation</param>
		/// <returns>This filter context</returns>
		public FilterContext<TView> Filter(Expression<Func<TView, dynamic>> propertyExpression, object compareValue, FilterOperation operation = FilterOperation.Like)
		{
			var path = propertyExpression.GetPropertyPath();
			if (string.IsNullOrEmpty(path))
				throw new ArgumentException("propertyExpression must be a member path");
			return (FilterContext<TView>)Filter(path, compareValue, operation);
		}

		/// <summary>
		/// Fluent filter method for <see cref="FilterOperation.Like"/>
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <param name="compareValue">Target compare value</param>
		/// <returns>This filter context</returns>
		public FilterContext<TView> Like(Expression<Func<TView, dynamic>> propertyExpression, object compareValue)
		{
			return Filter(propertyExpression, compareValue);
		}

		/// <summary>
		/// Fluent filter method for <see cref="FilterOperation.Equals"/>
		/// </summary>
		/// <param name="propertyExpression">Member expression</param>
		/// <param name="compareValue">Target compare value</param>
		/// <returns>This filter context</returns>
		public FilterContext<TView> Equals(Expression<Func<TView, dynamic>> propertyExpression, object compareValue)
		{
			return Filter(propertyExpression, compareValue, FilterOperation.Equals);
		}

		/// <summary>
		/// Creates a strongly typed context
		/// </summary>
		/// <returns>New Filter Context</returns>
		public new static FilterContext<TView> CreateContext()
		{
			return new FilterContext<TView>();
		}
	}

	/// <summary>
	/// Describes a filter
	/// </summary>
	[JsonObject]
	public sealed class FilterDescriptor
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public FilterDescriptor() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="property"><see cref="FilterDescriptor.Property"/></param>
		/// <param name="compareValue"><see cref="FilterDescriptor.CompareValue"/></param>
		/// <param name="operation"><see cref="FilterDescriptor.Operation"/></param>
		public FilterDescriptor(string property, object compareValue, FilterOperation operation = FilterOperation.Like)
		{
			this.Property = property;
			this.CompareValue = compareValue;
			this.Operation = operation;
		}

		/// <summary>
		/// The target property
		/// </summary>
		[JsonProperty]
		public string Property { get; set; }

		/// <summary>
		/// The target compare value
		/// </summary>
		[JsonProperty]
		public object CompareValue { get; set; }

		/// <summary>
		/// The target operation
		/// </summary>
		[JsonProperty]
		public FilterOperation Operation { get; set; }

		/// <summary>
		/// Returns JSON formated represenation of the FilterDescriptor
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString()
		{
			return "{" + string.Format("\"Property\":\"{0}\", \"CompareValue\":\"{1}\", \"Operation\":\"{2}\"", Property.Replace("\"", "&quot"), CompareValue.ToString().Replace("\"", "&quot;"), (int)Operation) + "}";
		}
	}

	/// <summary>
	/// Filter operations
	/// </summary>
	public enum FilterOperation
	{
		/// <summary>
		/// Equals operation
		/// </summary>
		Equals,
		/// <summary>
		/// Like operation
		/// </summary>
		Like
	}
}
