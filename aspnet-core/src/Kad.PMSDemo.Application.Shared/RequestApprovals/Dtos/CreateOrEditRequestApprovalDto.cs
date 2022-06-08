
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestApprovals.Dtos
{
    public class CreateOrEditRequestApprovalDto : EntityDto<int?>
    {

		public DateTime? ApprovalSentTime { get; set; }
		
		
		public bool Approved { get; set; }
		
		
		public DateTime? ApprovedTime { get; set; }
		
		
		 public int? RequestId { get; set; }
		 
		 		 public long? ApproverId { get; set; }
		 
		 
    }
}