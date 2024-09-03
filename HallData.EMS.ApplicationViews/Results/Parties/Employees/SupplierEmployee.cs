using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.Results
{
	
	/// <summary>
	/// Employee Key class
	/// </summary>
	public abstract class SupplierEmployeeKey<TKey> : EmployeeKey<TKey>, ISupplierEmployeeKey<TKey>
		where TKey: ISupplierEmployeeId
	{
	}
	public class SupplierEmployeeKey : SupplierEmployeeKey<SupplierEmployeeId>, ISupplierEmployeeKey
	{
		public override SupplierEmployeeId Key
		{
			get
			{
				return new SupplierEmployeeId(this.PartyGuid ?? Guid.Empty, this.EmployerGuid);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	public abstract class SupplierEmployeeWithKey<TKey, TPartyType, TTitleType> : EmployeeWithKey<TKey, TPartyType, TTitleType>, ISupplierEmployeeBase<TKey, TPartyType, TTitleType>
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
		where TKey: ISupplierEmployeeId
	{
		
	}
	public class SupplierEmployee<TPartyType, TTitleType> : SupplierEmployeeWithKey<SupplierEmployeeId, TPartyType, TTitleType>, ISupplierEmployee<TPartyType, TTitleType>
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
	{
		public override SupplierEmployeeId Key
		{
			get
			{
				return new SupplierEmployeeId(this.PartyGuid ?? Guid.Empty, this.EmployerGuid);
			}
			set
			{
				this.PartyGuid = value.PartyGuid;
				this.EmployerGuid = value.EmployerGuid;
			}
		}
	}
	/// <summary>
	/// Employee Base class with restriction by Title, Party and Status type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTitleType">Title type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	public class SupplierEmployee<TPartyType, TTitleType, TStatusType> : SupplierEmployee<TPartyType, TTitleType>, ISupplierEmployee<TPartyType, TTitleType, TStatusType>
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
		where TStatusType : StatusTypeKey
	{
		/// <summary>
		/// Employee Relationship Status
		/// </summary>
		/// <remarks>Status of the Employee relationship</remarks>
		[AddOperationParameter]
		[ChildView]
		public TStatusType EmployeeRelationshipStatus { get; set; }
		/// <summary>
		///Employee status type
		/// </summary>
		[AddOperationParameter]
		[ChildView]
		public TStatusType Status { get; set; }
	}
	/// <summary>
	/// Employee Base class with restriction by Title, Party, Status and Organization type 
	/// </summary>
	/// <typeparam name="TPartyType">Party type</typeparam>
	/// <typeparam name="TTitleType">Title type</typeparam>
	/// <typeparam name="TStatusType">Status type</typeparam>
	/// <typeparam name="TOrganization">Organization type</typeparam>
	public class SupplierEmployee<TPartyType, TTitleType, TStatusType, TOrganization> : SupplierEmployee<TPartyType, TTitleType, TStatusType>, ISupplierEmployee<TPartyType, TTitleType, TStatusType, TOrganization>
		where TStatusType : StatusTypeKey
		where TPartyType : PartyTypeKey
		where TTitleType : TitleTypeKey
		where TOrganization : ISupplierKey
	{
		/// <summary>
		/// Organization Employer
		/// </summary>
		[ChildView]
		public TOrganization Employer { get; set; }
	}
	/// <summary>
	/// Employee Result 
	/// </summary>
	public class SupplierEmployeeResult : SupplierEmployee<PartyTypeResult, TitleTypeName, StatusTypeResult, SupplierResult>, ISupplierEmployeeResult
	{
		public PrimaryPartyAddress PrimaryAddress { get; set; }

		public PrimaryPartyPhone PrimaryPhone { get; set; }

		public PrimaryPartyEmail PrimaryEmail { get; set; }
	}
}
