import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ClientListsServiceProxy, CreateOrEditClientListDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IndustryLookupTableModalComponent } from './industry-lookup-table-modal.component';
import { ReportingTerritoryLookupTableModalComponent } from './reportingTerritory-lookup-table-modal.component';
import { StockExchangeLookupTableModalComponent } from './stockExchange-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditClientListModal',
    templateUrl: './create-or-edit-clientList-modal.component.html'
})
export class CreateOrEditClientListModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('industryLookupTableModal') industryLookupTableModal: IndustryLookupTableModalComponent;
    @ViewChild('reportingTerritoryLookupTableModal') reportingTerritoryLookupTableModal: ReportingTerritoryLookupTableModalComponent;
    @ViewChild('stockExchangeLookupTableModal') stockExchangeLookupTableModal: StockExchangeLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    clientList: CreateOrEditClientListDto = new CreateOrEditClientListDto();
    industryIndustryName = '';
    reportingTerritoryTerritoryName = '';
    stockExchangeStockExchangeName = '';


    constructor(
        injector: Injector,
        private _clientListsServiceProxy: ClientListsServiceProxy
    ) {
        super(injector);
    }

    show(clientListId?: number): void {
        if (!clientListId) {
            this.clientList = new CreateOrEditClientListDto();
            this.clientList.id = clientListId;
            this.industryIndustryName = '';
            this.reportingTerritoryTerritoryName = '';
            this.stockExchangeStockExchangeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._clientListsServiceProxy.getClientListForEdit(clientListId).subscribe(result => {
                this.clientList = result.clientList;
                this.industryIndustryName = result.industryIndustryName;
                this.reportingTerritoryTerritoryName = result.reportingTerritoryTerritoryName;
                this.stockExchangeStockExchangeName = result.stockExchangeStockExchangeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._clientListsServiceProxy.createOrEdit(this.clientList)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(result => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(result);
            });
    }

    openSelectIndustryModal() {
        this.industryLookupTableModal.id = this.clientList.industryId;
        this.industryLookupTableModal.displayName = this.industryIndustryName;
        this.industryLookupTableModal.show();
    }
    openSelectReportingTerritoryModal() {
        this.reportingTerritoryLookupTableModal.id = this.clientList.reportingTerritoryId;
        this.reportingTerritoryLookupTableModal.displayName = this.reportingTerritoryTerritoryName;
        this.reportingTerritoryLookupTableModal.show();
    }
    openSelectStockExchangeModal() {
        this.stockExchangeLookupTableModal.id = this.clientList.stockExchangeId;
        this.stockExchangeLookupTableModal.displayName = this.stockExchangeStockExchangeName;
        this.stockExchangeLookupTableModal.show();
    }


    setIndustryIdNull() {
        this.clientList.industryId = null;
        this.industryIndustryName = '';
    }
    setReportingTerritoryIdNull() {
        this.clientList.reportingTerritoryId = null;
        this.reportingTerritoryTerritoryName = '';
    }
    setStockExchangeIdNull() {
        this.clientList.stockExchangeId = null;
        this.stockExchangeStockExchangeName = '';
    }


    getNewIndustryId() {
        this.clientList.industryId = this.industryLookupTableModal.id;
        this.industryIndustryName = this.industryLookupTableModal.displayName;
    }
    getNewReportingTerritoryId() {
        this.clientList.reportingTerritoryId = this.reportingTerritoryLookupTableModal.id;
        this.reportingTerritoryTerritoryName = this.reportingTerritoryLookupTableModal.displayName;
    }
    getNewStockExchangeId() {
        this.clientList.stockExchangeId = this.stockExchangeLookupTableModal.id;
        this.stockExchangeStockExchangeName = this.stockExchangeLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
