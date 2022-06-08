
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.StockExchanges.Dtos
{
    public class CreateOrEditStockExchangeDto : EntityDto<int?>
    {

		public string StockExchangeName { get; set; }
		
		
		public string Country { get; set; }
		
		

    }
}