
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestThreads.Dtos
{
    public class CreateOrEditRequestThreadDto : EntityDto<int?>
    {

		public string Comment { get; set; }
		
		
		public DateTime CommentDate { get; set; }
		
		
		 public int? RequestId { get; set; }
		 
		 		 public long? CommentById { get; set; }
		 
		 
    }
}