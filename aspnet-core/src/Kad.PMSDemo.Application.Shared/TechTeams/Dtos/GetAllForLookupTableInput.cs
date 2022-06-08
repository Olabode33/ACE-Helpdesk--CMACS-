using Abp.Application.Services.Dto;

namespace Test.TechTeams.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}