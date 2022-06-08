using Abp.Application.Services.Dto;
using System;

namespace Test.RequestDomains.Dtos
{
    public class GetAllRequestDomainsForExcelInput
    {
		public string Filter { get; set; }

		public string DomainNameFilter { get; set; }



    }
}