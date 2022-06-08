using Abp.Application.Services.Dto;

namespace Test.AttachedDocs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}