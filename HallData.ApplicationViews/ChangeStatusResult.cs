using System.Collections.Generic;

namespace HallData.ApplicationViews
{
	
	/// <summary>
	/// Result class for status change actions
	/// </summary>
	public class ChangeStatusResult
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ChangeStatusResult() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="statusChanged"><see cref="StatusChanged"/></param>
		/// <param name="warningMessage"><see cref="WarningMessage"/></param>
		public ChangeStatusResult(bool statusChanged, string warningMessage = null)
		{
			this.StatusChanged = statusChanged;
			this.WarningMessage = warningMessage;
		}

		/// <summary>
		/// Indicates if the status changed as a result of the request
		/// </summary>
		public bool StatusChanged { get; set; }

		/// <summary>
		/// The warning if the status did not change
		/// </summary>
		public string WarningMessage { get; set; }
	}

	/// <summary>
	/// Holder class for a query result resuling from a status change
	/// </summary>
	/// <typeparam name="TResult">The result type</typeparam>
	public sealed class ChangeStatusQueryResult<TResult> : ChangeStatusResult, IQueryResult<TResult>
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ChangeStatusQueryResult() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="result"><see cref="Result"/></param>
		/// <param name="availableProperties"><see cref="AvailableProperties"/></param>
		/// <param name="statusChanged"><see cref="StatusChanged"/></param>
		/// <param name="warningMessage"><see cref="WarningMessage"/></param>
		public ChangeStatusQueryResult(TResult result, IEnumerable<string> availableProperties, bool statusChanged, string warningMessage) : base(statusChanged, warningMessage)
		{
			this.Result = result;
			this.AvailableProperties = availableProperties;
		}

		/// <summary>
		/// Factory constructor for an already populated <see cref="ChangeStatusResult"/>
		/// </summary>
		/// <param name="result"><see cref="Result"/></param>
		/// <param name="availableProperties"><see cref="AvailableProperties"/></param>
		/// <param name="changeStatusResult">An already populated <see cref="ChangeStatusResult"/></param>
		public ChangeStatusQueryResult(TResult result, IEnumerable<string> availableProperties, ChangeStatusResult changeStatusResult) :
			this(result, availableProperties, changeStatusResult.StatusChanged, changeStatusResult.WarningMessage) { }

		/// <summary>
		/// Factory constructor for an already populated <see cref="ChangeStatusResult"/> with no result
		/// </summary>
		/// <param name="changeStatusResult">An already populated <see cref="ChangeStatusResult"/></param>
		public ChangeStatusQueryResult(ChangeStatusResult changeStatusResult) : base(changeStatusResult.StatusChanged, changeStatusResult.WarningMessage) { }

		/// <summary>
		/// The result
		/// </summary>
		public TResult Result { get; set; }

		/// <summary>
		/// Available properties from the query
		/// </summary>
		public IEnumerable<string> AvailableProperties { get; set; }
	}
}
