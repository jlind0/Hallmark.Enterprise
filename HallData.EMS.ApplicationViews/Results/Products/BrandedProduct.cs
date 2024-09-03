using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;

namespace HallData.EMS.ApplicationViews
{
    public class BrandedProductKey : ProductKey, IBrandedProductKey
    {

    }

    public class BrandedProduct<TProductType, TFrequency> : Product<TProductType>, IBrandedProduct<TProductType, TFrequency>
        where TProductType: ProductTypeKey
        where TFrequency: FrequencyKey
    {
        [AddOperationParameter]
        [UpdateOperationParameter]
        public virtual DateTime? StartDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        public DateTime? EndDate { get; set; }
        [AddOperationParameter]
        [UpdateOperationParameter]
        [ChildView]
        public TFrequency Frequency { get; set; }
    }
}
