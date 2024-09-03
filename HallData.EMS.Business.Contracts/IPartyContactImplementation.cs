using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.Models;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
    public interface IPartyContactImplementation : IGenericContactImplementation<PartyContactMechanismId, Guid, PartyContactMechanismKey, 
        PartyContactMechanismResult<Address>, PartyContactMechanismForAdd<AddressForAdd>, PartyContactMechanismForUpdate<AddressForUpdate>,
        PartyContactMechanismResult<Email>, PartyContactMechanismForAdd<EmailForAdd>, PartyContactMechanismForUpdate<EmailForUpdate>,
        PartyContactMechanismResult<Phone>, PartyContactMechanismForAdd<PhoneForAdd>, PartyContactMechanismForUpdate<PhoneForUpdate>,
        PartyContactMechanismResult<ContactMechanismGeneric>, PartyContactMechanismForAdd<ContactMechanismGenericForAdd>,
        PartyContactMechanismForUpdate<ContactMechanismGenericForUpdate>> { }
}