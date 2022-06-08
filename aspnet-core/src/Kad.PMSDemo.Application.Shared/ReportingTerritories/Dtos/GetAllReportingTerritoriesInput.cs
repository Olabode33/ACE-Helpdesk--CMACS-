using Abp.Application.Services.Dto;
using System;

namespace Test.ReportingTerritories.Dtos
{
    public class GetAllReportingTerritoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TerritoryNameFilter { get; set; }



    }
}