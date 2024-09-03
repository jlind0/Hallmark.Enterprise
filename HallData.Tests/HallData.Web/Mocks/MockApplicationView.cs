using System;
using HallData.ApplicationViews;

namespace HallData.Tests.HallData.Web.Mocks
{
	class MockApplicationView : IHasKey
	{
		public int Key
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Name { get; set; }

		[AddOperationParameter]
		[UpdateOperationParameter]
		public string Code { get; set; }
	}
}
