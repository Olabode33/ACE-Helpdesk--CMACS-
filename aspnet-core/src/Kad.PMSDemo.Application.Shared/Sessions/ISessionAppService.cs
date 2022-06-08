using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.Sessions.Dto;

namespace Kad.PMSDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
