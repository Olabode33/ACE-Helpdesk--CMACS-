import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RequestApprovalsServiceProxy, RequestApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestApprovalModalComponent } from './create-or-edit-requestApproval-modal.component';
import { ViewRequestApprovalModalComponent } from './view-requestApproval-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requestApprovals.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RequestApprovalsComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestApprovalModal') createOrEditRequestApprovalModal: CreateOrEditRequestApprovalModalComponent;
    @ViewChild('viewRequestApprovalModalComponent') viewRequestApprovalModal: ViewRequestApprovalModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxApprovalSentTimeFilter: moment.Moment;
    minApprovalSentTimeFilter: moment.Moment;
    approvedFilter = -1;
    maxApprovedTimeFilter: moment.Moment;
    minApprovedTimeFilter: moment.Moment;
    requestRequestNoFilter = '';
    userNameFilter = '';



    constructor(
        injector: Injector,
        private _requestApprovalsServiceProxy: RequestApprovalsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequestApprovals(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestApprovalsServiceProxy.getAll(
            this.filterText,
            this.maxApprovalSentTimeFilter,
            this.minApprovalSentTimeFilter,
            this.approvedFilter,
            this.maxApprovedTimeFilter,
            this.minApprovedTimeFilter,
            this.requestRequestNoFilter,
            this.userNameFilter,
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

    createRequestApproval(): void {
        this.createOrEditRequestApprovalModal.show();
    }

    deleteRequestApproval(requestApproval: RequestApprovalDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestApprovalsServiceProxy.delete(requestApproval.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._requestApprovalsServiceProxy.getRequestApprovalsToExcel(
            this.filterText,
            this.maxApprovalSentTimeFilter,
            this.minApprovalSentTimeFilter,
            this.approvedFilter,
            this.maxApprovedTimeFilter,
            this.minApprovedTimeFilter,
            this.requestRequestNoFilter,
            this.userNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
