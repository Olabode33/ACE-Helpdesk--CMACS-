using Abp.Application.Services.Dto;
using System;

namespace Test.RequestAreas.Dtos
{
    public class GetAllRequestSubAreasInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }


		 public string RequestAreaRequestAreaNameFilter { get; set; }

		 
    }
}