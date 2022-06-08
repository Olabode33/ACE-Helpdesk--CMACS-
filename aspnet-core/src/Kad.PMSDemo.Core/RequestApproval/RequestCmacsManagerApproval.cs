using Test.Requests;
using Kad.PMSDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestApprovals
{
	[Table("RequestCmacsManagerApprovals")]
    public class RequestCmacsManagerApproval : FullAuditedEntity 
    {

		public virtual bool Approved { get; set; }
		
		public virtual DateTime? ApprovedTime { get; set; }
		

		public virtual int? RequestId { get; set; }
		
        [ForeignKey("RequestId")]
		public Request RequestFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}