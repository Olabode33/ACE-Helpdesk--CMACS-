import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TechTeamDto, TechTeamsServiceProxy, TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTechTeamModalComponent } from './create-or-edit-techTeam-modal.component';
import { ViewTechTeamModalComponent } from './view-techTeam-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { NotifyService } from 'abp-ng2-module';
import { Table, Paginator, LazyLoadEvent } from 'primeng';

@Component({
    templateUrl: './techTeams.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class TechTeamsComponent extends AppComponentBase {

    @ViewChild('createOrEditTechTeamModal') createOrEditTechTeamModal: CreateOrEditTechTeamModalComponent;
    @ViewChild('viewTechTeamModalComponent') viewTechTeamModal: ViewTechTeamModalComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxTimeChargeFilter: number;
    maxTimeChargeFilterEmpty: number;
    minTimeChargeFilter: number;
    minTimeChargeFilterEmpty: number;
    roleFilter = -1;
    requestLocalChargeCodeFilter = '';
    userNameFilter = '';

    // staffCategory = TechTeamDtoRole;


    constructor(
        injector: Injector,
        private _techTeamsServiceProxy: TechTeamsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getTechTeams(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._techTeamsServiceProxy.getAll(
            this.filterText,
            this.maxTimeChargeFilter == null ? this.maxTimeChargeFilterEmpty : this.maxTimeChargeFilter,
            this.minTimeChargeFilter == null ? this.minTimeChargeFilterEmpty : this.minTimeChargeFilter,
            this.roleFilter,
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

    createTechTeam(): void {
        this.createOrEditTechTeamModal.show();
    }

    deleteTechTeam(techTeam: TechTeamDto): void {
        this.message.confirm(
            '', '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._techTeamsServiceProxy.delete(techTeam.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._techTeamsServiceProxy.getTechTeamsToExcel(
            this.filterText,
            this.maxTimeChargeFilter == null ? this.maxTimeChargeFilterEmpty : this.maxTimeChargeFilter,
            this.minTimeChargeFilter == null ? this.minTimeChargeFilterEmpty : this.minTimeChargeFilter,
            this.roleFilter,
            this.requestLocalChargeCodeFilter,
            this.userNameFilter,
        )
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
