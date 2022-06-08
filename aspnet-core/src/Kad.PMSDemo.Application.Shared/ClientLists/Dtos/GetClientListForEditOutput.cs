using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.ClientLists.Dtos
{
    public class GetClientListForEditOutput
    {
		public CreateOrEditClientListDto ClientList { get; set; }

		public string IndustryIndustryName { get; set;}

		public string ReportingTerritoryTerritoryName { get; set;}

		public string StockExchangeStockExchangeName { get; set;}


    }
}