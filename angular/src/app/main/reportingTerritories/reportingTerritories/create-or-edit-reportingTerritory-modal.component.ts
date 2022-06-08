import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { ReportingTerritoriesServiceProxy, CreateOrEditReportingTerritoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditReportingTerritoryModal',
    templateUrl: './create-or-edit-reportingTerritory-modal.component.html'
})
export class CreateOrEditReportingTerritoryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    reportingTerritory: CreateOrEditReportingTerritoryDto = new CreateOrEditReportingTerritoryDto();


    constructor(
        injector: Injector,
        private _reportingTerritoriesServiceProxy: ReportingTerritoriesServiceProxy
    ) {
        super(injector);
    }

    show(reportingTerritoryId?: number): void {
        if (!reportingTerritoryId) {
            this.reportingTerritory = new CreateOrEditReportingTerritoryDto();
            this.reportingTerritory.id = reportingTerritoryId;

            this.active = true;
            this.modal.show();
        } else {
            this._reportingTerritoriesServiceProxy.getReportingTerritoryForEdit(reportingTerritoryId).subscribe(result => {
                this.reportingTerritory = result.reportingTerritory;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._reportingTerritoriesServiceProxy.createOrEdit(this.reportingTerritory)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
