using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HallData.Admin.ApplicationViews
{
	public class Filter
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? DelayFilter { get; set; }
	}

	public class Filter<TFilterType, TFilterOption, TFilterOptionCollection, TTemplate, TColumn> : Filter
		where TFilterOption: FilterOperationOptionKey
		where TFilterOptionCollection : IEnumerable<TFilterOption>
		where TTemplate: TemplateKey
		where TColumn: ApplicationViewColumnKey
		where TFilterType: FilterTypeKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildViewCollection(parmType: CollectionParameterType.Xml)]
		public TFilterOptionCollection OperationOptions { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TFilterOption InitialOperationOption { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TColumn FilterColumn { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TFilterType FilterType { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TTemplate Template { get; set; }
	}

	public class FilterForAddUpdate : Filter<FilterTypeKey, FilterOperationOptionKey, List<FilterOperationOptionKey>, TemplateKey, ApplicationViewColumnKey> { }

	public class FilterResult : Filter<FilterTypeResult, FilterOperationOptionResult, List<FilterOperationOptionResult>, TemplateResult, ApplicationViewColumnResult> { }
}
