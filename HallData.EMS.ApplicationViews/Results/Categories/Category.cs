using System;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	public class CategoryKey : ICategoryKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? Id { get; set; }

		[JsonIgnore]
		public int Key { get; set; }
	}

	public class Category<TCategoryType, TParentCategory> :
		CategoryKey, ICategory<TCategoryType, TParentCategory> 
		where TCategoryType : CategoryTypeKey
		where TParentCategory : CategoryKey
	{
		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TCategoryType CategoryType { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int Level { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Path { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public int OrderIndex { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsRoleRequired { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public bool? IsDynamic { get; set; }

		[ChildView]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public TParentCategory ParentCategory { get; set; }
	}

	public class Category<TCategoryType, TParentCategory, TStatusType> : Category<TCategoryType, TParentCategory>,
		ICategory<TCategoryType, TParentCategory, TStatusType>
		where TCategoryType : CategoryTypeKey
		where TParentCategory : CategoryKey
		where TStatusType : StatusTypeKey
	{
		[ChildView]
		[AddOperationParameter]
		public TStatusType Status { get; set; }

	}

	public class CategoryForAdd : Category<CategoryTypeKey, CategoryKey, StatusTypeKey>, ICategoryForAdd
	{
		[JsonIgnore]
		public override int? Id { get; set; }
	}

	public class CategoryForUpdate : Category<CategoryTypeKey, CategoryKey>,  ICategoryForUpdate
	{
		[GlobalizedRequired("CATEGORY_ID_REQUIRED")]
		public override int? Id { get; set; }
	}

	public class CategoryResult : Category<CategoryTypeResult, CategoryResult, StatusTypeResult>, ICategoryResult 
	{ }
}
