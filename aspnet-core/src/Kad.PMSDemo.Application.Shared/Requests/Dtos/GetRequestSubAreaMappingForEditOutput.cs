using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.Requests.Dtos
{
    public class GetRequestSubAreaMappingForEditOutput
    {
		public CreateOrEditRequestSubAreaMappingDto RequestSubAreaMapping { get; set; }

		public string RequestRequestNo { get; set;}

		public string RequestSubAreaName { get; set;}


    }
}