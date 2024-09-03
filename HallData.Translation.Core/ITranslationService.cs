using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Translation
{
    public interface ITranslationService
    {
        string Translate(string englishMessage);
        string GetErrorMessage(string errorCode);
    }
}
