using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.EMS.ApplicationViews
{
    public interface IPrimaryProductContactMechanism : IProductContactMechanismKey { }
    public interface IPrimaryProductContactMechanism<TMechanismType> : IPrimaryContactMechanism<TMechanismType>, IPrimaryProductContactMechanism
        where TMechanismType : MechanismTypeKey{ }
    public interface IPrimaryProductContactMechanism<TMechanismType, TStatusType> : IPrimaryProductContactMechanism<TMechanismType>, IPrimaryContactMechanism<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        where TStatusType : StatusTypeKey { }
    public interface IPrimaryProductContactMechanismForAddUpdate : IPrimaryProductContactMechanism<MechanismTypeKey, StatusTypeKey>, IPrimaryContactMechanismForAddUpdate { }
	public interface IPrimaryProductContactMechanismResult : IPrimaryProductContactMechanism<MechanismTypeResult, StatusTypeResult>, IPrimaryContactMechanismResult, IProductContactMechanismResult { }

    public interface IPrimaryProductAddress<TMechanismType> : IPrimaryProductContactMechanism<TMechanismType>, IPrimaryAddress<TMechanismType>
        where TMechanismType : MechanismTypeKey
         { }
    public interface IPrimaryProductAddress<TMechanismType, TStatusType> : IPrimaryProductAddress<TMechanismType>, IPrimaryProductContactMechanism<TMechanismType, TStatusType>, IPrimaryAddress<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        
        where TStatusType : StatusTypeKey { }
    public interface IPrimaryProductAddressForAddUpdate : IPrimaryProductAddress<MechanismTypeKey, StatusTypeKey>, IPrimaryAddressForAddUpdate, IPrimaryProductContactMechanismForAddUpdate { }
	public interface IPrimaryProductAddressResult : IPrimaryProductAddress<MechanismTypeResult, StatusTypeResult>, IPrimaryAddressResult, IPrimaryProductContactMechanismResult { }

    public interface IPrimaryProductEmail<TMechanismType> : IPrimaryProductContactMechanism<TMechanismType>, IPrimaryEmail<TMechanismType>
        where TMechanismType : MechanismTypeKey
         { }
    public interface IPrimaryProductEmail<TMechanismType, TStatusType> : IPrimaryProductEmail<TMechanismType>, IPrimaryProductContactMechanism<TMechanismType, TStatusType>, IPrimaryEmail<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        
        where TStatusType : StatusTypeKey { }
    public interface IPrimaryProductEmailForAddUpdate : IPrimaryProductEmail<MechanismTypeKey, StatusTypeKey>, IPrimaryEmailForAddUpdate, IPrimaryProductContactMechanismForAddUpdate { }
	public interface IPrimaryProductEmailResult : IPrimaryProductEmail<MechanismTypeResult, StatusTypeResult>, IPrimaryEmailResult, IPrimaryProductContactMechanismResult { }

    public interface IPrimaryProductPhone<TMechanismType> : IPrimaryProductContactMechanism<TMechanismType>, IPrimaryPhone<TMechanismType>
        where TMechanismType : MechanismTypeKey
         { }
    public interface IPrimaryProductPhone<TMechanismType, TStatusType> : IPrimaryProductPhone<TMechanismType>, IPrimaryProductContactMechanism<TMechanismType, TStatusType>, IPrimaryPhone<TMechanismType, TStatusType>
        where TMechanismType : MechanismTypeKey
        
        where TStatusType : StatusTypeKey { }
    public interface IPrimaryProductPhoneForAddUpdate : IPrimaryProductPhone<MechanismTypeKey, StatusTypeKey>, IPrimaryPhoneForAddUpdate, IPrimaryProductContactMechanismForAddUpdate { }
	public interface IPrimaryProductPhoneResult : IPrimaryProductPhone<MechanismTypeResult, StatusTypeResult>, IPrimaryPhoneResult, IPrimaryProductContactMechanismResult { }
}
