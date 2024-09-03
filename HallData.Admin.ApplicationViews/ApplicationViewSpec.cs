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
	public class ApplicationViewSpec
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? InitialPageSize { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? InitialPage { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? HasSearchCriteria { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int? SearchCriteriaDelay { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
        public int? PageDisplayCount { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? HasPaging { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanSaveSettings { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? SortSingleColumnMode { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanReorderColumns { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanPickColumns { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildViewCollection(parmType: CollectionParameterType.Xml)]
		public List<PageOption> PageOptions { get; set; }
	}

	public class ApplicationViewSpec<TTemplate> : ApplicationViewSpec
		where TTemplate: TemplateKey
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TTemplate GridTemplate { get; set; }
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TTemplate PagerTemplate { get; set; }
		
	}

	public class ApplicationViewSpecForAddUpdate : ApplicationViewSpec<TemplateKey> { }

	public class ApplicationViewSpecResult : ApplicationViewSpec<TemplateResult> { }
}
