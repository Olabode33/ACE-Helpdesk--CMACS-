using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.AttachedDocs.Dtos
{
    public class GetAttachedDocForEditOutput
    {
		public CreateOrEditAttachedDocDto AttachedDoc { get; set; }

		public string RequestRequestNo { get; set;}

		public string UserName { get; set;}

		public string BinaryObjectTenantId { get; set;}


    }
}