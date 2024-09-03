using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews
{
	public interface IPrimaryPartyContactMechanism : IPartyContactMechanism { }

	public interface IPrimaryPartyContactMechanism<TMechanismType> : IPrimaryContactMechanism<TMechanismType>, IPrimaryPartyContactMechanism
		where TMechanismType: MechanismTypeKey { }

	public interface IPrimaryPartyContactMechanism<TMechanismType, TStatusType> : IPrimaryPartyContactMechanism<TMechanismType>, IPrimaryContactMechanism<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }

	public interface IPrimaryPartyContactMechanismForAddUpdate : IPrimaryPartyContactMechanism<MechanismTypeKey, StatusTypeKey>, IPrimaryContactMechanismForAddUpdate { }
	
	public interface IPrimaryPartyContactMechanismResult : IPrimaryPartyContactMechanism<MechanismTypeResult, StatusTypeResult>, IPrimaryContactMechanismResult, IPartyContactMechanismResult { }

	public interface IPrimaryPartyAddress<TMechanismType> : IPrimaryPartyContactMechanism<TMechanismType>, IPrimaryAddress<TMechanismType>
		where TMechanismType: MechanismTypeKey { }

	public interface IPrimaryPartyAddress<TMechanismType, TStatusType> : IPrimaryPartyAddress<TMechanismType>, IPrimaryPartyContactMechanism<TMechanismType, TStatusType>, IPrimaryAddress<TMechanismType, TStatusType>
		where TMechanismType: MechanismTypeKey
		where TStatusType : StatusTypeKey { }

	public interface IPrimaryPartyAddressForAddUpdate : IPrimaryPartyAddress<MechanismTypeKey, StatusTypeKey>, IPrimaryAddressForAddUpdate, IPrimaryPartyContactMechanismForAddUpdate { }
	
	public interface IPrimaryPartyAddressResult : IPrimaryPartyAddress<MechanismTypeResult, StatusTypeResult>, IPrimaryAddressResult, IPrimaryPartyContactMechanismResult { }

	public interface IPrimaryPartyEmail<TMechanismType> : IPrimaryPartyContactMechanism<TMechanismType>, IPrimaryEmail<TMechanismType>
		where TMechanismType : MechanismTypeKey { }
	
	public interface IPrimaryPartyEmail<TMechanismType, TStatusType> : IPrimaryPartyEmail<TMechanismType>, IPrimaryPartyContactMechanism<TMechanismType, TStatusType>, IPrimaryEmail<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey { }

	public interface IPrimaryPartyEmailForAddUpdate : IPrimaryPartyEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryEmailForAddUpdate, IPrimaryPartyContactMechanismForAddUpdate { }
	
	public interface IPrimaryPartyEmailResult : IPrimaryPartyEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryEmailResult, IPrimaryPartyContactMechanismResult { }

	public interface IPrimaryPartyPhone<TMechanismType> : IPrimaryPartyContactMechanism<TMechanismType>, IPrimaryPhone<TMechanismType>
		where TMechanismType : MechanismTypeKey { }
	
	public interface IPrimaryPartyPhone<TMechanismType, TStatusType> : IPrimaryPartyPhone<TMechanismType>, IPrimaryPartyContactMechanism<TMechanismType, TStatusType>, IPrimaryPhone<TMechanismType, TStatusType>
		where TMechanismType : MechanismTypeKey
		where TStatusType : StatusTypeKey { }
	
	public interface IPrimaryPartyPhoneForAddUpdate : IPrimaryPartyPhone<MechanismTypeKey, StatusTypeKey>, IPrimaryPhoneForAddUpdate, IPrimaryPartyContactMechanismForAddUpdate { }
	
	public interface IPrimaryPartyPhoneResult : IPrimaryPartyPhone<MechanismTypeResult, StatusTypeResult>, IPrimaryPhoneResult, IPrimaryPartyContactMechanismResult { }

}
