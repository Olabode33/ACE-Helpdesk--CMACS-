using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Test.TechTeams;
using Test.TechTeams.Dtos;
using Test.AttachedDocs.Dtos;

namespace Test.Requests.Dtos
{
    public class GetRequestForEditOutput
    {
		public CreateOrEditRequestDto Request { get; set; }

		public string RequestAreaRequestAreaName { get; set;}

		public string RequestDomainDomainName { get; set;}

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public string UserName3 { get; set;}

		public string ClientListClientName { get; set;}

		public string UserName4 { get; set;}

        public List<TechTeamTmpDto> TechTeam_ { get; set; }

        public string CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public List<ApprovalAuditInfo> TORApprovalsAuditInfo { get; set; }
        public List<ApprovalAuditInfo> RequestApprovalsAuditInfo { get; set; }
        public ApprovalAuditInfo CmacsManagerApprovalInfo { get; set; }

        public List<AttachedDocDto> RequestAttachments { get; set; }
        public List<AttachedDocDto> SignedTORAttachments { get; set; }
        public List<AttachedDocDto> ReviewedFSAttachments { get; set; }
        public List<NameValueDto> SubAreas { get; set; }
        public string NextAction { get; set; }
    }

    public class ApprovalAuditInfo
    {
        public DateTime? ApprovalSentDate { get; set; }
        public string ApproverName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool ApprovalStatus { get; set; }
    }
}