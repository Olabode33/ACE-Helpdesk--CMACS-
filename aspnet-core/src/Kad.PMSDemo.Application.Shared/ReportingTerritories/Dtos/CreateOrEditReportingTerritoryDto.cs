
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.ReportingTerritories.Dtos
{
    public class CreateOrEditReportingTerritoryDto : EntityDto<int?>
    {

		[StringLength(ReportingTerritoryConsts.MaxTerritoryNameLength, MinimumLength = ReportingTerritoryConsts.MinTerritoryNameLength)]
		public string TerritoryName { get; set; }
		
		

    }
}