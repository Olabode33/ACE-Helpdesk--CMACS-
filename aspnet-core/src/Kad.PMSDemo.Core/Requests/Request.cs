using Test;

using Test.RequestAreas;
using Test.RequestDomains;
using Kad.PMSDemo.Authorization.Users;
using Test.ClientLists;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.Requests
{
	[Table("Requests")]
    public class Request : FullAuditedEntity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		[StringLength(RequestConsts.MaxLocalSubCodeLength, MinimumLength = RequestConsts.MinLocalSubCodeLength)]
		public virtual string LocalSubCode { get; set; }
		
		[StringLength(RequestConsts.MaxLocalChargeCodeLength, MinimumLength = RequestConsts.MinLocalChargeCodeLength)]
		public virtual string LocalChargeCode { get; set; }
		
		public virtual DateTime SubmissionDate { get; set; }
		
		public virtual DateTime RequiredResponseDate { get; set; }
		
		public virtual string ReasonResponseDate { get; set; }
		
		public virtual bool IssueDiscussed { get; set; }
		
		public virtual string IssueDiscussedWith { get; set; }
		
		[StringLength(RequestConsts.MaxOOTReviewerLength, MinimumLength = RequestConsts.MinOOTReviewerLength)]
		public virtual string OOTReviewer { get; set; }
		
		public virtual int OOTReviewerTime { get; set; }
		
		public virtual string ConsultationIssue { get; set; }
		
		public virtual string Background { get; set; }
		
		public virtual string TechReference { get; set; }
		
		public virtual string AgreedGuidance { get; set; }
		
		public virtual string TechGrpResponse { get; set; }
		
		public virtual DateTime? CompletionDate { get; set; }
		
		public virtual RequestStatus RequestStatusId { get; set; }
		
		public virtual int? RequestAreaId { get; set; }
		public RequestArea RequestArea { get; set; }
		
		public virtual int? RequestDomainId { get; set; }
		public RequestDomain RequestDomain { get; set; }
		
		public virtual long? RequestorId { get; set; }
		public User Requestor { get; set; }
		
		public virtual long? RequestorPartnerId { get; set; }
		public User RequestorPartner { get; set; }
		
		public virtual long? RequestorManagerId { get; set; }
		public User RequestorManager { get; set; }
		
		public virtual int? ClientListId { get; set; }
		public ClientList ClientList { get; set; }
		
		public virtual long? AssigneeId { get; set; }
		public User Assignee { get; set; }

        public virtual RequestType RequestTypeId { get; set; }

        public virtual string Enquiry { get; set; }

        public virtual string EnquiryResponse { get; set; }

        [StringLength(RequestConsts.MaxRequestNoLength, MinimumLength = RequestConsts.MinRequestNoLength)]
        public virtual string RequestNo { get; set; } 

        public virtual string TermsOfRef { get; set; }

        public virtual bool TermsOfRefApproved { get; set; }

        public virtual DateTime? TORSentDate { get; set; }

        public virtual DateTime? TORApprovedDate { get; set; }

        public virtual DateTime? RequestSentDate { get; set; }

        public virtual DateTime? RequestApprovedDate { get; set; }

        public virtual string TechConclusion { get; set; }

        public virtual string OtherConsideration { get; set; }

        public virtual bool RequestApproved { get; set; }

        public virtual bool HasSignedTOR { get; set; }
        public virtual string ReturnComment { get; set; }
        public virtual bool VoluntryRequiredTor { get; set; }
    }
}