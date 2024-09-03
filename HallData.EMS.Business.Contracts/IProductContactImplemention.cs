using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;

namespace HallData.EMS.Business
{
    public interface IProductContactImplementation : IGenericContactImplementation<ProductContactMechanismId, Guid, ProductContactMechanismKey,
        ProductContactMechanismResult<Address>, ProductContactMechanismForAdd<AddressForAdd>, ProductContactMechanismForUpdate<AddressForUpdate>,
        ProductContactMechanismResult<Email>, ProductContactMechanismForAdd<EmailForAdd>, ProductContactMechanismForUpdate<EmailForUpdate>,
        ProductContactMechanismResult<Phone>, ProductContactMechanismForAdd<PhoneForAdd>, ProductContactMechanismForUpdate<PhoneForUpdate>,
        ProductContactMechanismResult<ContactMechanismGeneric>, ProductContactMechanismForAdd<ContactMechanismGenericForAdd>, 
        ProductContactMechanismForUpdate<ContactMechanismGenericForUpdate>>
    {
    }
}
