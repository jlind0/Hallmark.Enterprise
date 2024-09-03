using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews.Results
{
	public interface ICategoryKey : IHasKey
	{
		int? Id { get; set; }
	}

	public interface ICategory<TCategoryType, TParentCategory, TStatusType> : ICategory<TCategoryType, TParentCategory>
		where TCategoryType : CategoryTypeKey
		where TParentCategory : CategoryKey
		where TStatusType : StatusTypeKey
	{
		TStatusType Status { get; set; }
	}

	public interface ICategory<TCategoryType, TParentCategory> : ICategoryKey
		where TParentCategory : CategoryKey
	{
		TParentCategory ParentCategory { get; set; }
		TCategoryType CategoryType { get; set; }
		string Name { get; set; }
		int Level { get; set; }
		string Path { get; set; }
		int OrderIndex { get; set; }
		bool? IsRoleRequired { get; set; }
		bool? IsDynamic { get; set; }
	}

	public interface ICategoryForAdd : ICategory<CategoryTypeKey, CategoryKey, StatusTypeKey> 
	{ }

	public interface ICategoryForUpdate : ICategory<CategoryTypeKey, CategoryKey> 
	{ }

	public interface ICategoryResult : ICategory<CategoryTypeResult, CategoryResult, StatusTypeResult> 
	{ }
}
