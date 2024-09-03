using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;

namespace HallData.EMS.Business
{
	[Service("categories")]
	public interface ICategoryImplementation : IReadOnlyCategoryImplementation, 
		IDeletableBusinessImplementationWithBase<int, CategoryResult, CategoryForAdd, CategoryForUpdate>
	{

	}

	public interface IReadOnlyCategoryImplementation : IReadOnlyBusinessImplementation<CategoryResult> { }
}
