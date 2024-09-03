using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using Newtonsoft.Json;
using HallData.Validation;

namespace HallData.Admin.ApplicationViews
{
	public class InterfaceKey : IHasKey
	{
		[UpdateOperationParameter]
		[ChildKeyOperationParameter]
		public virtual int? InterfaceId { get; set; }

		[JsonIgnore]
		public int Key
		{
			get
			{
				return this.InterfaceId ?? 0;
			}
			set
			{
				this.InterfaceId = value;
			}
		}
	}

	public class Interface<TRelatedInterface, TRelatedInterfaceCollection, TAttribute, TAttributeCollection> : InterfaceKey
		where TRelatedInterface: InterfaceKey
		where TRelatedInterfaceCollection: IEnumerable<TRelatedInterface>
		where TAttribute: InterfaceAttributeKey
		where TAttributeCollection: IEnumerable<TAttribute>
	{
		[GlobalizedRequired("ADMIN_INTERFACE_NAME_REQUIRED")]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Name { get; set; }

		[GlobalizedRequired("ADMIN_INTERFACE_DISPLAYNAME_REQUIRED")]
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string DisplayName { get; set; }

		public virtual TRelatedInterfaceCollection RelatedInterfaces { get; set; }
		public virtual TAttributeCollection Attributes { get; set; }
	}
	public class InterfaceForAdd : Interface<InterfaceKey, IList<InterfaceKey>, InterfaceAttributeForAdd, IList<InterfaceAttributeForAdd>>
	{
		public InterfaceForAdd()
		{
			this.RelatedInterfaces = new List<InterfaceKey>();
			this.Attributes = new List<InterfaceAttributeForAdd>();
		}

		[JsonIgnore]
		public override int? InterfaceId
		{
			get
			{
				return base.InterfaceId;
			}
			set
			{
				base.InterfaceId = value;
			}
		}
	}

	public class InterfaceForMerge : InterfaceKey, IRelationshipMergeable
	{
		public RelationshipMergeActions MergeAction { get; set; }

		[GlobalizedRequired("AMDIN_INTERFACE_INTERFACEID_REQUIRED")]
		public override int? InterfaceId
		{
			get
			{
				return base.InterfaceId;
			}
			set
			{
				base.InterfaceId = value;
			}
		}
	}

	public class InterfaceForUpdate : Interface<InterfaceForMerge, RelationshipMerge<InterfaceKey, InterfaceForMerge>, InterfaceAttributeForMerge, 
		Merge<InterfaceAttributeForAdd, InterfaceAttributeForUpdateBase, InterfaceAttributeKey, InterfaceAttributeForMerge>>
	{
		public InterfaceForUpdate()
		{
			this.RelatedInterfaces = new RelationshipMerge<InterfaceKey, InterfaceForMerge>();
			this.Attributes = new Merge<InterfaceAttributeForAdd, InterfaceAttributeForUpdateBase, InterfaceAttributeKey, InterfaceAttributeForMerge>();
		}

		[GlobalizedRequired("ADMIN_INTERFACE_INTERFACEID_REQUIRED")]
		public override int? InterfaceId
		{
			get
			{
				return base.InterfaceId;
			}
			set
			{
				base.InterfaceId = value;
			}
		}
	}

	[DefaultView("InterfaceResults")]
	public class InterfaceResult : Interface<InterfaceResult, IList<InterfaceResult>, InterfaceAttributeResult, IList<InterfaceAttributeResult>>
	{
		public InterfaceResult()
		{
			this.RelatedInterfaces = new List<InterfaceResult>();
			this.Attributes = new List<InterfaceAttributeResult>();
		}
	}
}
