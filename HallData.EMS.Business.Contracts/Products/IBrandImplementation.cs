using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.ApplicationViews;
using Newtonsoft.Json.Linq;

namespace HallData.EMS.Business
{
	public interface IReadOnlyBrandImplementation : IReadOnlyProductBaseImplementation<BrandResult> { }

	[Service("brands")]
	public interface IBrandImplementation : IReadOnlyBrandImplementation, IProductBaseImplementation<BrandResult, BrandForAdd, BrandForUpdate> { }
}
