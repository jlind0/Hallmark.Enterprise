using System;
using System.Threading;
using System.Threading.Tasks;
using HallData.EMS.ApplicationViews;
using HallData.EMS.ApplicationViews.Results;
using HallData.EMS.ApplicationViews.Enums;
using HallData.Repository;
using Newtonsoft.Json.Linq;
using HallData.ApplicationViews;

namespace HallData.EMS.Data
{
	public interface IReadOnlyNewsletterRepository : IReadOnlyBrandedProductRepository<NewsletterResult>{}
    public interface INewsletterRepository : IReadOnlyNewsletterRepository, IBrandedProductRepository<NewsletterResult, NewsletterForAddBase, NewsletterForUpdate> { }
}
