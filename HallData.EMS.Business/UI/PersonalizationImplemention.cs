using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.Business;
using HallData.EMS.ApplicationViews.UI;
using HallData.EMS.Data.UI;
using HallData.Security;
using System.Threading;
using HallData.Exceptions;

namespace HallData.EMS.Business.UI
{
    public class PersonalizationImplementation : BusinessRepositoryProxy<IPersonalizationRepository>, IPersonalizationImplementation
    {
        public PersonalizationImplementation(IPersonalizationRepository repository, ISecurityImplementation security) : base(repository, security) { }


        public async Task<ApplicationViewResult> Get(string viewName, CancellationToken token = default(CancellationToken))
        {
            var userID = await ActivateAndGetSignedInUserGuid(token);
            return await this.Repository.Get(viewName, userID, token);
        }

        public async Task<ApplicationViewResult> Personalize(ApplicationViewForParty view, CancellationToken token = default(CancellationToken))
        {
            var userId = await ActivateAndGetSignedInUserGuid(token);
            if (userId == null)
                throw new GlobalizedAuthenticationException();
            await this.Repository.Personalize(view, userId.Value, token);
            return await Get(view.Name, token);
        }

        public async Task<ApplicationViewResult> RestoreDefaultSettings(string viewName, CancellationToken token = default(CancellationToken))
        {
            var userId = await ActivateAndGetSignedInUserGuid(token);
            if (userId == null)
                throw new GlobalizedAuthenticationException();
            await this.Repository.RestoreDefaultSettings(viewName, userId.Value, token);
            return await Get(viewName, token);
        }
    }
}
