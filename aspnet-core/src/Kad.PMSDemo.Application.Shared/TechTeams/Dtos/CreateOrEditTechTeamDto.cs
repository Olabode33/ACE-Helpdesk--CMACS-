using Test;
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.TechTeams.Dtos
{
    public class CreateOrEditTechTeamDto : EntityDto<int?>
    {

		public int TimeCharge { get; set; }
		
		
		public StaffCategory Role { get; set; }
		
		
		 public int? RequestId { get; set; }
		 
		 public long? CMACSUserId { get; set; }
		 
    }
}