
using Kad.PMSDemo.Authorization.Users;
using Test.Requests;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.TORApprovals
{
	[Table("TORApprovals")]
    public class TORApproval : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		public virtual DateTime? TORTimeSent { get; set; }
		
		public virtual bool Approved { get; set; }
		
		public virtual DateTime? ApprovedTime { get; set; }
		

		public virtual long? ApproverId { get; set; }
		public User Approver { get; set; }
		
		public virtual int? RequestId { get; set; }
		public Request Request { get; set; }
		
    }
}