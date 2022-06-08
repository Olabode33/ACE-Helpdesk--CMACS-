using Test;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Test.TechTeams.Dtos;
using Test.AttachedDocs.Dtos;

namespace Test.Requests.Dtos
{
    public class CreateOrEditRequestDto : EntityDto<int?>
    {
        [StringLength(RequestConsts.MaxLocalSubCodeLength, MinimumLength = RequestConsts.MinLocalSubCodeLength)]
        public string LocalSubCode { get; set; }

        [StringLength(RequestConsts.MaxLocalChargeCodeLength, MinimumLength = RequestConsts.MinLocalChargeCodeLength)]
        public string LocalChargeCode { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTime? RequiredResponseDate { get; set; }

        public string ReasonResponseDate { get; set; }

        public bool IssueDiscussed { get; set; }

        public string IssueDiscussedWith { get; set; }

        [StringLength(RequestConsts.MaxOOTReviewerLength, MinimumLength = RequestConsts.MinOOTReviewerLength)]
        public string OOTReviewer { get; set; }

        public int OOTReviewerTime { get; set; }

        public string ConsultationIssue { get; set; }

        public string Background { get; set; }

        public string TechReference { get; set; }

        public string AgreedGuidance { get; set; }

        public string TechGrpResponse { get; set; }

        public DateTime? CompletionDate { get; set; }

        public RequestStatus RequestStatusId { get; set; }

        public int? RequestAreaId { get; set; }

        public int? RequestDomainId { get; set; }

        public long? RequestorId { get; set; }

        public long? RequestorPartnerId { get; set; }

        public long? RequestorManagerId { get; set; }

        public int? ClientListId { get; set; }

        public long? AssigneeId { get; set; }

        public RequestType RequestTypeId { get; set; }

        public string Enquiry { get; set; }

        public string EnquiryResponse { get; set; }

        public string RequestNo { get; set; }

        public string TermsOfRef { get; set; }

        public bool TermsOfRefApproved { get; set; }

        public string TechConclusion { get; set; }

        public string OtherConsideration { get; set; }

        public bool HasSignedTOR { get; set; }
        public string ReturnComment { get; set; }
        public bool VoluntryRequiredTor { get; set; }
        public List<CreateOrEditAttachedDocDto> Attachments { get; set; }
        public List<NameValueDto> SubAreas { get; set; }
    }

    public class CreateRequestTORDto
    {
        public int RequestId { get; set; }
        public string Tor { get; set; }
        public bool HasSignedTOR { get; set; }
    }
}