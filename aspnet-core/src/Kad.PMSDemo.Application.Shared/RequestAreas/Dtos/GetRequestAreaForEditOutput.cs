using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestAreas.Dtos
{
    public class GetRequestAreaForEditOutput
    {
		public CreateOrEditRequestAreaDto RequestArea { get; set; }


    }
}