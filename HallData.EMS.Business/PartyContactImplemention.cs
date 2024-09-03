using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.Models;
using HallData.Business;
using HallData.EMS.Data;
using System.Threading;
using HallData.Repository;
using HallData.Security;
using HallData.EMS.ApplicationViews.Enums;
using System.ComponentModel.DataAnnotations;
using HallData.Exceptions;
using HallData.Validation;
using HallData.ApplicationViews;

namespace HallData.EMS.Business
{
    public class PartyContactImplementation : GenericContactImplementation<IPartyContactMechanismRepository, PartyContactMechanismId, Guid, PartyContactMechanismKey, 
        PartyContactMechanismResult<Address>, PartyContactMechanismForAdd<AddressForAdd>, PartyContactMechanismForUpdate<AddressForUpdate>,
        PartyContactMechanismResult<Email>, PartyContactMechanismForAdd<EmailForAdd>, PartyContactMechanismForUpdate<EmailForUpdate>,
        PartyContactMechanismResult<Phone>, PartyContactMechanismForAdd<PhoneForAdd>, PartyContactMechanismForUpdate<PhoneForUpdate>,
        PartyContactMechanismResult<ContactMechanismGeneric>, PartyContactMechanismForAdd<ContactMechanismGenericForAdd>,
        PartyContactMechanismForUpdate<ContactMechanismGenericForUpdate>>, IPartyContactImplementation
   {
       public PartyContactImplementation(IPartyContactMechanismRepository repository, ISecurityImplementation security, 
           IContactMechanismRepository contactMechanismRepository)
            : base(repository, security, contactMechanismRepository) { }

       protected override PartyContactMechanismId CreateKey(Guid id, Guid contactMechanismId, string contactMechanismTypeName)
       {
           return new PartyContactMechanismId(contactMechanismId, id, contactMechanismTypeName);
       }
   }
}
