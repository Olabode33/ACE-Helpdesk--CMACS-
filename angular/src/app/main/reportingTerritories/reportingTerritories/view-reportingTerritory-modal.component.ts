import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetReportingTerritoryForView, ReportingTerritoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewReportingTerritoryModal',
    templateUrl: './view-reportingTerritory-modal.component.html'
})
export class ViewReportingTerritoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetReportingTerritoryForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetReportingTerritoryForView();
        this.item.reportingTerritory = new ReportingTerritoryDto();
    }

    show(item: GetReportingTerritoryForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
