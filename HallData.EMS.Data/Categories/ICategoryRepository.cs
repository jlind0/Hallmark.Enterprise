using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.Repository;

namespace HallData.EMS.Data
{
	public interface IReadOnlyCategoryRepository : IReadOnlyRepository<CategoryResult> { }

	public interface ICategoryRepository : IReadOnlyCategoryRepository, 
		IDeletableRepository<int, CategoryResult, CategoryForAdd, CategoryForUpdate> { }
}