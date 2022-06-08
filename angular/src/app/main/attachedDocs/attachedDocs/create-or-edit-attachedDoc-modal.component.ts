import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { AttachedDocsServiceProxy, CreateOrEditAttachedDocDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BinaryObjectLookupTableModalComponent } from './binaryObject-lookup-table-modal.component';
import { RequestLookupTableModalComponent } from '@app/main/techTeams/techTeams/request-lookup-table-modal.component';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditAttachedDocModal',
    templateUrl: './create-or-edit-attachedDoc-modal.component.html'
})
export class CreateOrEditAttachedDocModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;
    @ViewChild('binaryObjectLookupTableModal') binaryObjectLookupTableModal: BinaryObjectLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    attachedDoc: CreateOrEditAttachedDocDto = new CreateOrEditAttachedDocDto();
    requestRequestNo = '';
    userName = '';
    binaryObjectTenantId = '';


    constructor(
        injector: Injector,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy
    ) {
        super(injector);
    }

    show(attachedDocId?: number): void {
        if (!attachedDocId) {
            this.attachedDoc = new CreateOrEditAttachedDocDto();
            this.attachedDoc.id = attachedDocId;
            this.requestRequestNo = '';
            this.userName = '';
            this.binaryObjectTenantId = '';

            this.active = true;
            this.modal.show();
        } else {
            this._attachedDocsServiceProxy.getAttachedDocForEdit(attachedDocId).subscribe(result => {
                this.attachedDoc = result.attachedDoc;
                this.requestRequestNo = result.requestRequestNo;
                this.userName = result.userName;
                this.binaryObjectTenantId = result.binaryObjectTenantId;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._attachedDocsServiceProxy.createOrEdit(this.attachedDoc)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.attachedDoc.requestId;
        this.requestLookupTableModal.displayName = this.requestRequestNo;
        this.requestLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.attachedDoc.docOwnerId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }
    openSelectBinaryObjectModal() {
        this.binaryObjectLookupTableModal.id = this.attachedDoc.documentId;
        this.binaryObjectLookupTableModal.displayName = this.binaryObjectTenantId;
        this.binaryObjectLookupTableModal.show();
    }


    setRequestIdNull() {
        this.attachedDoc.requestId = null;
        this.requestRequestNo = '';
    }
    setDocOwnerIdNull() {
        this.attachedDoc.docOwnerId = null;
        this.userName = '';
    }
    setDocumentIdNull() {
        this.attachedDoc.documentId = null;
        this.binaryObjectTenantId = '';
    }


    getNewRequestId() {
        this.attachedDoc.requestId = this.requestLookupTableModal.id;
        this.requestRequestNo = this.requestLookupTableModal.displayName;
    }
    getNewDocOwnerId() {
        this.attachedDoc.docOwnerId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }
    getNewDocumentId() {
        this.attachedDoc.documentId = this.binaryObjectLookupTableModal.id;
        this.binaryObjectTenantId = this.binaryObjectLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
