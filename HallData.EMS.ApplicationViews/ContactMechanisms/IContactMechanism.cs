using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
	public interface IContactMechanismKey : IHasKey<Guid>
	{
		Guid? ContactMechanismGuid { get; set; }
	}

	public interface IContactMechanism<TMechanismType> : IContactMechanismKey
		where TMechanismType: MechanismTypeKey
	{
		TMechanismType MechanismType { get; set; }
	}

	public interface IPrimaryContactMechanism<TMechanismType> : IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		string ContactMechanismTypeName { get; set; }
	}

	public interface IPrimaryContactMechanismWithContactMechanismType<TMechanismType, TContactMechanismType> : IPrimaryContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
		where TContactMechanismType: ContactMechanismTypeKey
	{
		TContactMechanismType ContactMechanismType { get; set; }
	}

	public interface IContactMechanism<TMechanismType, TStatusType> : IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		TStatusType Status { get; set; }
	}

	public interface IPrimaryContactMechanism<TMechanismType, TStatusType> : IPrimaryContactMechanism<TMechanismType>, IContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
	{
		TStatusType PrimaryRelationshipStatus { get; set; }
	}

	public interface IPrimaryContactMechanism<TMechanismType, TStatusType, TContactMechanismType> : IPrimaryContactMechanism<TMechanismType, TStatusType>, IPrimaryContactMechanismWithContactMechanismType<TMechanismType, TContactMechanismType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey
		where TContactMechanismType: ContactMechanismTypeKey
	{ }

	public interface IContactMechanismForAdd : IContactMechanism<MechanismTypeKey, StatusTypeKey> { }
	
	public interface IPrimaryContactMechanismForAddUpdate : IPrimaryContactMechanism<MechanismTypeKey, StatusTypeKey>, IContactMechanismForAdd, IContactMechanismForUpdate { }
	
	public interface IContactMechanismForUpdate : IContactMechanism<MechanismTypeKey> { }
	
	public interface IContactMechanismResult : IContactMechanism<MechanismTypeResult, StatusTypeResult> { }
	
	public interface IPrimaryContactMechanismResult : IPrimaryContactMechanism<MechanismTypeResult, StatusTypeResult, ContactMechanismType>, IContactMechanismResult { }
	
	public interface IAddress<TMechanismType> : IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		string AddressLine1 { get; set; }
		string AddressLine2 { get; set; }
		string AddressLine3 { get; set; }
		string City { get; set; }
		string PostalCode { get; set; }
		string Country { get; set; }
	}

	public interface IPrimaryAddress<TMechanismType> : IAddress<TMechanismType>, IPrimaryContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey { }
	
	public interface IAddress<TMechanismType, TStatusType> : IAddress<TMechanismType>, IContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IPrimaryAddress<TMechanismType, TStatusType> : IAddress<TMechanismType, TStatusType>, IPrimaryContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType: StatusTypeKey{}
	
	public interface IAddressForAdd : IAddress<MechanismTypeKey, StatusTypeKey>, IContactMechanismForAdd { }
	
	public interface IAddressForUpdate : IAddress<MechanismTypeKey>, IContactMechanismForUpdate { }
	
	public interface IPrimaryAddressForAddUpdate : IPrimaryAddress<MechanismTypeKey, StatusTypeKey>, IAddressForAdd, IAddressForUpdate, IPrimaryContactMechanismForAddUpdate { }
	
	public interface IAddressResult : IAddress<MechanismTypeResult, StatusTypeResult>, IContactMechanismResult { }
	
	public interface IPrimaryAddressResult : IPrimaryAddress<MechanismTypeResult, StatusTypeResult>, IAddressResult, IPrimaryContactMechanismResult { }
	
	public interface IEmail<TMechanismType> : IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		string EmailAddress { get; set; }
	}
	
	public interface IPrimaryEmail<TMechanismType> : IEmail<TMechanismType>, IPrimaryContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey{ }
	
	public interface IEmail<TMechanismType, TStatusType> : IEmail<TMechanismType>, IContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IPrimaryEmail<TMechanismType, TStatusType> : IPrimaryEmail<TMechanismType>, IEmail<TMechanismType, TStatusType>, 
		IPrimaryContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IEmailForAdd : IEmail<MechanismTypeKey, StatusTypeKey>, IContactMechanismForAdd { }
	
	public interface IEmailForUpdate : IEmail<MechanismTypeKey>, IContactMechanismForUpdate { }
	
	public interface IPrimaryEmailForAddUpdate : IPrimaryEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryContactMechanismForAddUpdate, IEmailForAdd, IEmailForUpdate { }
	
	public interface IEmailResult : IEmail<MechanismTypeResult, StatusTypeResult>, IContactMechanismResult { }
	
	public interface IPrimaryEmailResult : IPrimaryEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryContactMechanismResult, IEmailResult { }
	
	public interface IPhone<TMechanismType> : IContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey
	{
		string PhoneNumber { get; set; }
	}
	
	public interface IPrimaryPhone<TMechanismType> : IPhone<TMechanismType>, IPrimaryContactMechanism<TMechanismType>
		where TMechanismType: MechanismTypeKey{ }
	
	public interface IPhone<TMechanismType, TStatusType> : IPhone<TMechanismType>, IContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IPrimaryPhone<TMechanismType, TStatusType> : IPrimaryPhone<TMechanismType>, IPhone<TMechanismType, TStatusType>, IPrimaryContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IPhoneForAdd : IPhone<MechanismTypeKey, StatusTypeKey>, IContactMechanismForAdd { }
	
	public interface IPhoneForUpdate : IPhone<MechanismTypeKey>, IContactMechanismForUpdate { }
	
	public interface IPrimaryPhoneForAddUpdate : IPrimaryPhone<MechanismTypeKey, StatusTypeKey>, IPhoneForAdd, IPhoneForUpdate, IPrimaryContactMechanismForAddUpdate { }
	
	public interface IPhoneResult : IPhone<MechanismTypeResult, StatusTypeResult>, IContactMechanismResult { }
	
	public interface IPrimaryPhoneResult : IPrimaryPhone<MechanismTypeResult, StatusTypeResult>, IPhoneResult, IPrimaryContactMechanismResult { }

	public interface IContactMechanismGeneric<TMechanismType> : IAddress<TMechanismType>, IPhone<TMechanismType>, IEmail<TMechanismType>
		where TMechanismType : MechanismTypeKey { }
	
	public interface IContactMechanismGeneric<TMechanismType, TStatusType> : IAddress<TMechanismType, TStatusType>, IPhone<TMechanismType, TStatusType>, IEmail<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey{ }
	
	public interface IContactMechanismGenericForAdd : IContactMechanismGeneric<MechanismTypeKey, StatusTypeKey>, IAddressForAdd, IPhoneForAdd, IEmailForAdd { }
	
	public interface IContactMechanismGenericForUpdate : IContactMechanismGeneric<MechanismTypeKey>, IAddressForUpdate, IPhoneForUpdate, IEmailForUpdate { }
	
	public interface IContactMechanismGenericResult : IContactMechanismGeneric<MechanismTypeResult, StatusTypeResult>, IAddressResult, IPhoneResult, IEmailResult { }
}
