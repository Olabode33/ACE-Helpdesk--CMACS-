using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Kad.PMSDemo.Authorization;
using Kad.PMSDemo.Tenants.Dashboard.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Test;
using Test.Requests;
using Test.Tenants.Dashboard.Dto;

namespace Kad.PMSDemo.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : PMSDemoAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<Request> _requestRepository;

        public TenantDashboardAppService(IRepository<Request> requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public GetMemberActivityOutput GetMemberActivity()
        {
            return null;
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var allRequest = _requestRepository.GetAll().ToList();
            var all = allRequest.Count();
            var outstanding = allRequest.Count(x => x.RequestStatusId == RequestStatus.Requested || x.RequestStatusId == RequestStatus.Assigned || x.RequestStatusId == RequestStatus.AwaitingTOR);
            var wip = allRequest.Count(x => x.RequestStatusId == RequestStatus.WIP || x.RequestStatusId == RequestStatus.Prepared || x.RequestStatusId == RequestStatus.Accepted || x.RequestStatusId == RequestStatus.CMASManagerApproved);
            var completed = allRequest.Count(x => x.RequestStatusId == RequestStatus.Completed);


            var output = new GetDashboardDataOutput
            {
                TotalProfit = all,
                TransactionPercent = Convert.ToInt32(((decimal)wip / (decimal)all) * 100),
                NewVisitPercent = Convert.ToInt32(((decimal)completed / (decimal)all) * 100),
                BouncePercent = Convert.ToInt32(((decimal)outstanding / (decimal)all) * 100),
                TotalRequestCount = all,
                TotalConsultationCount = allRequest.Count(x => x.RequestTypeId == RequestType.Consultation),
                TotalEnquiryCount = allRequest.Count(x => x.RequestTypeId == RequestType.Enquiry),
                RequestStatusStat = GetRequestStatusStat()
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return new GetTopStatsOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500)
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod);
            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(0, 3000)
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }

        private GetRequestStatusStatsDto GetRequestStatusStat()
        {
            var requests = _requestRepository.GetAll().OrderBy(x => x.RequestStatusId);
            var consultationRequest = requests.Where(x => x.RequestTypeId == RequestType.Consultation)
                            .GroupBy(x => x.RequestStatusId)
                            .Select(x => new RequestGroupByStatus { Status = x.Key, Count = x.Count() }).ToList();

            var enquiry = requests.Where(x => x.RequestTypeId == RequestType.Enquiry)
                            .GroupBy(x => x.RequestStatusId)
                            .Select(x => new RequestGroupByStatus { Status = x.Key, Count = x.Count() }).ToList();


            var fsReview = requests.Where(x => x.RequestTypeId == RequestType.FS_Review)
                            .GroupBy(x => x.RequestStatusId)
                            .Select(x => new RequestGroupByStatus { Status = x.Key, Count = x.Count() }).ToList();


            var consultationArray = new int[8];
            consultationArray[0] = GetRequestCountByStatus(consultationRequest, RequestStatus.Requested);
            consultationArray[1] = GetRequestCountByStatus(consultationRequest, RequestStatus.Assigned);
            consultationArray[2] = GetRequestCountByStatus(consultationRequest, RequestStatus.AwaitingTOR);
            consultationArray[3] = GetRequestCountByStatus(consultationRequest, RequestStatus.WIP);
            consultationArray[4] = GetRequestCountByStatus(consultationRequest, RequestStatus.Prepared);
            consultationArray[5] = GetRequestCountByStatus(consultationRequest, RequestStatus.Accepted);
            consultationArray[6] = GetRequestCountByStatus(consultationRequest, RequestStatus.CMASManagerApproved);
            consultationArray[7] = GetRequestCountByStatus(consultationRequest, RequestStatus.Completed);


            var enquiryArray = new int[8];
            enquiryArray[0] = GetRequestCountByStatus(enquiry, RequestStatus.Requested);
            enquiryArray[1] = GetRequestCountByStatus(enquiry, RequestStatus.Assigned);
            enquiryArray[2] = GetRequestCountByStatus(enquiry, RequestStatus.AwaitingTOR);
            enquiryArray[3] = GetRequestCountByStatus(enquiry, RequestStatus.WIP);
            enquiryArray[4] = GetRequestCountByStatus(enquiry, RequestStatus.Prepared);
            enquiryArray[5] = GetRequestCountByStatus(enquiry, RequestStatus.Accepted);
            enquiryArray[6] = GetRequestCountByStatus(enquiry, RequestStatus.CMASManagerApproved);
            enquiryArray[7] = GetRequestCountByStatus(enquiry, RequestStatus.Completed);

            var fsReviewArray = new int[8];
            fsReviewArray[0] = GetRequestCountByStatus(fsReview, RequestStatus.Requested);
            fsReviewArray[1] = GetRequestCountByStatus(fsReview, RequestStatus.Assigned);
            fsReviewArray[2] = GetRequestCountByStatus(fsReview, RequestStatus.AwaitingTOR);
            fsReviewArray[3] = GetRequestCountByStatus(fsReview, RequestStatus.WIP);
            fsReviewArray[4] = GetRequestCountByStatus(fsReview, RequestStatus.Prepared);
            fsReviewArray[5] = GetRequestCountByStatus(fsReview, RequestStatus.Accepted);
            fsReviewArray[6] = GetRequestCountByStatus(fsReview, RequestStatus.CMASManagerApproved);
            fsReviewArray[7] = GetRequestCountByStatus(fsReview, RequestStatus.Completed);

            return new GetRequestStatusStatsDto
            {
                Status = new string[] {
                    RequestStatus.Requested.ToString(),
                    RequestStatus.Assigned.ToString(),
                    RequestStatus.AwaitingTOR.ToString(),
                    RequestStatus.WIP.ToString(),
                    RequestStatus.Prepared.ToString(),
                    RequestStatus.Accepted.ToString(),
                    RequestStatus.CMASManagerApproved.ToString(),
                    RequestStatus.Completed.ToString() },
                Consultation = consultationArray,
                Enquiry = enquiryArray,
                FS_Review = fsReviewArray
            };
        }

        private int GetRequestCountByStatus(List<RequestGroupByStatus> requests, RequestStatus requestStatus)
        {
            var filter = requests.FirstOrDefault(x => x.Status == requestStatus);
            return filter == null ? 0 : filter.Count;
        }
    }
}