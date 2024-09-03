using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Repository.Mocks;

namespace HallData.EMS.Tests.HallData.Business.Mocks
{
	public class MockPersonRepository : DeleteableMockRepositoryGuid<PersonResult, PersonForAdd, PersonForUpdate>,
		IPersonRepository
	{

		public MockPersonRepository(IEnumerable<PersonResult> people) : base(people)
		{
			
		}

		public Task AddCategory(Guid partyID, int categoryID, Guid userID, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		public Task RemoveCategory(Guid partyID, int categoryID, Guid userID, System.Threading.CancellationToken token = default(CancellationToken))
		{
			throw new NotImplementedException();
		}
	}
}
