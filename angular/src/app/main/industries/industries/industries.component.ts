import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IndustriesServiceProxy, IndustryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditIndustryModalComponent } from './create-or-edit-industry-modal.component';
import { ViewIndustryModalComponent } from './view-industry-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './industries.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class IndustriesComponent extends AppComponentBase {

    @ViewChild('createOrEditIndustryModal') createOrEditIndustryModal: CreateOrEditIndustryModalComponent;
    @ViewChild('viewIndustryModalComponent') viewIndustryModal: ViewIndustryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    industryNameFilter = '';

    constructor(
        injector: Injector,
        private _industriesServiceProxy: IndustriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getIndustries(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._industriesServiceProxy.getAll(
            this.filterText,
            this.industryNameFilter,
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

    createIndustry(): void {
        this.createOrEditIndustryModal.show();
    }

    deleteIndustry(industry: IndustryDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._industriesServiceProxy.delete(industry.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._industriesServiceProxy.getIndustriesToExcel(
            this.filterText,
            this.industryNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
