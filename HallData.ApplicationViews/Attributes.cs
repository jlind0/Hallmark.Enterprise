using System;

namespace HallData.ApplicationViews
{
	/// <summary>
	/// Operations that can be perfomed on a view
	/// </summary>
	public enum ViewOperations
	{
		/// <summary>
		/// Add Operation
		/// </summary>
		Add,
		/// <summary>
		/// Update Operation
		/// </summary>
		Update,
		/// <summary>
		/// Child Key Operation
		/// </summary>
		ChildKey
	}
	
	/// <summary>
	/// Base attribute to mark a property for the data layer to add as a parameter for an operation
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
	public class ViewOperationParameterAttribute : Attribute
	{
		/// <summary>
		/// The operation to populate the target property as a parameter
		/// </summary>
		public ViewOperations Operation { get; private set; }
		/// <summary>
		/// Instantiates a <see cref="ViewOperationParameterAttribute"/>
		/// </summary>
		/// <param name="operation"><see cref="ViewOperationParameterAttribute.Operation"/></param>
		public ViewOperationParameterAttribute(ViewOperations operation)
		{
			this.Operation = operation;
		}
	}

	/// <summary>
	/// Indicates that the property should be populated for Add operations
	/// <example>
	/// <code>
	/// public class FooView
	/// {
	///     [AddOperationParameter]
	///     public string Name{get;set;}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	public class AddOperationParameterAttribute : ViewOperationParameterAttribute
	{
		/// <summary>
		/// Default constructor for <see cref="AddOperationParameterAttribute"/>
		/// </summary>
		public AddOperationParameterAttribute() : base(ViewOperations.Add) { }
	}

	/// <summary>
	/// Indicates that the property should be populated for Update operations
	/// <example>
	/// <code>
	/// public class FooView
	/// {
	///     [UpdateOperationParameter]
	///     public string Name{get;set;}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	public class UpdateOperationParameterAttribute : ViewOperationParameterAttribute
	{
		/// <summary>
		/// Default constructor for <see cref="UpdateOperationParameterAttribute"/>
		/// </summary>
		public UpdateOperationParameterAttribute() : base(ViewOperations.Update) { }
	}

	/// <summary>
	/// Indicates that property should by populated for ChildView operations
	/// <remarks>Allows to mark a key to ignore on an add if the context of the add is that application view, but not if its a child application view.</remarks>
	/// <example>
	/// <code>
	/// public class FooView
	/// {
	///     [ChildKeyOperationParameter]
	///     public int? FooId{get;set;}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	public class ChildKeyOperationParameterAttribute : ViewOperationParameterAttribute
	{
		/// <summary>
		/// Default constructor for <see cref="ChildKeyOperationParameterAttribute"/>
		/// </summary>
		public ChildKeyOperationParameterAttribute() : base(ViewOperations.ChildKey) { }
	}

	/// <summary>
	/// Indicates that the property is a child view for mapping purposes
	/// <example>
	/// <code>
	/// public class FooView
	/// {
	///     [ChildView]
	///     public BarView Bar{get;set;}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class ChildViewAttribute: Attribute
	{

	}

	/// <summary>
	/// Indicates that the property should be populated as a DataTable during a data operation
	/// <example>
	/// <code>
	/// public class FooView
	/// {
	///     [UpdateOperationParameter]
	///     [AddOperationParameter]
	///     [ChildViewCollection("dbo.bartabletype")]
	///     public ICollection&lt;BarView&gt; Bars{get;set;}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false)]
	public class ChildViewCollectionAttribute: Attribute
	{
		/// <summary>
		/// The data table type name in SQL Server
		/// </summary>
		public string DataTableTypeName { get; private set; }
        public CollectionParameterType ParameterType { get; private set; }
		/// <summary>
		/// Constructor for <see cref="ChildViewCollectionAttribute"/>
		/// </summary>
		/// <param name="dataTableTypeName"><see cref="ChildViewCollectionAttribute.DataTableTypeName"/></param>
		public ChildViewCollectionAttribute(string dataTableTypeName = null, CollectionParameterType parmType = CollectionParameterType.DataTable)
		{
			this.DataTableTypeName = dataTableTypeName;
            this.ParameterType = parmType;
		}
	}
    public enum CollectionParameterType
    {
        DataTable,
        Xml
    }
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ChildViewMergeAttribute : Attribute
	{
		public string AddDataTableTypeName { get; set; }
		public string UpdateDataTableTypeName { get; set; }
		public string DeleteDataTableTypeName { get; set; }
        public CollectionParameterType ParameterType { get; private set; }
        public ChildViewMergeAttribute(CollectionParameterType parmType = CollectionParameterType.DataTable)
        {
            this.ParameterType = parmType;
        }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ChildViewRelationshipMergeAttribute : Attribute
	{
		public string DataTableTypeName { get; private set; }
        public CollectionParameterType ParameterType { get; private set; }
        public ChildViewRelationshipMergeAttribute(string dataTableTypeName = null, CollectionParameterType parmType = CollectionParameterType.Xml)
		{
			this.DataTableTypeName = dataTableTypeName;
            this.ParameterType = parmType;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class DefaultViewAttribute : Attribute
	{
		public string DefaultView { get; set; }
		public string DefaultViewSingle { get; set; }
		public string DefaultViewMany { get; set; }
		public DefaultViewAttribute(string defaultView, string defaultSingle = null, string defaultMany = null)
		{
			defaultMany = defaultMany ?? defaultView;
			defaultSingle = defaultSingle ?? defaultView;
			this.DefaultView = defaultView;
			this.DefaultViewSingle = defaultSingle;
			this.DefaultViewMany = defaultMany;
		}
	}
}
