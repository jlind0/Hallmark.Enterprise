using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Validation;
using System.ComponentModel.DataAnnotations;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;

namespace HallData.EMS.Models
{
    public class ChangePasswordParameters
    {
        [GlobalizedRequired("PASSWORDCHANGE_CURRENT_REQUIRED")]
        [GlobalizedDataType(DataType.Password, "PASSWORDCHANGE_CURRENT_FORMAT")]
        public string CurrentPassword { get; set; }
        [GlobalizedRequired("PASSWORDCHANGE_NEW_REQUIRED")]
        [GlobalizedDataType(DataType.Password, "PASSWORDCHANGE_NEW_FORMAT")]
        public string NewPassword { get; set; }
    }
    public class ChangePasswordResult : IQueryResult<UserResult>
    {
        public bool PasswordChanged { get; set; }
        public ChangePasswordResult(IQueryResult<UserResult> result, bool passwordChanged)
        {
            this.PasswordChanged = passwordChanged;
            if(result != null)
            {
                this.Result = result.Result;
                this.AvailableProperties = result.AvailableProperties;
            }
        }

        public UserResult Result { get; set; }

        public IEnumerable<string> AvailableProperties { get; set; }
    }
}
