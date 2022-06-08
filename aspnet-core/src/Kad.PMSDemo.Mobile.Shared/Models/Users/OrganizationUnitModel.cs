using Abp.AutoMapper;
using Kad.PMSDemo.Organizations.Dto;

namespace Kad.PMSDemo.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}