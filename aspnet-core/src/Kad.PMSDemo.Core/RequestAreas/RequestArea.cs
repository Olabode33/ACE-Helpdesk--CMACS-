

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestAreas
{
	[Table("RequestAreas")]
    public class RequestArea : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(RequestAreaConsts.MaxRequestAreaNameLength, MinimumLength = RequestAreaConsts.MinRequestAreaNameLength)]
		public virtual string RequestAreaName { get; set; }
		

    }
}