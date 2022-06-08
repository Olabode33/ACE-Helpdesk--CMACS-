import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TORApprovalsServiceProxy, TORApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTORApprovalModalComponent } from './create-or-edit-torApproval-modal.component';
import { ViewTORApprovalModalComponent } from './view-torApproval-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './torApprovals.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TORApprovalsComponent extends AppComponentBase {

    @ViewChild('createOrEditTORApprovalModal') createOrEditTORApprovalModal: CreateOrEditTORApprovalModalComponent;
    @ViewChild('viewTORApprovalModalComponent') viewTORApprovalModal: ViewTORApprovalModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTORTimeSentFilter: moment.Moment;
    minTORTimeSentFilter: moment.Moment;
    approvedFilter = -1;
    maxApprovedTimeFilter: moment.Moment;
    minApprovedTimeFilter: moment.Moment;
    userNameFilter = '';
    requestRequestNoFilter = '';



    constructor(
        injector: Injector,
        private _torApprovalsServiceProxy: TORApprovalsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTORApprovals(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._torApprovalsServiceProxy.getAll(
            this.filterText,
            this.maxTORTimeSentFilter,
            this.minTORTimeSentFilter,
            this.approvedFilter,
            this.maxApprovedTimeFilter,
            this.minApprovedTimeFilter,
            this.userNameFilter,
            this.requestRequestNoFilter,
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

    createTORApproval(): void {
        this.createOrEditTORApprovalModal.show();
    }

    deleteTORApproval(torApproval: TORApprovalDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._torApprovalsServiceProxy.delete(torApproval.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._torApprovalsServiceProxy.getTORApprovalsToExcel(
            this.filterText,
            this.maxTORTimeSentFilter,
            this.minTORTimeSentFilter,
            this.approvedFilter,
            this.maxApprovedTimeFilter,
            this.minApprovedTimeFilter,
            this.userNameFilter,
            this.requestRequestNoFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
