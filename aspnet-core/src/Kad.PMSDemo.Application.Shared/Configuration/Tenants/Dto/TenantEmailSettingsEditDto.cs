using Abp.Auditing;
using Kad.PMSDemo.Configuration.Dto;

namespace Kad.PMSDemo.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}