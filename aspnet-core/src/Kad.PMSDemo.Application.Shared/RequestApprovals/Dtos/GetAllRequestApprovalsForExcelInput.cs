using Abp.Application.Services.Dto;
using System;

namespace Test.RequestApprovals.Dtos
{
    public class GetAllRequestApprovalsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxApprovalSentTimeFilter { get; set; }
		public DateTime? MinApprovalSentTimeFilter { get; set; }

		public int ApprovedFilter { get; set; }

		public DateTime? MaxApprovedTimeFilter { get; set; }
		public DateTime? MinApprovedTimeFilter { get; set; }


		 public string RequestRequestNoFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}