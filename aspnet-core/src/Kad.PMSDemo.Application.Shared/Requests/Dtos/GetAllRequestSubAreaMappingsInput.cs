using Abp.Application.Services.Dto;
using System;

namespace Test.Requests.Dtos
{
    public class GetAllRequestSubAreaMappingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }


		 public string RequestRequestNoFilter { get; set; }

		 		 public string RequestSubAreaNameFilter { get; set; }

		 
    }
}