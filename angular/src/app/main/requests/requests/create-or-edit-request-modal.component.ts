import { AfterViewChecked, Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { RequestsServiceProxy, CreateOrEditRequestDto, TechTeamsServiceProxy, CreateOrEditTechTeamDto, TenantSettingsServiceProxy, DemoUiComponentsServiceProxy, AttachedDocsServiceProxy, CreateOrEditAttachedDocDto, CreateRequestTORDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RequestAreaLookupTableModalComponent } from './requestArea-lookup-table-modal.component';
import { RequestDomainLookupTableModalComponent } from './requestDomain-lookup-table-modal.component';
import { UserLookupTableModalComponent } from './user-lookup-table-modal.component';
import { ClientListLookupTableModalComponent } from './clientList-lookup-table-modal.component';
import { AssignRequestsComponent } from '@app/main/assignRequests/assignRequests/assignRequests.component';
//Added for file upload
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppConsts } from '@shared/AppConsts';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'createOrEditRequestModal',
    templateUrl: './create-or-edit-request-modal.component.html',
    animations: [appModuleAnimation()]
})

export class CreateOrEditRequestModalComponent extends AppComponentBase implements AfterViewChecked {

    @ViewChild('createOrEditModal') modal: ModalDirective;
    @ViewChild('requestAreaLookupTableModal') requestAreaLookupTableModal: RequestAreaLookupTableModalComponent;
    @ViewChild('requestDomainLookupTableModal') requestDomainLookupTableModal: RequestDomainLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal2') userLookupTableModal2: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal3') userLookupTableModal3: UserLookupTableModalComponent;
    @ViewChild('clientListLookupTableModal') clientListLookupTableModal: ClientListLookupTableModalComponent;
    @ViewChild('userLookupTableModal4') userLookupTableModal4: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal5') userLookupTableModal5: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    techTeam: CreateOrEditTechTeamDto = new CreateOrEditTechTeamDto();
    request: CreateOrEditRequestDto = new CreateOrEditRequestDto();
    attachedDoc: CreateOrEditAttachedDocDto = new CreateOrEditAttachedDocDto();
    requestAreaRequestAreaName = '';
    requestDomainDomainName = '';
    userName = '';
    userName2 = '';
    userName3 = '';
    clientListClientName = '';
    userName4 = '';
    attachedDocsIds = [];
    //Added for file upload
    uploadUrl: string;
    uploadedFiles: any[] = [];

    techTeamMembers = [];
    _callType: number;

    constructor(
        injector: Injector,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _techTeamsServiceProxy: TechTeamsServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _demoUiComponentsService: DemoUiComponentsServiceProxy,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles';
        //this.uploadUrl = 'http://localhost:22742/TestAppFileUpload/UploadFile';
    }

    //Functions for file upload
    onUpload(event): void {
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }

        let resultarray = JSON.parse(event.xhr.responseText)['result'];

        for (let i = 0; i < resultarray.length; i++) {

            let item = {
                DocumentId: resultarray[i].id,
                FileName: resultarray[i].fileName,
                RequestId: this.request.id
            };

            this.attachedDocsIds.push(item);
        }

    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }

    ngAfterViewChecked(): void {
        //Temporary fix for: https://github.com/valor-software/ngx-bootstrap/issues/1508
        // $('tabset ul.nav').addClass('m-tabs-line');
        // $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }

    show(requestId?: number, callType?: number): void {
        this._callType = callType;
        if (!requestId) {
            this.request = new CreateOrEditRequestDto();
            this.request.id = requestId;
            this.request.requestTypeId = 0;
            this.requestAreaRequestAreaName = '';
            this.requestDomainDomainName = '';
            this.userName = '';
            this.userName2 = '';
            this.userName3 = '';
            this.clientListClientName = '';
            this.techTeamMembers = [];
            this.attachedDocsIds = [];
            //this.userName4 = '';

            this.active = true;
            this.modal.show();
        } else {
            this.techTeamMembers = [];
            this._requestsServiceProxy.getRequestForEdit(requestId).subscribe(result => {
                this.request = result.request;
                this.requestAreaRequestAreaName = result.requestAreaRequestAreaName;
                this.requestDomainDomainName = result.requestDomainDomainName;
                this.userName = result.userName;
                this.userName2 = result.userName2;
                this.userName3 = result.userName3;
                this.clientListClientName = result.clientListClientName;

                if (result.techTeam_.length > 0) {

                    for (let i = 0; i < result.techTeam_.length; i++) {

                        let item = {
                            CMACSUserid: result.techTeam_[i].cmacsUserId,
                            displayName: result.techTeam_[i].name,
                            requestid: result.request.id,
                            timeCharge: 0
                        };

                        this.techTeamMembers.push(item);
                    }

                }

                this.active = true;
                this.modal.show();
            });

        }
    }

    save(): void {

        this.saving = true;

        let attachedDocInfo = [];



        for (let i = 0; i < this.attachedDocsIds.length; i++) {
            attachedDocInfo.push(this.attachedDocsIds[i].id);

            //this.attachedDoc =
        }

        //This has to be reviewed
        //this.request.fileInfo = attachedDocInfo;

        this._requestsServiceProxy.createOrEdit(this.request)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });

        if (this.attachedDocsIds.length > 0) {
            this._attachedDocsServiceProxy.createOrEditMultipleDoc(this.attachedDocsIds)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    //this.notify.info(this.l('SavedSuccessfully'));
                    //this.close();
                    //this.modalSave.emit(null);
                    console.log('doc info saved');
                });
        }

    }

    sendTOR(): void {
        let tor: CreateRequestTORDto = new CreateRequestTORDto();
    tor.hasSignedTOR = this.request.hasSignedTOR;
    tor.requestId = this.request.id;
    tor.tor = this.request.termsOfRef;

        this._requestsServiceProxy.sendTOR(tor)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    approveTOR(): void {
        this._requestsServiceProxy.approveTOR(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    approveRequest(): void {
        this._requestsServiceProxy.approveRequest(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SuccessfullyApproved'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    openSelectRequestAreaModal() {
        this.requestAreaLookupTableModal.id = this.request.requestAreaId;
        this.requestAreaLookupTableModal.displayName = this.requestAreaRequestAreaName;
        this.requestAreaLookupTableModal.show();
    }
    openSelectRequestDomainModal() {
        this.requestDomainLookupTableModal.id = this.request.requestDomainId;
        this.requestDomainLookupTableModal.displayName = this.requestDomainDomainName;
        this.requestDomainLookupTableModal.show();
    }
    openSelectUserModal() {
        this.userLookupTableModal.id = this.request.requestorId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }
    openSelectUserModal2() {
        this.userLookupTableModal2.id = this.request.requestorPartnerId;
        this.userLookupTableModal2.displayName = this.userName;
        this.userLookupTableModal2.show();
    }
    openSelectUserModal3() {
        this.userLookupTableModal3.id = this.request.requestorManagerId;
        this.userLookupTableModal3.displayName = this.userName;
        this.userLookupTableModal3.show();
    }
    openSelectClientListModal() {
        this.clientListLookupTableModal.id = this.request.clientListId;
        this.clientListLookupTableModal.displayName = this.clientListClientName;
        this.clientListLookupTableModal.show();
    }
    openSelectUserModal4() {
        this.userLookupTableModal4.id = this.request.assigneeId;
        this.userLookupTableModal4.displayName = this.userName;
        this.userLookupTableModal4.show();
    }


    setRequestAreaIdNull() {
        this.request.requestAreaId = null;
        this.requestAreaRequestAreaName = '';
    }
    setRequestDomainIdNull() {
        this.request.requestDomainId = null;
        this.requestDomainDomainName = '';
    }
    setRequestorIdNull() {
        this.request.requestorId = null;
        this.userName = '';
    }
    setRequestorPartnerIdNull() {
        this.request.requestorPartnerId = null;
        this.userName2 = '';
    }
    setRequestorManagerIdNull() {
        this.request.requestorManagerId = null;
        this.userName3 = '';
    }
    setClientListIdNull() {
        this.request.clientListId = null;
        this.clientListClientName = '';
    }
    setAssigneeIdNull() {
        this.request.assigneeId = null;
        this.userName4 = '';
    }

    getNewRequestAreaId() {
        this.request.requestAreaId = this.requestAreaLookupTableModal.id;
        this.requestAreaRequestAreaName = this.requestAreaLookupTableModal.displayName;
    }
    getNewRequestDomainId() {
        this.request.requestDomainId = this.requestDomainLookupTableModal.id;
        this.requestDomainDomainName = this.requestDomainLookupTableModal.displayName;
    }
    getNewRequestorId() {
        this.request.requestorId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }
    getNewRequestorPartnerId() {
        this.request.requestorPartnerId = this.userLookupTableModal2.id;
        this.userName2 = this.userLookupTableModal2.displayName;
    }
    getNewRequestorManagerId() {
        this.request.requestorManagerId = this.userLookupTableModal3.id;
        this.userName3 = this.userLookupTableModal3.displayName;
    }
    getNewClientListId() {
        this.request.clientListId = this.clientListLookupTableModal.id;
        this.clientListClientName = this.clientListLookupTableModal.displayName;
    }
    getNewAssigneeId() {
        this.request.assigneeId = this.userLookupTableModal4.id;
        this.userName4 = this.userLookupTableModal4.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    openSelectUserModal5() {
        this.userLookupTableModal5.show();
    }

    removeapproverUserId(functionId: number) {
        for (let i = 0; i < this.techTeamMembers.length; i++) {
            if (this.techTeamMembers[i].id === functionId) {
                this.techTeamMembers.splice(i, 1);
                return;
            }
        }
    }

    getNewTechTeamMember() {

        if (this.userLookupTableModal5.id > 0) {

            //check that the user has not been selected already
            for (let i = 0; i < this.techTeamMembers.length; i++) {
                if (this.techTeamMembers[i].id === this.userLookupTableModal5.id) {
                    return;
                }
            }

            let item = {
                CMACSUserid: this.userLookupTableModal5.id,
                displayName: this.userLookupTableModal5.displayName,
                requestid: this.request.id,
                timeCharge: 0
            };

            this.techTeamMembers.push(item);
            //console.log(this.techTeamMembers);
        }
    }

    saveTechMember(): void {
        let assignedtechmembers = [];

        if (this.techTeamMembers.length < 2) {
            this.notify.error('You need at least 2 team members');
            this.saving = false;
            return;
        }

        this._techTeamsServiceProxy.createOrEditTechTeam(this.techTeamMembers)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
}
