using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestDocs.Dtos
{
    public class GetRequestDocForEditOutput
    {
		public CreateOrEditRequestDocDto RequestDoc { get; set; }

		public string RequestLocalChargeCode { get; set;}

		public string UserName { get; set;}


    }
}