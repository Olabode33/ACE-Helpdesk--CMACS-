
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestAreas.Dtos
{
    public class CreateOrEditRequestAreaDto : EntityDto<int?>
    {

		[StringLength(RequestAreaConsts.MaxRequestAreaNameLength, MinimumLength = RequestAreaConsts.MinRequestAreaNameLength)]
		public string RequestAreaName { get; set; }
		
		

    }
}