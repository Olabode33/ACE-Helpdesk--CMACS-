import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AttachedDocsServiceProxy, AttachedDocDto, StaffEntityType } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAttachedDocModalComponent } from './create-or-edit-attachedDoc-modal.component';
import { ViewAttachedDocModalComponent } from './view-attachedDoc-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './attachedDocs.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class AttachedDocsComponent extends AppComponentBase {

    @ViewChild('createOrEditAttachedDocModal') createOrEditAttachedDocModal: CreateOrEditAttachedDocModalComponent;
    @ViewChild('viewAttachedDocModalComponent') viewAttachedDocModal: ViewAttachedDocModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    fileNameFilter = '';
    docOwnerTypeIdFilter = -1;
    requestRequestNoFilter = '';
    userNameFilter = '';
    binaryObjectTenantIdFilter = '';

    staffEntityType = StaffEntityType;


    constructor(
        injector: Injector,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getAttachedDocs(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._attachedDocsServiceProxy.getAll(
            this.filterText,
            this.fileNameFilter,
            this.docOwnerTypeIdFilter,
            this.requestRequestNoFilter,
            this.userNameFilter,
            this.binaryObjectTenantIdFilter,
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

    createAttachedDoc(): void {
        this.createOrEditAttachedDocModal.show();
    }

    deleteAttachedDoc(attachedDoc: AttachedDocDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._attachedDocsServiceProxy.delete(attachedDoc.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._attachedDocsServiceProxy.getAttachedDocsToExcel(
            this.filterText,
            this.fileNameFilter,
            this.docOwnerTypeIdFilter,
            this.requestRequestNoFilter,
            this.userNameFilter,
            this.binaryObjectTenantIdFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
