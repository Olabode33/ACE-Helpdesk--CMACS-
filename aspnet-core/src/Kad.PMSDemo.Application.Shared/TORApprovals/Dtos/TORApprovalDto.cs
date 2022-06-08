
using System;
using Abp.Application.Services.Dto;

namespace Test.TORApprovals.Dtos
{
    public class TORApprovalDto : EntityDto
    {
		public DateTime? TORTimeSent { get; set; }

		public bool Approved { get; set; }

		public DateTime? ApprovedTime { get; set; }


		 public long? ApproverId { get; set; }

		 		 public int? RequestId { get; set; }

		 
    }
}