import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RequestsServiceProxy, RequestDto, RequestStatus } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestModalComponent } from '@app/main/requests/requests/create-or-edit-request-modal.component';
import { ViewRequestModalComponent } from '@app/main/requests/requests/view-request-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './assignRequests.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AssignRequestsComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestModal') createOrEditRequestModal: CreateOrEditRequestModalComponent;
    @ViewChild('viewRequestModalComponent') viewRequestModal: ViewRequestModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
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



    requestStatus = RequestStatus;


    constructor(
        injector: Injector,
        // private _http: Http,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequests(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestsServiceProxy.getAll(
            this.filterText,
            /*
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

            this.requestAreaRequestAreaNameFilter,
            this.requestDomainDomainNameFilter,
            this.userNameFilter,
            this.userName2Filter,
            this.userName3Filter,
            this.clientListClientNameFilter,
            this.userName4Filter,
            */
            this.requestStatusIdFilter = 0,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createRequest(): void {
        this.createOrEditRequestModal.show();
    }

    deleteRequest(request: RequestDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.delete(request.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    markAsCompleted(request: RequestDto): void {
        console.log(request.id);
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.markTreated(request.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyCompleted'));
                        });
                }
            }
        );
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
