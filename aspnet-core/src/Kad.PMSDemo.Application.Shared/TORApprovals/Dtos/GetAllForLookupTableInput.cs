using Abp.Application.Services.Dto;

namespace Test.TORApprovals.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}