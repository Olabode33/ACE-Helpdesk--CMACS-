using Test;
using Test;

using System;
using Abp.Application.Services.Dto;

namespace Test.ClientLists.Dtos
{
    public class ClientListDto : EntityDto
    {
		public string ClientName { get; set; }

		public string ClientAddress { get; set; }

		public string ParentEntity { get; set; }

		public string UltimateParentEntity { get; set; }

		public bool SecRegistered { get; set; }

		public FinYearEnd FinancialYearEnd { get; set; }

		public ChannelType ChannelTypeName { get; set; }


		 public int? IndustryId { get; set; }

		 		 public int? ReportingTerritoryId { get; set; }

		 		 public int? StockExchangeId { get; set; }

		 
    }
}