using System.Threading.Tasks;
using Kad.PMSDemo.Sessions.Dto;

namespace Kad.PMSDemo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
