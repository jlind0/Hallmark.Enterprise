using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.ApplicationViews;
using HallData.Validation;
using Newtonsoft.Json;

namespace HallData.EMS.ApplicationViews.UI
{
    public class TemplateTypeKey : IHasKey
    {
        [UpdateOperationParameter]
        [ChildKeyOperationParameter]
        public int TemplateTypeId { get; set; }

        public int Key
        {
            get
            {
                return this.TemplateTypeId;
            }
            set
            {
                this.TemplateTypeId = value;
            }
        }
    }
    public class TemplateType : TemplateTypeKey
    {
        public string Name { get; set; }
    }
}
