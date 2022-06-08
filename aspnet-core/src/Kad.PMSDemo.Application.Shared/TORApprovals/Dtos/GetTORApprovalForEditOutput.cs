using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.TORApprovals.Dtos
{
    public class GetTORApprovalForEditOutput
    {
		public CreateOrEditTORApprovalDto TORApproval { get; set; }

		public string UserName { get; set;}

		public string RequestRequestNo { get; set;}


    }
}