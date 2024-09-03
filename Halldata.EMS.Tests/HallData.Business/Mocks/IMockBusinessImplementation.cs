using System;
using HallData.Business;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.Data;
using HallData.Security;

namespace HallData.EMS.Tests.HallData.Business.Mocks
{
	public interface IMockBusinessImplementation : IDeletableBusinessImplementationWithBase<Guid, PersonResult, PersonForAdd, PersonForUpdate>
	{

	}

	public class MockBusinessImplementation :
		DeletableBusinessRepositoryProxyWithBase<IPersonRepository, Guid, PersonResult, PersonForAdd, PersonForUpdate>,
		IMockBusinessImplementation
	{
		public MockBusinessImplementation(IPersonRepository repository, ISecurityImplementation security) : 
			base(repository, security)
		{
			
		}
	}
}
