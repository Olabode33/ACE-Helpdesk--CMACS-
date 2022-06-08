import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestApprovalsServiceProxy, CreateOrEditRequestApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestLookupTableModalComponent } from '@app/main/techTeams/techTeams/request-lookup-table-modal.component';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditRequestApprovalModal',
    templateUrl: './create-or-edit-requestApproval-modal.component.html'
})
export class CreateOrEditRequestApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestApproval: CreateOrEditRequestApprovalDto = new CreateOrEditRequestApprovalDto();
    requestRequestNo = '';
    userName = '';


    constructor(
        injector: Injector,
        private _requestApprovalsServiceProxy: RequestApprovalsServiceProxy
    ) {
        super(injector);
    }

    show(requestApprovalId?: number): void {
        if (!requestApprovalId) {
            this.requestApproval = new CreateOrEditRequestApprovalDto();
            this.requestApproval.id = requestApprovalId;
            this.requestRequestNo = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._requestApprovalsServiceProxy.getRequestApprovalForEdit(requestApprovalId).subscribe(result => {
                this.requestApproval = result.requestApproval;
                this.requestRequestNo = result.requestRequestNo;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestApprovalsServiceProxy.createOrEdit(this.requestApproval)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.requestApproval.requestId;
        this.requestLookupTableModal.displayName = this.requestRequestNo;
        this.requestLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.requestApproval.approverId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


    setRequestIdNull() {
        this.requestApproval.requestId = null;
        this.requestRequestNo = '';
    }
    setApproverIdNull() {
        this.requestApproval.approverId = null;
        this.userName = '';
    }


    getNewRequestId() {
        this.requestApproval.requestId = this.requestLookupTableModal.id;
        this.requestRequestNo = this.requestLookupTableModal.displayName;
    }
    getNewApproverId() {
        this.requestApproval.approverId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
