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
    public class ProductContactImplementation : GenericContactImplementation<IProductContactMechanismRepository,ProductContactMechanismId, Guid, ProductContactMechanismKey,
        ProductContactMechanismResult<Address>, ProductContactMechanismForAdd<AddressForAdd>, ProductContactMechanismForUpdate<AddressForUpdate>,
        ProductContactMechanismResult<Email>, ProductContactMechanismForAdd<EmailForAdd>, ProductContactMechanismForUpdate<EmailForUpdate>,
        ProductContactMechanismResult<Phone>, ProductContactMechanismForAdd<PhoneForAdd>, ProductContactMechanismForUpdate<PhoneForUpdate>,
        ProductContactMechanismResult<ContactMechanismGeneric>, ProductContactMechanismForAdd<ContactMechanismGenericForAdd>, 
        ProductContactMechanismForUpdate<ContactMechanismGenericForUpdate>>, IProductContactImplementation
    {
        public ProductContactImplementation(IProductContactMechanismRepository repository, ISecurityImplementation security,
            IContactMechanismRepository contactMechanismRepository)
            : base(repository, security, contactMechanismRepository) { }

        protected override ProductContactMechanismId CreateKey(Guid id, Guid contactMechanismId, string contactMechanismTypeName)
        {
            return new ProductContactMechanismId(id, contactMechanismId, contactMechanismTypeName);
        }
    }
}
