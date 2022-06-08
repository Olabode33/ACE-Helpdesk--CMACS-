import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestAreasServiceProxy, CreateOrEditRequestAreaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditRequestAreaModal',
    templateUrl: './create-or-edit-requestArea-modal.component.html'
})
export class CreateOrEditRequestAreaModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestArea: CreateOrEditRequestAreaDto = new CreateOrEditRequestAreaDto();


    constructor(
        injector: Injector,
        private _requestAreasServiceProxy: RequestAreasServiceProxy
    ) {
        super(injector);
    }

    show(requestAreaId?: number): void {
        if (!requestAreaId) {
            this.requestArea = new CreateOrEditRequestAreaDto();
            this.requestArea.id = requestAreaId;

            this.active = true;
            this.modal.show();
        } else {
            this._requestAreasServiceProxy.getRequestAreaForEdit(requestAreaId).subscribe(result => {
                this.requestArea = result.requestArea;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestAreasServiceProxy.createOrEdit(this.requestArea)
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
