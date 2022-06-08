
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.Industries.Dtos
{
    public class CreateOrEditIndustryDto : EntityDto<int?>
    {

		[Required]
		[StringLength(IndustryConsts.MaxIndustryNameLength, MinimumLength = IndustryConsts.MinIndustryNameLength)]
		public string IndustryName { get; set; }
		
		

    }
}