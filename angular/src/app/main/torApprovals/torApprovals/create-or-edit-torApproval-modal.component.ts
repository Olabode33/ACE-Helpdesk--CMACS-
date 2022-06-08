import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { TORApprovalsServiceProxy, CreateOrEditTORApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { RequestLookupTableModalComponent } from '@app/main/techTeams/techTeams/request-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditTORApprovalModal',
    templateUrl: './create-or-edit-torApproval-modal.component.html'
})
export class CreateOrEditTORApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    torApproval: CreateOrEditTORApprovalDto = new CreateOrEditTORApprovalDto();
    userName = '';
    requestRequestNo = '';


    constructor(
        injector: Injector,
        private _torApprovalsServiceProxy: TORApprovalsServiceProxy
    ) {
        super(injector);
    }

    show(torApprovalId?: number): void {
        if (!torApprovalId) {
            this.torApproval = new CreateOrEditTORApprovalDto();
            this.torApproval.id = torApprovalId;
            this.userName = '';
            this.requestRequestNo = '';

            this.active = true;
            this.modal.show();
        } else {
            this._torApprovalsServiceProxy.getTORApprovalForEdit(torApprovalId).subscribe(result => {
                this.torApproval = result.torApproval;
                this.userName = result.userName;
                this.requestRequestNo = result.requestRequestNo;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._torApprovalsServiceProxy.createOrEdit(this.torApproval)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectUserModal() {
        this.userLookupTableModal.id = this.torApproval.approverId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }
    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.torApproval.requestId;
        this.requestLookupTableModal.displayName = this.requestRequestNo;
        this.requestLookupTableModal.show();
    }


    setApproverIdNull() {
        this.torApproval.approverId = null;
        this.userName = '';
    }
    setRequestIdNull() {
        this.torApproval.requestId = null;
        this.requestRequestNo = '';
    }


    getNewApproverId() {
        this.torApproval.approverId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }
    getNewRequestId() {
        this.torApproval.requestId = this.requestLookupTableModal.id;
        this.requestRequestNo = this.requestLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
