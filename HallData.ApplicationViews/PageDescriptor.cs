using Newtonsoft.Json;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// Describes a page for server side paging
	/// </summary>
	[JsonObject]
	public sealed class PageDescriptor
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public PageDescriptor() { }

		/// <summary>
		/// Factory constructor
		/// </summary>
		/// <param name="pageSize"><see cref="PageDescriptor.PageSize"/></param>
		/// <param name="currentPage"><see cref="PageDescriptor.CurrentPage"/></param>
		public PageDescriptor(int pageSize, int currentPage)
		{
			this.PageSize = pageSize;
			this.CurrentPage = currentPage;
		}

		/// <summary>
		/// The Page Size, must be greater or equal to 1
		/// </summary>
		[JsonProperty]
		public int PageSize { get; set; }

		/// <summary>
		/// The current Page, 1 based
		/// </summary>
		[JsonProperty]
		public int CurrentPage { get; set; }

		/// <summary>
		/// Returns JSON for PageDescriptor
		/// </summary>
		/// <returns>JSON</returns>
		public override string ToString()
		{
			return "\"Page\":{" + string.Format("\"PageSize\":\"{0}\", \"CurrentPage\":\"{1}\"", this.PageSize, this.CurrentPage) + "}";
		}
	}
}
