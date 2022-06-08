using Abp.Application.Services.Dto;

namespace Test.StockExchanges.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}