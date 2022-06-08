

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.Industries
{
	[Table("Industries")]
    public class Industry : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[Required]
		[StringLength(IndustryConsts.MaxIndustryNameLength, MinimumLength = IndustryConsts.MinIndustryNameLength)]
		public virtual string IndustryName { get; set; }

    }
}