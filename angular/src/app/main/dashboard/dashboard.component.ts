import { Component, Injector, ViewEncapsulation, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { TenantDashboardServiceProxy, RequestsServiceProxy, GetDashboardDataOutput, SalesSummaryDatePeriod } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { DashboardChartBase } from '@app/shared/common/customizable-dashboard/widgets/dashboard-chart-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class DashboardComponent extends AppComponentBase implements OnInit {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultTenantDashboard;

    dashboardHeaderStats: DashboardHeaderStats = new DashboardHeaderStats();
    dailySalesLineChart: DailySalesLineChart;
    generalStatsPieChart: GeneralStatsPieChart;


    filterText = '';
    localSubCodeFilter = '';
    localChargeCodeFilter = '';
    maxSubmissionDateFilter: moment.Moment;
    minSubmissionDateFilter: moment.Moment;
    maxRequiredResponseDateFilter: moment.Moment;
    minRequiredResponseDateFilter: moment.Moment;
    reasonResponseDateFilter = '';
    issueDiscussedFilter = -1;
    issueDiscussedWithFilter = '';
    ootReviewerFilter = '';
    maxOOTReviewerTimeFilter: number;
    maxOOTReviewerTimeFilterEmpty: number;
    minOOTReviewerTimeFilter: number;
    minOOTReviewerTimeFilterEmpty: number;
    consultationIssueFilter = '';
    backgroundFilter = '';
    techReferenceFilter = '';
    agreedGuidanceFilter = '';
    techGrpResponseFilter = '';
    maxCompletionDateFilter: moment.Moment;
    minCompletionDateFilter: moment.Moment;
    requestStatusIdFilter = -1;
    requestAreaRequestAreaNameFilter = '';
    requestDomainDomainNameFilter = '';
    userNameFilter = '';
    userName2Filter = '';
    userName3Filter = '';
    clientListClientNameFilter = '';
    userName4Filter = '';


    constructor(
        injector: Injector,
        private _dashboardService: TenantDashboardServiceProxy,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _location: Location,
        private _router: Router) {
        super(injector);
        this.dailySalesLineChart = new DailySalesLineChart(this._dashboardService);
        this.generalStatsPieChart = new GeneralStatsPieChart(this._dashboardService);
    }

    ngOnInit() {
        this.dailySalesLineChart.reload();
        this.generalStatsPieChart.reload();
        this.getDashboardStatisticsData(SalesSummaryDatePeriod.Daily);
    }

    getDashboardStatisticsData(datePeriod): void {
        // this.salesSummaryChart.showLoading();
        // this.generalStatsPieChart.showLoading();

        this._dashboardService
            .getDashboardData(datePeriod)
            .subscribe(result => {
                this.dashboardHeaderStats.init(result.totalRequestCount, result.totalConsultationCount, result.totalEnquiryCount, result.newUsers);
                this.generalStatsPieChart.init(result.transactionPercent, result.newVisitPercent, result.bouncePercent);
                this.dailySalesLineChart.init(result.requestStatusStat);
                // this.profitSharePieChart.init(result.profitShares);
                // this.salesSummaryChart.init(result.salesSummary, result.totalSales, result.revenue, result.expenses, result.growth);
            });
    }

    goBack(): void {
        this._location.back();
    }

    navigateToCompletedRequestPage(): void {
        this._router.navigate(['app/main/requests/requests']);
    }

    exportToExcel(): void {
        this._requestsServiceProxy.getRequestsToExcel(
            this.filterText,
            this.localSubCodeFilter,
            this.localChargeCodeFilter,
            this.maxSubmissionDateFilter,
            this.minSubmissionDateFilter,
            this.maxRequiredResponseDateFilter,
            this.minRequiredResponseDateFilter,
            this.reasonResponseDateFilter,
            this.issueDiscussedFilter,
            this.issueDiscussedWithFilter,
            this.ootReviewerFilter,
            this.maxOOTReviewerTimeFilter == null ? this.maxOOTReviewerTimeFilterEmpty : this.maxOOTReviewerTimeFilter,
            this.minOOTReviewerTimeFilter == null ? this.minOOTReviewerTimeFilterEmpty : this.minOOTReviewerTimeFilter,
            this.consultationIssueFilter,
            this.backgroundFilter,
            this.techReferenceFilter,
            this.agreedGuidanceFilter,
            this.techGrpResponseFilter,
            this.maxCompletionDateFilter,
            this.minCompletionDateFilter,
            this.requestStatusIdFilter,
            this.requestAreaRequestAreaNameFilter,
            this.requestDomainDomainNameFilter,
            this.userNameFilter,
            this.userName2Filter,
            this.userName3Filter,
            this.clientListClientNameFilter,
            this.userName4Filter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}

class DashboardHeaderStats extends DashboardChartBase {

    totalProfit = 0; totalProfitCounter = 0;
    newFeedbacks = 0; newFeedbacksCounter = 0;
    newOrders = 0; newOrdersCounter = 0;
    newUsers = 0; newUsersCounter = 0;

    totalProfitChange = 100; totalProfitChangeCounter = 0;
    newFeedbacksChange = 85; newFeedbacksChangeCounter = 0;
    newOrdersChange = 45; newOrdersChangeCounter = 0;
    newUsersChange = 57; newUsersChangeCounter = 0;

    init(totalProfit, newFeedbacks, newOrders, newUsers) {
        this.totalProfit = totalProfit;
        this.newFeedbacks = newFeedbacks;
        this.newOrders = newOrders;
        this.newUsers = newUsers;
        //additional
        this.totalProfitChange = (totalProfit / totalProfit) * 100;
        this.newFeedbacksChange = (newFeedbacks / totalProfit) * 100;
        this.newOrdersChange = (newOrders / totalProfit) * 100;
        this.hideLoading();
    }
}

class DailySalesLineChart extends DashboardChartBase {

    chartData: any[];
    scheme: any = {
        name: 'Requests',
        selectable: true,
        group: 'Ordinal',
        domain: [
            '#D85604', '#F3BE26', '#34bfa3'
        ]
    };
    showXAxis = true;
    showYAxis = true;
    gradient = false;
    showLegend = true;
    showXAxisLabel = true;
    xAxisLabel = 'Status';
    showYAxisLabel = true;
    yAxisLabel = 'Requests';

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(data) {
        console.log(data);
        this.chartData = [];
        if (data.status.length > 0) {
            for (let i = 0; i < data.status.length; i++) {
                this.chartData.push({
                    name: data.status[i],
                    series: [
                        {
                            name: 'Consultation',
                            value: data.consultation[i]
                        },
                        {
                            name: 'Enquiry',
                            value: data.enquiry[i]
                        },
                        {
                            name: 'FS Review',
                            value: data.fS_Review[i]
                        }]
                });
            }
        }
    }

    reload() {
        this.showLoading();
        this._dashboardService
            .getDashboardData(SalesSummaryDatePeriod.Daily)
            .subscribe(result => {
                console.log(result);
                this.init(result.requestStatusStat);
                this.hideLoading();
            });
    }
}

class GeneralStatsPieChart extends DashboardChartBase {

    public data = [];
    scheme: any = {
        name: 'Requests',
        selectable: true,
        group: 'Ordinal',
        domain: [
            '#F44336', '#FFC107', '#4CAF50'
        ]
    };

    constructor(private _dashboardService: TenantDashboardServiceProxy) {
        super();
    }

    init(transactionPercent, newVisitPercent, bouncePercent) {
        this.data = [
            {
                'name': 'Outstanding',
                'value': bouncePercent
            }, {
                'name': 'WIP',
                'value': transactionPercent
            }, {
                'name': 'Completed',
                'value': newVisitPercent
            }];

        this.hideLoading();
    }

    reload() {
        this.showLoading();
        this._dashboardService
            .getDashboardData(SalesSummaryDatePeriod.Daily)
            .subscribe(result => {
                this.init(result.transactionPercent, result.newVisitPercent, result.bouncePercent);
            });
    }
}
