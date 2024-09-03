using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Repository;

namespace HallData.Globalization
{
    public interface IGlobalizationRepository : IRepository
    {
        string GetErrorMessage(string errorCode, Cultures culture);
    }
}
