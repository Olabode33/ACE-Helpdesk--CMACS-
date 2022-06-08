import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RequestThreadsServiceProxy, RequestThreadDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestThreadModalComponent } from './create-or-edit-requestThread-modal.component';
import { ViewRequestThreadModalComponent } from './view-requestThread-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requestThreads.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RequestThreadsComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestThreadModal') createOrEditRequestThreadModal: CreateOrEditRequestThreadModalComponent;
    @ViewChild('viewRequestThreadModalComponent') viewRequestThreadModal: ViewRequestThreadModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    commentFilter = '';
    maxCommentDateFilter: moment.Moment;
    minCommentDateFilter: moment.Moment;
    requestLocalChargeCodeFilter = '';
    userNameFilter = '';



    constructor(
        injector: Injector,
        private _requestThreadsServiceProxy: RequestThreadsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequestThreads(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestThreadsServiceProxy.getAll(
            this.filterText,
            this.commentFilter,
            this.maxCommentDateFilter,
            this.minCommentDateFilter,
            this.requestLocalChargeCodeFilter,
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

    createRequestThread(): void {
        this.createOrEditRequestThreadModal.show();
    }

    deleteRequestThread(requestThread: RequestThreadDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestThreadsServiceProxy.delete(requestThread.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._requestThreadsServiceProxy.getRequestThreadsToExcel(
            this.filterText,
            this.commentFilter,
            this.maxCommentDateFilter,
            this.minCommentDateFilter,
            this.requestLocalChargeCodeFilter,
            this.userNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
