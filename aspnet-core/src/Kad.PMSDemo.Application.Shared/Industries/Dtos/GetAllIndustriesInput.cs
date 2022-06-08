using Abp.Application.Services.Dto;
using System;

namespace Test.Industries.Dtos
{
    public class GetAllIndustriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string IndustryNameFilter { get; set; }



    }
}