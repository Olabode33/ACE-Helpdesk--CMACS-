using Abp.Application.Services.Dto;

namespace Test.RequestThreads.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}