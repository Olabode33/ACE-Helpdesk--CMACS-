using Test;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestDocs.Dtos
{
    public class CreateOrEditRequestDocDto : EntityDto<int?>
    {

		[StringLength(RequestDocConsts.MaxDocumentNameLength, MinimumLength = RequestDocConsts.MinDocumentNameLength)]
		public string DocumentName { get; set; }
		
		
		[StringLength(RequestDocConsts.MaxDocumentLocationLength, MinimumLength = RequestDocConsts.MinDocumentLocationLength)]
		public string DocumentLocation { get; set; }
		
		
		public StaffEntityType PreparerTypeId { get; set; }
		
		
		public Guid DocumentGUID { get; set; }
		
		
		 public int? RequestId { get; set; }
		 
		 		 public long? PreparerId { get; set; }
		 
		 
    }
}