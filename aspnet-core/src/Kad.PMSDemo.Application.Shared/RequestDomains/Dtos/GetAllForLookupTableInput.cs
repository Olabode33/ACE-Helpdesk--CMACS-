using Abp.Application.Services.Dto;

namespace Test.RequestDomains.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}