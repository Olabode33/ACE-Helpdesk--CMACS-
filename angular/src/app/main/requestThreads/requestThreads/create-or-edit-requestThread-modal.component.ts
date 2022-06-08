import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestThreadsServiceProxy, CreateOrEditRequestThreadDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestLookupTableModalComponent } from '@app/main/techTeams/techTeams/request-lookup-table-modal.component';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditRequestThreadModal',
    templateUrl: './create-or-edit-requestThread-modal.component.html'
})
export class CreateOrEditRequestThreadModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    requestThread: CreateOrEditRequestThreadDto = new CreateOrEditRequestThreadDto();
    requestLocalChargeCode = '';
    userName = '';


    constructor(
        injector: Injector,
        private _requestThreadsServiceProxy: RequestThreadsServiceProxy
    ) {
        super(injector);
    }

    show(requestThreadId?: number): void {
        if (!requestThreadId) {
            this.requestThread = new CreateOrEditRequestThreadDto();
            this.requestThread.id = requestThreadId;
            this.requestLocalChargeCode = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._requestThreadsServiceProxy.getRequestThreadForEdit(requestThreadId).subscribe(result => {
                this.requestThread = result.requestThread;
                this.requestLocalChargeCode = result.requestLocalChargeCode;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._requestThreadsServiceProxy.createOrEdit(this.requestThread)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.requestThread.requestId;
        this.requestLookupTableModal.displayName = this.requestLocalChargeCode;
        this.requestLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.requestThread.commentById;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


    setRequestIdNull() {
        this.requestThread.requestId = null;
        this.requestLocalChargeCode = '';
    }
    setCommentByIdNull() {
        this.requestThread.commentById = null;
        this.userName = '';
    }


    getNewRequestId() {
        this.requestThread.requestId = this.requestLookupTableModal.id;
        this.requestLocalChargeCode = this.requestLookupTableModal.displayName;
    }
    getNewCommentById() {
        this.requestThread.commentById = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
