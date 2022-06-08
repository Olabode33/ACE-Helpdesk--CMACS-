import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StockExchangesServiceProxy, StockExchangeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditStockExchangeModalComponent } from './create-or-edit-stockExchange-modal.component';
import { ViewStockExchangeModalComponent } from './view-stockExchange-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './stockExchanges.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StockExchangesComponent extends AppComponentBase {

    @ViewChild('createOrEditStockExchangeModal') createOrEditStockExchangeModal: CreateOrEditStockExchangeModalComponent;
    @ViewChild('viewStockExchangeModalComponent') viewStockExchangeModal: ViewStockExchangeModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    stockExchangeNameFilter = '';
    countryFilter = '';



    constructor(
        injector: Injector,
        private _stockExchangesServiceProxy: StockExchangesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getStockExchanges(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._stockExchangesServiceProxy.getAll(
            this.filterText,
            this.stockExchangeNameFilter,
            this.countryFilter,
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

    createStockExchange(): void {
        this.createOrEditStockExchangeModal.show();
    }

    deleteStockExchange(stockExchange: StockExchangeDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._stockExchangesServiceProxy.delete(stockExchange.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._stockExchangesServiceProxy.getStockExchangesToExcel(
            this.filterText,
            this.stockExchangeNameFilter,
            this.countryFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
