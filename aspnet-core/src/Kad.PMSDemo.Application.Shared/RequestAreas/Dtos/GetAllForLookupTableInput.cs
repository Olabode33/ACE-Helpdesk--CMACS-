using Abp.Application.Services.Dto;

namespace Test.RequestAreas.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}