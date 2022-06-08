using Abp.Application.Services.Dto;

namespace Test.Industries.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}