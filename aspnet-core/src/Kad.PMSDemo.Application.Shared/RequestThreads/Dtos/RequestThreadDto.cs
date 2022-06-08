
using System;
using Abp.Application.Services.Dto;

namespace Test.RequestThreads.Dtos
{
    public class RequestThreadDto : EntityDto
    {
		public string Comment { get; set; }

		public DateTime CommentDate { get; set; }


		 public int? RequestId { get; set; }

		 		 public long? CommentById { get; set; }

		 
    }
}