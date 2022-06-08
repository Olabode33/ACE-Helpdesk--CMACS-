using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestThreads.Dtos
{
    public class GetRequestThreadForEditOutput
    {
		public CreateOrEditRequestThreadDto RequestThread { get; set; }

		public string RequestLocalChargeCode { get; set;}

		public string UserName { get; set;}


    }
}