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
	public class ApplicationViewColumnSpec
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanSort { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? CanFilter { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string HeaderText { get; set; }
	}

	public class ApplicationViewColumnSpec<TSortOption, TFilter, TTemplate> : ApplicationViewColumnSpec
		where TSortOption: SortDirectionOptionKey
		where TFilter: Filter
		where TTemplate: TemplateKey
	{
		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TSortOption InitialSortDirectionOption { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TFilter Filter { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TTemplate ColumnTemplate { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TTemplate CellTemplate { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		[ChildView]
		public TTemplate HeaderTemplate { get; set; }
	}

	public class ApplicationViewColumnSpecForAddUpdate : ApplicationViewColumnSpec<SortDirectionOptionKey, FilterForAddUpdate, TemplateKey> { }

	public class ApplicationViewColumnSpecResult : ApplicationViewColumnSpec<SortDirectionOption, FilterResult, TemplateResult> { }
}
