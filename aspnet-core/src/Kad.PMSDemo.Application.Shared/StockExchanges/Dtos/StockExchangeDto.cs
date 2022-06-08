
using System;
using Abp.Application.Services.Dto;

namespace Test.StockExchanges.Dtos
{
    public class StockExchangeDto : EntityDto
    {
		public string StockExchangeName { get; set; }

		public string Country { get; set; }



    }
}