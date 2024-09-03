using HallData.Business;

namespace HallData.Tests.HallData.Web.Mocks
{
	[Service("mockbusinessimplementation")]
	interface IMockBusinessImplementation : IDeletableBusinessImplementation<MockApplicationView>
	{
	}
}
