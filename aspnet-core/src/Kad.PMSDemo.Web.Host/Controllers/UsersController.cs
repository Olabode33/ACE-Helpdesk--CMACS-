using Abp.AspNetCore.Mvc.Authorization;
using Kad.PMSDemo.Authorization;
using Kad.PMSDemo.Storage;
using Abp.BackgroundJobs;

namespace Kad.PMSDemo.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}