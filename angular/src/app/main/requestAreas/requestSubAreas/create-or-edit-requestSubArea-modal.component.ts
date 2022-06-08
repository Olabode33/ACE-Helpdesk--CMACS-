import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestSubAreasServiceProxy, CreateOrEditRequestSubAreaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { RequestSubAreaRequestAreaLookupTableModalComponent } from './requestSubArea-requestArea-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'createOrEditRequestSubAreaModal',
    templateUrl: './create-or-edit-requestSubArea-modal.component.html'
})
export class CreateOrEditRequestSubAreaModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestSubAreaRequestAreaLookupTableModal') requestSubAreaRequestAreaLookupTableModal: RequestSubAreaRequestAreaLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestSubArea: CreateOrEditRequestSubAreaDto = new CreateOrEditRequestSubAreaDto();

    requestAreaRequestAreaName = '';


    constructor(
        injector: Injector,
        private _requestSubAreasServiceProxy: RequestSubAreasServiceProxy
    ) {
        super(injector);
    }

    show(requestSubAreaId?: number): void {

        if (!requestSubAreaId) {
            this.requestSubArea = new CreateOrEditRequestSubAreaDto();
            this.requestSubArea.id = requestSubAreaId;
            this.requestAreaRequestAreaName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._requestSubAreasServiceProxy.getRequestSubAreaForEdit(requestSubAreaId).subscribe(result => {
                this.requestSubArea = result.requestSubArea;

                this.requestAreaRequestAreaName = result.requestAreaRequestAreaName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestSubAreasServiceProxy.createOrEdit(this.requestSubArea)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestAreaModal() {
        this.requestSubAreaRequestAreaLookupTableModal.id = this.requestSubArea.requestAreaId;
        this.requestSubAreaRequestAreaLookupTableModal.displayName = this.requestAreaRequestAreaName;
        this.requestSubAreaRequestAreaLookupTableModal.show();
    }


    setRequestAreaIdNull() {
        this.requestSubArea.requestAreaId = null;
        this.requestAreaRequestAreaName = '';
    }


    getNewRequestAreaId() {
        this.requestSubArea.requestAreaId = this.requestSubAreaRequestAreaLookupTableModal.id;
        this.requestAreaRequestAreaName = this.requestSubAreaRequestAreaLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
