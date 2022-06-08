import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestDocsServiceProxy, CreateOrEditRequestDocDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestLookupTableModalComponent } from '@app/main/techTeams/techTeams/request-lookup-table-modal.component';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditRequestDocModal',
    templateUrl: './create-or-edit-requestDoc-modal.component.html'
})
export class CreateOrEditRequestDocModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestDoc: CreateOrEditRequestDocDto = new CreateOrEditRequestDocDto();
    requestLocalChargeCode = '';
    userName = '';


    constructor(
        injector: Injector,
        private _requestDocsServiceProxy: RequestDocsServiceProxy
    ) {
        super(injector);
    }

    show(requestDocId?: number): void {
        if (!requestDocId) {
            this.requestDoc = new CreateOrEditRequestDocDto();
            this.requestDoc.id = requestDocId;
            this.requestLocalChargeCode = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._requestDocsServiceProxy.getRequestDocForEdit(requestDocId).subscribe(result => {
                this.requestDoc = result.requestDoc;
                this.requestLocalChargeCode = result.requestLocalChargeCode;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestDocsServiceProxy.createOrEdit(this.requestDoc)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.requestDoc.requestId;
        this.requestLookupTableModal.displayName = this.requestLocalChargeCode;
        this.requestLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.requestDoc.preparerId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


    setRequestIdNull() {
        this.requestDoc.requestId = null;
        this.requestLocalChargeCode = '';
    }
    setPreparerIdNull() {
        this.requestDoc.preparerId = null;
        this.userName = '';
    }


    getNewRequestId() {
        this.requestDoc.requestId = this.requestLookupTableModal.id;
        this.requestLocalChargeCode = this.requestLookupTableModal.displayName;
    }
    getNewPreparerId() {
        this.requestDoc.preparerId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
