using Abp.Application.Services.Dto;
using System;

namespace Test.TORApprovals.Dtos
{
    public class GetAllTORApprovalsForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxTORTimeSentFilter { get; set; }
		public DateTime? MinTORTimeSentFilter { get; set; }

		public int ApprovedFilter { get; set; }

		public DateTime? MaxApprovedTimeFilter { get; set; }
		public DateTime? MinApprovedTimeFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string RequestRequestNoFilter { get; set; }

		 
    }
}