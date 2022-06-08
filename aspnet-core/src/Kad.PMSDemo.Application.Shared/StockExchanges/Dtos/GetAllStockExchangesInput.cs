using Abp.Application.Services.Dto;
using System;

namespace Test.StockExchanges.Dtos
{
    public class GetAllStockExchangesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string StockExchangeNameFilter { get; set; }

		public string CountryFilter { get; set; }



    }
}