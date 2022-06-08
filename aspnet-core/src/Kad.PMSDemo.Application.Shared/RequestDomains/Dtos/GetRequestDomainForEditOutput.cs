using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestDomains.Dtos
{
    public class GetRequestDomainForEditOutput
    {
		public CreateOrEditRequestDomainDto RequestDomain { get; set; }


    }
}