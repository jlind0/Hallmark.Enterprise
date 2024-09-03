using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Exceptions
{
    public enum ErrorType : short
    {
        Authorization,
        Authentication,
        Validation,
        Other
    }
}
