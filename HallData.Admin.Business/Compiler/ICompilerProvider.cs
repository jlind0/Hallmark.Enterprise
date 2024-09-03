using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Admin.Compiler
{
    public interface ICompilerProvider
    {
        void CloseContext();
        ICompiler Compiler { get; }
    }
}
