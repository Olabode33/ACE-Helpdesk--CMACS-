using Abp.Application.Services.Dto;
using System;

namespace Test.RequestDomains.Dtos
{
    public class GetAllRequestDomainsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string DomainNameFilter { get; set; }



    }
}