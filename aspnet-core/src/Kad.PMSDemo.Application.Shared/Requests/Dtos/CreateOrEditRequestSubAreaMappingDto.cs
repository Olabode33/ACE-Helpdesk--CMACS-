
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.Requests.Dtos
{
    public class CreateOrEditRequestSubAreaMappingDto : EntityDto<int?>
    {

		 public int RequestId { get; set; }
		 
		 		 public int RequestSubAreaId { get; set; }
		 
		 
    }
}