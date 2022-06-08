using Abp.Application.Services.Dto;
using System;

namespace Test.RequestApprovals.Dtos
{
    public class GetAllRequestCmacsManagerApprovalsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string RequestRequestNoFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}