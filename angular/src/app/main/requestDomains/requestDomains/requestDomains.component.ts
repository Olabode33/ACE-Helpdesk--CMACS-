import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestDomainDto, RequestDomainsServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditRequestDomainModalComponent } from './create-or-edit-requestDomain-modal.component';
import { ViewRequestDomainModalComponent } from './view-requestDomain-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './requestDomains.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class RequestDomainsComponent extends AppComponentBase {

    @ViewChild('createOrEditRequestDomainModal') createOrEditRequestDomainModal: CreateOrEditRequestDomainModalComponent;
    @ViewChild('viewRequestDomainModalComponent') viewRequestDomainModal: ViewRequestDomainModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    domainNameFilter = '';



    constructor(
        injector: Injector,
        private _requestDomainsServiceProxy: RequestDomainsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getRequestDomains(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestDomainsServiceProxy.getAll(
            this.filterText,
            this.domainNameFilter,
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

    createRequestDomain(): void {
        this.createOrEditRequestDomainModal.show();
    }

    deleteRequestDomain(requestDomain: RequestDomainDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestDomainsServiceProxy.delete(requestDomain.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._requestDomainsServiceProxy.getRequestDomainsToExcel(
            this.filterText,
            this.domainNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
