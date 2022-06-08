
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestDomains.Dtos
{
    public class CreateOrEditRequestDomainDto : EntityDto<int?>
    {

		[StringLength(RequestDomainConsts.MaxDomainNameLength, MinimumLength = RequestDomainConsts.MinDomainNameLength)]
		public string DomainName { get; set; }
		
		

    }
}