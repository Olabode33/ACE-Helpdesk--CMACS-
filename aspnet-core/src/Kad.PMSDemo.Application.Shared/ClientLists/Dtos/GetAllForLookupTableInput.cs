using Abp.Application.Services.Dto;

namespace Test.ClientLists.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}