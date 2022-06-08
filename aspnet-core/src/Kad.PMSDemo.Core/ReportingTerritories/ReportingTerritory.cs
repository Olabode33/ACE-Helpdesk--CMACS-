

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.ReportingTerritories
{
	[Table("ReportingTerritories")]
    public class ReportingTerritory : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(ReportingTerritoryConsts.MaxTerritoryNameLength, MinimumLength = ReportingTerritoryConsts.MinTerritoryNameLength)]
		public virtual string TerritoryName { get; set; }
		

    }
}