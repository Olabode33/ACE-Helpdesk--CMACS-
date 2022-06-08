using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.TechTeams.Dtos
{
    public class GetTechTeamForEditOutput
    {
		public CreateOrEditTechTeamDto TechTeam { get; set; }

		public string RequestLocalChargeCode { get; set;}

		public string UserName { get; set;}


    }
}