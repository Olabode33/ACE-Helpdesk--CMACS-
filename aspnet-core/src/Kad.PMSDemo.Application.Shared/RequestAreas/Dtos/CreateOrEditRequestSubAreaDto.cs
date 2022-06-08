
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestAreas.Dtos
{
    public class CreateOrEditRequestSubAreaDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		 public int RequestAreaId { get; set; }
		 
		 
    }
}