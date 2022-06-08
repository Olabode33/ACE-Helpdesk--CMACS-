
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestApprovals.Dtos
{
    public class CreateOrEditRequestCmacsManagerApprovalDto : EntityDto<int?>
    {

		public bool Approved { get; set; }
		
		
		public DateTime? ApprovedTime { get; set; }
		
		
		 public int? RequestId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}