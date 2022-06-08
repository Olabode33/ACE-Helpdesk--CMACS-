using Abp.Application.Services.Dto;

namespace Test.ReportingTerritories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}