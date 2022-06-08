import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestDocDto, RequestDocsServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestDocModalComponent } from './create-or-edit-requestDoc-modal.component';
import { ViewRequestDocModalComponent } from './view-requestDoc-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requestDocs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RequestDocsComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestDocModal') createOrEditRequestDocModal: CreateOrEditRequestDocModalComponent;
    @ViewChild('viewRequestDocModalComponent') viewRequestDocModal: ViewRequestDocModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    documentNameFilter = '';
    documentLocationFilter = '';
    preparerTypeIdFilter = -1;
    documentGUIDFilter = '';
    requestLocalChargeCodeFilter = '';
    userNameFilter = '';

    // staffEntityType = RequestDocDtoPreparerTypeId;


    constructor(
        injector: Injector,
        private _requestDocsServiceProxy: RequestDocsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequestDocs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestDocsServiceProxy.getAll(
            this.filterText,
            this.documentNameFilter,
            this.documentLocationFilter,
            this.preparerTypeIdFilter,
            this.documentGUIDFilter,
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

    createRequestDoc(): void {
        this.createOrEditRequestDocModal.show();
    }

    deleteRequestDoc(requestDoc: RequestDocDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestDocsServiceProxy.delete(requestDoc.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._requestDocsServiceProxy.getRequestDocsToExcel(
            this.filterText,
            this.documentNameFilter,
            this.documentLocationFilter,
            this.preparerTypeIdFilter,
            this.documentGUIDFilter,
            this.requestLocalChargeCodeFilter,
            this.userNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
