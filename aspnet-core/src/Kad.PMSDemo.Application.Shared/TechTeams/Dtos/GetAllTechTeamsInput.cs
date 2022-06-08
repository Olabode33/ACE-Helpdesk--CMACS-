using Abp.Application.Services.Dto;
using System;

namespace Test.TechTeams.Dtos
{
    public class GetAllTechTeamsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxTimeChargeFilter { get; set; }
		public int? MinTimeChargeFilter { get; set; }

		public int RoleFilter { get; set; }


		 public string RequestLocalChargeCodeFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}