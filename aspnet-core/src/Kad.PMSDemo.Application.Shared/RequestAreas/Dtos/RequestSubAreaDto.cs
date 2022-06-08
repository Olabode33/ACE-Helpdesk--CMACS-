
using System;
using Abp.Application.Services.Dto;

namespace Test.RequestAreas.Dtos
{
    public class RequestSubAreaDto : EntityDto
    {
		public string Name { get; set; }


		 public int RequestAreaId { get; set; }

		 
    }
}