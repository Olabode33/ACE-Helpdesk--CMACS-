

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.StockExchanges
{
	[Table("StockExchanges")]
    public class StockExchange : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		public virtual string StockExchangeName { get; set; }
		
		public virtual string Country { get; set; }
		

    }
}