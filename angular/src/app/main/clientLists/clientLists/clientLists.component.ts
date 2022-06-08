import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ChannelType, ClientListDto, ClientListsServiceProxy, FinYearEnd, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditClientListModalComponent } from './create-or-edit-clientList-modal.component';
import { ViewClientListModalComponent } from './view-clientList-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './clientLists.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ClientListsComponent extends AppComponentBase {

    @ViewChild('createOrEditClientListModal') createOrEditClientListModal: CreateOrEditClientListModalComponent;
    @ViewChild('viewClientListModalComponent') viewClientListModal: ViewClientListModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    clientNameFilter = '';
    clientAddressFilter = '';
    parentEntityFilter = '';
    ultimateParentEntityFilter = '';
    secRegisteredFilter = -1;
    financialYearEndFilter = -1;
    channelTypeNameFilter = -1;
    industryIndustryNameFilter = '';
    reportingTerritoryTerritoryNameFilter = '';
    stockExchangeStockExchangeNameFilter = '';

    finYearEnd = FinYearEnd;
    channelType = ChannelType;


    constructor(
        injector: Injector,
        private _clientListsServiceProxy: ClientListsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getClientLists(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._clientListsServiceProxy.getAll(
            this.filterText,
            this.clientNameFilter,
            this.clientAddressFilter,
            this.parentEntityFilter,
            this.ultimateParentEntityFilter,
            this.secRegisteredFilter,
            this.financialYearEndFilter,
            this.channelTypeNameFilter,
            this.industryIndustryNameFilter,
            this.reportingTerritoryTerritoryNameFilter,
            this.stockExchangeStockExchangeNameFilter,
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

    createClientList(): void {
        this.createOrEditClientListModal.show();
    }

    deleteClientList(clientList: ClientListDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._clientListsServiceProxy.delete(clientList.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._clientListsServiceProxy.getClientListsToExcel(
            this.filterText,
            this.clientNameFilter,
            this.clientAddressFilter,
            this.parentEntityFilter,
            this.ultimateParentEntityFilter,
            this.secRegisteredFilter,
            this.financialYearEndFilter,
            this.channelTypeNameFilter,
            this.industryIndustryNameFilter,
            this.reportingTerritoryTerritoryNameFilter,
            this.stockExchangeStockExchangeNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
