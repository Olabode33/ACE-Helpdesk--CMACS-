import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReportingTerritoriesServiceProxy, ReportingTerritoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditReportingTerritoryModalComponent } from './create-or-edit-reportingTerritory-modal.component';
import { ViewReportingTerritoryModalComponent } from './view-reportingTerritory-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './reportingTerritories.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ReportingTerritoriesComponent extends AppComponentBase {

    @ViewChild('createOrEditReportingTerritoryModal') createOrEditReportingTerritoryModal: CreateOrEditReportingTerritoryModalComponent;
    @ViewChild('viewReportingTerritoryModalComponent') viewReportingTerritoryModal: ViewReportingTerritoryModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    territoryNameFilter = '';

    constructor(
        injector: Injector,
        private _reportingTerritoriesServiceProxy: ReportingTerritoriesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getReportingTerritories(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._reportingTerritoriesServiceProxy.getAll(
            this.filterText,
            this.territoryNameFilter,
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

    createReportingTerritory(): void {
        this.createOrEditReportingTerritoryModal.show();
    }

    deleteReportingTerritory(reportingTerritory: ReportingTerritoryDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._reportingTerritoriesServiceProxy.delete(reportingTerritory.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._reportingTerritoriesServiceProxy.getReportingTerritoriesToExcel(
            this.filterText,
            this.territoryNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
