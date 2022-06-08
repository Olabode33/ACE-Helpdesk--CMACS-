import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { TechTeamsServiceProxy, CreateOrEditTechTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestLookupTableModalComponent } from './request-lookup-table-modal.component';
import { UserLookupTableModalComponent } from '@app/main/requests/requests/user-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditTechTeamModal',
    templateUrl: './create-or-edit-techTeam-modal.component.html'
})
export class CreateOrEditTechTeamModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestLookupTableModal') requestLookupTableModal: RequestLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    techTeam: CreateOrEditTechTeamDto = new CreateOrEditTechTeamDto();
    requestLocalChargeCode = '';
    userName = '';


    constructor(
        injector: Injector,
        private _techTeamsServiceProxy: TechTeamsServiceProxy
    ) {
        super(injector);
    }

    show(techTeamId?: number): void {
        if (!techTeamId) {
            this.techTeam = new CreateOrEditTechTeamDto();
            this.techTeam.id = techTeamId;
            this.requestLocalChargeCode = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._techTeamsServiceProxy.getTechTeamForEdit(techTeamId).subscribe(result => {
                this.techTeam = result.techTeam;
                this.requestLocalChargeCode = result.requestLocalChargeCode;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._techTeamsServiceProxy.createOrEdit(this.techTeam)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestModal() {
        this.requestLookupTableModal.id = this.techTeam.requestId;
        this.requestLookupTableModal.displayName = this.requestLocalChargeCode;
        this.requestLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.techTeam.cmacsUserId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


    setRequestIdNull() {
        this.techTeam.requestId = null;
        this.requestLocalChargeCode = '';
    }
    setCMACSUserIdNull() {
        this.techTeam.cmacsUserId = null;
        this.userName = '';
    }


    getNewRequestId() {
        this.techTeam.requestId = this.requestLookupTableModal.id;
        this.requestLocalChargeCode = this.requestLookupTableModal.displayName;
    }
    getNewCMACSUserId() {
        this.techTeam.cmacsUserId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
