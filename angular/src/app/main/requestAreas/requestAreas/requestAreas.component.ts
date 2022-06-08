import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RequestAreasServiceProxy, RequestAreaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestAreaModalComponent } from './create-or-edit-requestArea-modal.component';
import { ViewRequestAreaModalComponent } from './view-requestArea-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requestAreas.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RequestAreasComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestAreaModal') createOrEditRequestAreaModal: CreateOrEditRequestAreaModalComponent;
    @ViewChild('viewRequestAreaModalComponent') viewRequestAreaModal: ViewRequestAreaModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    requestAreaNameFilter = '';



    constructor(
        injector: Injector,
        private _requestAreasServiceProxy: RequestAreasServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequestAreas(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestAreasServiceProxy.getAll(
            this.filterText,
            this.requestAreaNameFilter,
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

    createRequestArea(): void {
        this.createOrEditRequestAreaModal.show();
    }

    deleteRequestArea(requestArea: RequestAreaDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestAreasServiceProxy.delete(requestArea.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._requestAreasServiceProxy.getRequestAreasToExcel(
            this.filterText,
            this.requestAreaNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
