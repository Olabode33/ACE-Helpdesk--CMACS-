
using System;
using Abp.Application.Services.Dto;

namespace Test.RequestApprovals.Dtos
{
    public class RequestCmacsManagerApprovalDto : EntityDto
    {
		public bool Approved { get; set; }

		public DateTime? ApprovedTime { get; set; }


		 public int? RequestId { get; set; }

		 		 public long? UserId { get; set; }

		 
    }
}