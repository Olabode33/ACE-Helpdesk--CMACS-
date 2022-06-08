using Abp.Application.Services.Dto;
using System;

namespace Test.ClientLists.Dtos
{
    public class GetAllClientListsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ClientNameFilter { get; set; }

		public string ClientAddressFilter { get; set; }

		public string ParentEntityFilter { get; set; }

		public string UltimateParentEntityFilter { get; set; }

		public int SecRegisteredFilter { get; set; }

		public int FinancialYearEndFilter { get; set; }

		public int ChannelTypeNameFilter { get; set; }


		 public string IndustryIndustryNameFilter { get; set; }

		 		 public string ReportingTerritoryTerritoryNameFilter { get; set; }

		 		 public string StockExchangeStockExchangeNameFilter { get; set; }

		 
    }
}