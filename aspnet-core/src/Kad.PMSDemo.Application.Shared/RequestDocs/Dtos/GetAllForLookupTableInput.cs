using Abp.Application.Services.Dto;

namespace Test.RequestDocs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}