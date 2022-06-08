import { CreateRequestTORDto, CreateOrEditRequestSubAreaMappingDto, CreateOrEditClientListDto } from './../../../../../shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, AfterViewInit, ViewChild, Injector, AfterViewChecked } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { RequestAreaLookupTableModalComponent } from '../../requests/requestArea-lookup-table-modal.component';
import { RequestDomainLookupTableModalComponent } from '../../requests/requestDomain-lookup-table-modal.component';
import { ClientListLookupTableModalComponent } from '../../requests/clientList-lookup-table-modal.component';
import { CreateOrEditTechTeamDto, CreateOrEditRequestDto, CreateOrEditAttachedDocDto, RequestsServiceProxy, TechTeamsServiceProxy, TenantSettingsServiceProxy, DemoUiComponentsServiceProxy, AttachedDocsServiceProxy, NameValueDto, CommonLookupServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/operators';
import { Location } from '@angular/common';
import { UserLookupTableModalComponent } from '../../requests/user-lookup-table-modal.component';
import * as moment from 'moment';
import { CreateOrEditClientListModalComponent } from '@app/main/clientLists/clientLists/create-or-edit-clientList-modal.component';

@Component({
    selector: 'app-create-edit-request',
    templateUrl: './create-edit-request.component.html',
    styleUrls: ['./create-edit-request.component.css'],
    animations: [appModuleAnimation()]
})
export class CreateEditRequestComponent extends AppComponentBase implements OnInit, AfterViewChecked {

    @ViewChild('requestAreaLookupTableModal') requestAreaLookupTableModal: RequestAreaLookupTableModalComponent;
    @ViewChild('requestDomainLookupTableModal') requestDomainLookupTableModal: RequestDomainLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal2') userLookupTableModal2: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal3') userLookupTableModal3: UserLookupTableModalComponent;
    @ViewChild('clientListLookupTableModal') clientListLookupTableModal: ClientListLookupTableModalComponent;
    @ViewChild('userLookupTableModal4') userLookupTableModal4: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal5') userLookupTableModal5: UserLookupTableModalComponent;
    @ViewChild('createOrEditClientListModal') createOrEditClientListModal: CreateOrEditClientListModalComponent;

    active = false;
    saving = false;

    formIsEditable = true;

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
    attachedDocsIds: CreateOrEditAttachedDocDto[] = new Array();
    subAreas: CreateOrEditRequestSubAreaMappingDto[] = new Array();
    //Added for file upload
    uploadUrl: string;
    uploadedFiles: any[] = [];
    uploadedSignedTORFiles: any[] = [];

    techTeamMembers = [];
    _callType: number;

    requestDomains: NameValueDto[] = new Array();
    requestAreas: NameValueDto[] = new Array();
    requestSubAreas: NameValueDto[] = new Array();
    filteredSubAreas: NameValueDto[] = new Array();
    selectedSubAreas: NameValueDto[] = new Array();

    constructor(
        injector: Injector,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _techTeamsServiceProxy: TechTeamsServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _demoUiComponentsService: DemoUiComponentsServiceProxy,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _location: Location,
        private _commonLookupServiceProxy: CommonLookupServiceProxy
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles';
        //this.uploadUrl = 'http://localhost:22742/TestAppFileUpload/UploadFile';

        _commonLookupServiceProxy.getAllRequestAreas().subscribe(result => {
            this.requestAreas = result;
        });
        _commonLookupServiceProxy.getAllRequestDomains().subscribe(result => {
            this.requestDomains = result;
        });
        _commonLookupServiceProxy.getAllRequestSubArea().subscribe(result => {
            this.requestSubAreas = result;
            this.filteredSubAreas = this.requestSubAreas;
        });
    }

    ngOnInit() {
        this._activatedRoute.paramMap.subscribe(params => {
            let requestId = +params.get('requestId');
            let callType = +params.get('callType');
            console.log(requestId);

            if (callType !== 0) {
                this.formIsEditable = false;
            } else {
                this.formIsEditable = true;
            }

            this.show(requestId, callType);
        });
    }

    //Functions for file upload
    onUpload(event): void {
        for (const file of event.files) {
            this.uploadedFiles.push(file);
        }

        let resultarray = JSON.parse(event.xhr.responseText)['result'];
        for (let i = 0; i < resultarray.length; i++) {
            let item = new CreateOrEditAttachedDocDto();

            item.documentId = resultarray[i].id;
            item.fileName = resultarray[i].fileName;
            item.requestId = this.request.id;
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
        console.log(requestId);
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

            this.request.requiredResponseDate = moment().add(this.getWorkDay(3), 'days');
            //this.userName4 = '';

            this.active = true;
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
                this.selectedSubAreas = result.subAreas;

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
            });

        }
    }

    searchSubAreas(event): void {
        let q: string = event.query;
        this.filteredSubAreas = this.requestSubAreas.filter(x => x.name.toLowerCase().search(q.toLowerCase()) == 0);

        console.log(this.filteredSubAreas);
        console.log(this.subAreas);
    }

    save(): void {
        this.saving = true;
        let attachedDocInfo = [];

        for (let i = 0; i < this.attachedDocsIds.length; i++) {
            attachedDocInfo.push(this.attachedDocsIds[i].id);
        }

        //console.log(this.request.requiredResponseDate.date());
        if (this.request.requiredResponseDate) {
            if (this.request.requiredResponseDate < moment().add(this.getWorkDay(3) - 1, 'days')) {
                this.message.error('<p class="lead text-center">The earliest response date is <b>3 working days</b> from today <br><b>('
                                        + moment().add(this.getWorkDay(3), 'days').format('dddd, MMMM Do YYYY') + ')</b></p>',
                                   'Required Response Date Error!', true);
                this.saving = false;
                return;
            }
        }
        //This has to be reviewed
        //this.request.fileInfo = attachedDocInfo;
        if (this.request.id === 0) {
            this.request.id = null;
        }
        this.request.attachments = this.attachedDocsIds;
        this.request.subAreas = this.selectedSubAreas;

        this._requestsServiceProxy.createOrEdit(this.request)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                //this.message.success(this.l('Created Successfully'), 'New Request');

                if (this.request.id && this.request.id > 0) {
                    this.saveAttachments();
                    this.message.success(this.l('Saved Successfully'), 'Edit Request');
                } else {
                    this.message.success(this.l('Created Successfully'), 'New Request');
                }

                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    saveAttachments(): void {
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

    onUploadSignedTOR(event): void {
        for (const file of event.files) {
            this.uploadedSignedTORFiles.push(file);
        }

        let resultarray = JSON.parse(event.xhr.responseText)['result'];
        for (let i = 0; i < resultarray.length; i++) {
            let attachment = new CreateOrEditAttachedDocDto();
            attachment.attachmentType = 1;
            attachment.documentId = resultarray[i].id;
            attachment.fileName = resultarray[i].fileName;
            attachment.requestId = this.request.id;
            //console.log(resultarray[i]);

            this.attachedDocsIds.push(attachment);
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
                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    approveTOR(): void {
        this._requestsServiceProxy.approveTOR(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    approveRequest(): void {
        this._requestsServiceProxy.approveRequest(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SuccessfullyApproved'));
                this.goBack();
                //this.modalSave.emit(null);
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
    openNewClientListModal() {
        this.createOrEditClientListModal.show();
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
    getNewClientList($event: CreateOrEditClientListDto) {
        this.request.clientListId = $event.id;
        this.clientListClientName = $event.clientName;
    }
    getNewAssigneeId() {
        this.request.assigneeId = this.userLookupTableModal4.id;
        this.userName4 = this.userLookupTableModal4.displayName;
    }

    goBack(): void {
        this.active = false;
        this._location.back();
    }

    openSelectUserModal5() {
        this.userLookupTableModal5.show();
    }

    removeapproverUserId(functionId: number) {
        for (let i = 0; i < this.techTeamMembers.length; i++) {
            if (this.techTeamMembers[i].id == functionId) {
                this.techTeamMembers.splice(i, 1);
                return;
            }
        }
    }

    getNewTechTeamMember() {

        if (this.userLookupTableModal5.id > 0) {

            //check that the user has not been selected already
            for (let i = 0; i < this.techTeamMembers.length; i++) {
                if (this.techTeamMembers[i].id == this.userLookupTableModal5.id) {
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
                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    getWorkDay(daysToAdd: number): number {
        let today = moment().day();

        if (today === 6) {
            return daysToAdd + 1;
        } else if (today >= 3 ) {
            return daysToAdd + 2;
        } else {
            return daysToAdd;
        }
    }

}