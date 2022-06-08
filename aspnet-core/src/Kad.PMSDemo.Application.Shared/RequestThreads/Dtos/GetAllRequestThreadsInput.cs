using Abp.Application.Services.Dto;
using System;

namespace Test.RequestThreads.Dtos
{
    public class GetAllRequestThreadsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CommentFilter { get; set; }

		public DateTime? MaxCommentDateFilter { get; set; }
		public DateTime? MinCommentDateFilter { get; set; }


		 public string RequestLocalChargeCodeFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}