using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
