using Test;

using System;
using Abp.Application.Services.Dto;
using Test.TechTeams.Dtos;
using System.Collections.Generic;

namespace Test.Requests.Dtos
{
    public class RequestDto : EntityDto
    {
		public string LocalSubCode { get; set; }

		public string LocalChargeCode { get; set; }

		public DateTime SubmissionDate { get; set; }

		public DateTime? RequiredResponseDate { get; set; }

		public string ReasonResponseDate { get; set; }

		public bool IssueDiscussed { get; set; }

		public string IssueDiscussedWith { get; set; }

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

        public bool HasSignedTOR { get; set; }

		public string ReturnComment { get; set; }
		public bool VoluntryRequiredTor { get; set; }
	}
}