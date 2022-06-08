using Test;

using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestDocs
{
	[Table("RequestDocs")]
    public class RequestDoc : FullAuditedEntity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(RequestDocConsts.MaxDocumentNameLength, MinimumLength = RequestDocConsts.MinDocumentNameLength)]
		public virtual string DocumentName { get; set; }
		
		[StringLength(RequestDocConsts.MaxDocumentLocationLength, MinimumLength = RequestDocConsts.MinDocumentLocationLength)]
		public virtual string DocumentLocation { get; set; }
		
		public virtual StaffEntityType PreparerTypeId { get; set; }
		
		public virtual Guid DocumentGUID { get; set; }
		

		public virtual int? RequestId { get; set; }
		public Request Request { get; set; }
		
		public virtual long? PreparerId { get; set; }
		public User Preparer { get; set; }
		
    }
}