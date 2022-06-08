using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestAreas.Dtos
{
    public class GetRequestSubAreaForEditOutput
    {
		public CreateOrEditRequestSubAreaDto RequestSubArea { get; set; }

		public string RequestAreaRequestAreaName { get; set;}


    }
}