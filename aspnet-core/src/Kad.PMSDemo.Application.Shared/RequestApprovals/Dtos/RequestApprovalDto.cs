
using System;
using Abp.Application.Services.Dto;

namespace Test.RequestApprovals.Dtos
{
    public class RequestApprovalDto : EntityDto
    {
		public DateTime? ApprovalSentTime { get; set; }

		public bool Approved { get; set; }

		public DateTime? ApprovedTime { get; set; }


		 public int? RequestId { get; set; }

		 		 public long? ApproverId { get; set; }

		 
    }
}