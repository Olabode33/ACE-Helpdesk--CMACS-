import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestDomainsServiceProxy, CreateOrEditRequestDomainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditRequestDomainModal',
    templateUrl: './create-or-edit-requestDomain-modal.component.html'
})
export class CreateOrEditRequestDomainModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestDomain: CreateOrEditRequestDomainDto = new CreateOrEditRequestDomainDto();


    constructor(
        injector: Injector,
        private _requestDomainsServiceProxy: RequestDomainsServiceProxy
    ) {
        super(injector);
    }

    show(requestDomainId?: number): void {
        if (!requestDomainId) {
            this.requestDomain = new CreateOrEditRequestDomainDto();
            this.requestDomain.id = requestDomainId;

            this.active = true;
            this.modal.show();
        } else {
            this._requestDomainsServiceProxy.getRequestDomainForEdit(requestDomainId).subscribe(result => {
                this.requestDomain = result.requestDomain;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestDomainsServiceProxy.createOrEdit(this.requestDomain)
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
