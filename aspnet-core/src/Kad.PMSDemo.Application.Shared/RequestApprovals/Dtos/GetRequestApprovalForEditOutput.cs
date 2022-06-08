using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.RequestApprovals.Dtos
{
    public class GetRequestApprovalForEditOutput
    {
		public CreateOrEditRequestApprovalDto RequestApproval { get; set; }

		public string RequestRequestNo { get; set;}

		public string UserName { get; set;}


    }
}