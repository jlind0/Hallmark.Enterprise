using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// Holder class for query results
	/// </summary>
	/// <typeparam name="TResult">Type of result</typeparam>
	public sealed class QueryResults<TResult>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public QueryResults() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="totalResultsCount"><see cref="QueryResults{TResult}.TotalResultsCount"/></param>
		/// <param name="results"><see cref="QueryResults{TResult}.Results"/></param>
		/// <param name="availableProperties"><see cref="QueryResult{TResult}.AvailableProperties"/></param>
		public QueryResults(long totalResultsCount, IEnumerable<TResult> results, IEnumerable<string> availableProperties)
		{
			this.TotalResultsCount = totalResultsCount;
			this.Results = results;
			if(availableProperties != null)
				this.AvailableProperties = availableProperties.TransformAvailableProperties();
		}

		/// <summary>
		/// The total results returning in the query (ignoring paging)
		/// </summary>
		[Required]
		public long TotalResultsCount { get; set; }

		/// <summary>
		/// The results for the current page
		/// </summary>
		public IEnumerable<TResult> Results { get; set; }

		/// <summary>
		/// Available properties from the query
		/// </summary>
		[Required]
		public IEnumerable<string> AvailableProperties { get; set; }
	}

	/// <summary>
	/// Interface for a holder of a query result
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public interface IQueryResult<TResult>
	{
		/// <summary>
		/// The result
		/// </summary>
		TResult Result { get; set; }

		/// <summary>
		/// Available properties from the query
		/// </summary>
		IEnumerable<string> AvailableProperties { get; set; }
	}

	/// <summary>
	/// Holder class for a query result
	/// </summary>
	/// <typeparam name="TResult">Type of result</typeparam>
	public sealed class QueryResult<TResult> : IQueryResult<TResult>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public QueryResult() { }

		/// <summary>
		/// The result
		/// </summary>
		[Required]
		public TResult Result { get; set; }

		/// <summary>
		/// Available properties from the query
		/// </summary>
		[Required]
		public IEnumerable<string> AvailableProperties { get; set; }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="result"><see cref="QueryResult{TResult}.Result"/></param>
		/// <param name="availableProperties"><see cref="QueryResult{TResult}.AvailableProperties"/></param>
		public QueryResult(TResult result, IEnumerable<string> availableProperties)
		{
			this.Result = result;
			if(availableProperties != null)
				this.AvailableProperties = availableProperties.TransformAvailableProperties();
		}
	}
	
	/// <summary>
	/// Query results extensions class
	/// </summary>
	public static class QueryResultsHelper
	{
		/// <summary>
		/// Transforms pipe delimited property names to . delimeted
		/// </summary>
		/// <param name="properties">available properties</param>
		/// <returns>transformed available properties</returns>
		public static IEnumerable<string> TransformAvailableProperties(this IEnumerable<string> properties)
		{
			List<string> props = new List<string>();
			foreach (var p in properties)
			{
				string[] cols = p.Split('|');
				for (var i = 0; i < cols.Length - 1; i++)
				{
					cols[i] = cols[i].Remove(0, cols[i].LastIndexOf("$") + 1);
				}
				props.Add(string.Join(".", cols).Replace("#", ""));
			}
			return props;
		}
	}
}
