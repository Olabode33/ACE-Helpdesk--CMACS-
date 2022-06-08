using Test;

using Test.Industries;
using Test.ReportingTerritories;
using Test.StockExchanges;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.ClientLists
{
	[Table("ClientLists")]
    public class ClientList : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(ClientListConsts.MaxClientNameLength, MinimumLength = ClientListConsts.MinClientNameLength)]
		public virtual string ClientName { get; set; }
		
		[StringLength(ClientListConsts.MaxClientAddressLength, MinimumLength = ClientListConsts.MinClientAddressLength)]
		public virtual string ClientAddress { get; set; }
		
		[StringLength(ClientListConsts.MaxParentEntityLength, MinimumLength = ClientListConsts.MinParentEntityLength)]
		public virtual string ParentEntity { get; set; }
		
		[StringLength(ClientListConsts.MaxUltimateParentEntityLength, MinimumLength = ClientListConsts.MinUltimateParentEntityLength)]
		public virtual string UltimateParentEntity { get; set; }
		
		public virtual bool SecRegistered { get; set; }
		
		public virtual FinYearEnd FinancialYearEnd { get; set; }
		
		public virtual ChannelType ChannelTypeName { get; set; }
		

		public virtual int? IndustryId { get; set; }
		public Industry Industry { get; set; }
		
		public virtual int? ReportingTerritoryId { get; set; }
		public ReportingTerritory ReportingTerritory { get; set; }
		
		public virtual int? StockExchangeId { get; set; }
		public StockExchange StockExchange { get; set; }
		
    }
}