using Test;
using Test;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.ClientLists.Dtos
{
    public class CreateOrEditClientListDto : EntityDto<int?>
    {

		[StringLength(ClientListConsts.MaxClientNameLength, MinimumLength = ClientListConsts.MinClientNameLength)]
		[Required]
		public string ClientName { get; set; }
		
		
		[StringLength(ClientListConsts.MaxClientAddressLength, MinimumLength = ClientListConsts.MinClientAddressLength)]
		public string ClientAddress { get; set; }
		
		
		[StringLength(ClientListConsts.MaxParentEntityLength, MinimumLength = ClientListConsts.MinParentEntityLength)]
		public string ParentEntity { get; set; }
		
		
		[StringLength(ClientListConsts.MaxUltimateParentEntityLength, MinimumLength = ClientListConsts.MinUltimateParentEntityLength)]
		public string UltimateParentEntity { get; set; }
		
		
		public bool SecRegistered { get; set; }
		
		
		public FinYearEnd FinancialYearEnd { get; set; }
		
		
		public ChannelType ChannelTypeName { get; set; }
		
		
		 public int? IndustryId { get; set; }
		 
		 		 public int? ReportingTerritoryId { get; set; }
		 
		 		 public int? StockExchangeId { get; set; }
		 
		 
    }
}