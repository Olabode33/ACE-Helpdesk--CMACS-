import { StatusClassHelper } from './../../_custom_helpers/statusClasshelper';
import { RequestStatusEnum } from '@app/main/_custom_helpers/RequestStatus.enum';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestDto, RequestsServiceProxy, RequestStatus, RequestType, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestModalComponent } from './create-or-edit-request-modal.component';
import { ViewRequestModalComponent } from './view-request-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { Location } from '@angular/common';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requests.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})

export class RequestsComponent extends AppComponentBase {

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
    requestType = RequestType;

    requestStatusEnum = RequestStatusEnum;

    statusClassHelper: StatusClassHelper = new StatusClassHelper();

    constructor(
        injector: Injector,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _router: Router,
        private _location: Location
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
            this.requestStatusEnum.Completed,
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
        this._router.navigate(['../create'], { relativeTo: this._activatedRoute });
        //this.createOrEditRequestModal.show();
    }

    deleteRequest(request: RequestDto): void {
        if (request.requestStatusId !== 0) {
            this.notify.error('Request in progress, cannot be deleted');
            return;
        }

        this.message.confirm(
            '', 'Are you sure you want to delete?',
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

    editRequest(request: RequestDto): void {
        if (request.requestStatusId !== 0) {
            this.notify.error('Request in progress, cannot be edited');
            return;
        }

        this.navigateToCreateOrEdit(request.id, 0);
    }

    assignRequest(request: RequestDto): void {

        if (request.requestStatusId != 0 && request.requestStatusId != 1) {
            this.notify.error('This request should be in requested state');
            return;
        }

        this.navigateToTreatRequestPage(request.id, 1);
    }

    createTOR(request: RequestDto): void {

        if (request.requestTypeId == 1) {
            this.notify.error('You don\'t need a TOR for an enquiry');
            return;
        }

        if (request.requestStatusId != 1) {
            this.notify.error('This request has not been assigned');
            return;
        }

        this.navigateToTreatRequestPage(request.id, 3);
    }

    approveTOR(request: RequestDto): void {

        if (request.requestStatusId != 5) {
            this.notify.error('This request is not awaiting TOR');
            return;
        }

        if (request.requestTypeId == 1) {
            this.notify.error('You don\'t need a TOR for an enquiry');
            return;
        }

        this.navigateToTreatRequestPage(request.id, 4);
    }

    treatRequest(request: RequestDto): void {

        if (request.requestTypeId == 0 && !request.termsOfRefApproved) {
            this.notify.error('TOR has not been approved, request cannot be treated');
            return;
        }

        if (request.requestTypeId == 0 && request.requestStatusId != 6) {
            this.notify.error('This request is not in WIP');
            return;
        }

        this.navigateToTreatRequestPage(request.id, 2);
    }

    sendRequestForApproval(request: RequestDto): void {

        if (request.requestStatusId != 6) {
            this.notify.error('This request is not in WIP');
            return;
        }

        this.message.confirm(
            '', 'Please confirm request can be sent for approval',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.sendRequestForApproval(request.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('RequestApprovalSuccessful'));
                        });
                }
            }
        );
    }

    approveRequest(request: RequestDto): void {

        if (request.requestStatusId != 2) {
            this.notify.error('This request has not been sent for approval');
            return;
        }

        this.navigateToTreatRequestPage(request.id, 5);
    }

    markAsCompleted(request: RequestDto): void {

        if (request.requestTypeId == 0) {
            if (!request.termsOfRefApproved) {
                this.notify.error('The TOR has not been approved');
                return;
            }

            if (request.requestStatusId != 7) {
                this.notify.error('This request has not been approved');
                return;
            }
        }

        if (request.requestTypeId == 1) {

            if (request.requestStatusId == 0) {
                this.notify.error('This enquiry was never assigned');
                return;
            }
        }

        this.message.confirm(
            '', 'Please confirm the request can be marked as completed',
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

    getStatusClass(statusId: number): string {
        return this.statusClassHelper.getStatusClass(statusId);
    }

    navigateToCreateOrEdit(requestId: number, callType: number): void {
        this._router.navigate(['app/main/requests/edit', requestId, callType]);
        //this.createOrEditRequestModal.show(request.id, 2);
    }

    navigateToTreatRequestPage(requestId: number, callType: number): void {
        this._router.navigate(['app/main/requests/treat', requestId, callType]);
        //this.createOrEditRequestModal.show(requestId, 2);
    }

    navigateToViewRequestPage(requestId: number): void {
        this._router.navigate(['app/main/requests/view', requestId]);
    }

    goBack(): void {
        this._location.back();
    }
}
