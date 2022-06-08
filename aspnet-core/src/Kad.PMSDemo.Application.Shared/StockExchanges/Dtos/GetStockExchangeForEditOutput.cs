using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.StockExchanges.Dtos
{
    public class GetStockExchangeForEditOutput
    {
		public CreateOrEditStockExchangeDto StockExchange { get; set; }


    }
}