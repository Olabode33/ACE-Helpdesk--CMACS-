using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Test.Requests.Dtos
{
    public class GetAllRequestsForExcelInput
    {
        public string Filter { get; set; }

        public string LocalSubCodeFilter { get; set; }

        public string LocalChargeCodeFilter { get; set; }

        public DateTime? MaxSubmissionDateFilter { get; set; }
        public DateTime? MinSubmissionDateFilter { get; set; }

        public DateTime? MaxRequiredResponseDateFilter { get; set; }
        public DateTime? MinRequiredResponseDateFilter { get; set; }

        public string ReasonResponseDateFilter { get; set; }

        public int IssueDiscussedFilter { get; set; }

        public string IssueDiscussedWithFilter { get; set; }

        public string OOTReviewerFilter { get; set; }

        public int? MaxOOTReviewerTimeFilter { get; set; }
        public int? MinOOTReviewerTimeFilter { get; set; }

        public string ConsultationIssueFilter { get; set; }

        public string BackgroundFilter { get; set; }

        public string TechReferenceFilter { get; set; }

        public string AgreedGuidanceFilter { get; set; }

        public string TechGrpResponseFilter { get; set; }

        public DateTime? MaxCompletionDateFilter { get; set; }
        public DateTime? MinCompletionDateFilter { get; set; }

        public int RequestStatusIdFilter { get; set; }


        public string RequestAreaRequestAreaNameFilter { get; set; }

        public string RequestDomainDomainNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string UserName2Filter { get; set; }

        public string UserName3Filter { get; set; }

        public string ClientListClientNameFilter { get; set; }

        public string UserName4Filter { get; set; }


    }

    public class RequestForExcelExport
    {
        public GetRequestForView Request { get; set; }
        public List<TechTeamTmpDto> TechTeam_ { get; set; }
        public List<NameValueDto> SubAreas { get; set; }

    }
}