using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Kad.PMSDemo.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
