using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HallData.Data;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using HallData.ApplicationViews;
using System.Data.Common;

namespace HallData.Repository
{
    /// <summary>
    /// Base class for QueryDescriptor, used to populate SqlQueryDescriptor UDT
    /// </summary>
    /// <typeparam name="TFilter">Filter Context</typeparam>
    /// <typeparam name="TSort">Sort Context</typeparam>
    public abstract class QueryDescriptorBase<TFilter, TSort>
        where TFilter : FilterContext
        where TSort: SortContext
    {
        /// <summary>
        /// Factory constructor
        /// </summary>
        /// <param name="filter"><see cref="QueryDescriptorBase{TFilter,TSort}.Filter"/></param>
        /// <param name="sort"><see cref="QueryDescriptorBase{TFilter,TSort}.Sort"/></param>
        /// <param name="page"><see cref="QueryDescriptorBase{TFilter,TSort}.Page"/></param>
        public QueryDescriptorBase(TFilter filter = null, TSort sort = null, PageDescriptor page = null)
        {
            this.Filter = filter;
            this.Sort = sort;
            this.Page = page;
        }
        /// <summary>
        /// The Filter Context for the Query
        /// </summary>
        public TFilter Filter { get; private set; }
        /// <summary>
        /// The Sort Context for the Query
        /// </summary>
        public TSort Sort { get; private set; }
        /// <summary>
        /// The PageDescriptor for the Query
        /// </summary>
        public PageDescriptor Page { get; private set; }
        /// <summary>
        /// Returns JSON for the query descriptor
        /// </summary>
        /// <returns>JSON</returns>
        public override string ToString()
        {
            List<string> arguments = new List<string>();
            if (Filter != null)
                arguments.Add(Filter.ToString());
            if (Sort != null)
                arguments.Add(Sort.ToString());
            if (Page != null)
                arguments.Add(Page.ToString());
            return "{" + string.Join(",", arguments) + "}";
        }
        /// <summary>
        /// Adds the query descriptor to a command
        /// </summary>
        /// <param name="cmd">Target command</param>
        /// <param name="parameterName">Parameter name of the query descriptor</param>
        /// <param name="searchCriteriaParameterName">Parameter name of the searchCriteria</param>
        public void AddInCommand(DbCommand cmd, string parameterName = "queryDescriptor", string searchCriteriaParameterName = "searchCriteria")
        {
            cmd.AddParameter(parameterName, this.ToString());
            if(this.Filter != null && !string.IsNullOrWhiteSpace(this.Filter.SearchCriteria))
                cmd.AddParameter(searchCriteriaParameterName, this.Filter.SearchCriteria);
        }
    }
    /// <summary>
    /// QueryDescriptorBase implemention for dynamically typed Filter and Sort Contexts
    /// </summary>
    public sealed class QueryDescriptor : QueryDescriptorBase<FilterContext, SortContext>
    {
        /// <summary>
        /// Factory constructor
        /// </summary>
        /// <param name="filter"><see cref="QueryDescriptorBase{TFilter,TSort}.Filter"/></param>
        /// <param name="sort"><see cref="QueryDescriptorBase{TFilter,TSort}.Sort"/></param>
        /// <param name="page"><see cref="QueryDescriptorBase{TFilter,TSort}.Page"/></param>
        public QueryDescriptor(FilterContext filter = null, SortContext sort = null, PageDescriptor page = null) : base(filter, sort, page) { }
    }
    /// <summary>
    /// QueryDescriptorBase implemention for strongly typed Filter and Sort Contexts
    /// </summary>
    /// <typeparam name="TView">The View Type</typeparam>
    public sealed class QueryDescriptor<TView> : QueryDescriptorBase<FilterContext<TView>, SortContext<TView>>
    {
        /// <summary>
        /// Factory constructor
        /// </summary>
        /// <param name="filter"><see cref="QueryDescriptorBase{TFilter,TSort}.Filter"/></param>
        /// <param name="sort"><see cref="QueryDescriptorBase{TFilter,TSort}.Sort"/></param>
        /// <param name="page"><see cref="QueryDescriptorBase{TFilter,TSort}.Page"/></param>
        public QueryDescriptor(FilterContext<TView> filter = null, SortContext<TView> sort = null, PageDescriptor page = null) : base(filter, sort, page) { }
    }
}
