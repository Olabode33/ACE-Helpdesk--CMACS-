

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestDomains
{
	[Table("RequestDomains")]
    public class RequestDomain : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(RequestDomainConsts.MaxDomainNameLength, MinimumLength = RequestDomainConsts.MinDomainNameLength)]
		public virtual string DomainName { get; set; }
		

    }
}