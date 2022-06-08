import { RequestStatus, RequestType } from './../../../../../shared/service-proxies/service-proxies';
import { StatusClassHelper } from './../../../_custom_helpers/statusClasshelper';
import { RequestStatusEnum } from '@app/main/_custom_helpers/RequestStatus.enum';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Component, OnInit, AfterViewInit, ViewChild, Injector, AfterViewChecked } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { RequestAreaLookupTableModalComponent } from '../../requests/requestArea-lookup-table-modal.component';
import { RequestDomainLookupTableModalComponent } from '../../requests/requestDomain-lookup-table-modal.component';
import { ClientListLookupTableModalComponent } from '../../requests/clientList-lookup-table-modal.component';
import { CreateOrEditTechTeamDto, CreateOrEditRequestDto, CreateOrEditAttachedDocDto, RequestsServiceProxy, TechTeamsServiceProxy, TenantSettingsServiceProxy, DemoUiComponentsServiceProxy, AttachedDocsServiceProxy, ApprovalAuditInfo, AttachedDocDto, CommonLookupServiceProxy, CreateRequestTORDto } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/operators';
import { Location } from '@angular/common';
import { UserLookupTableModalComponent } from '../../requests/user-lookup-table-modal.component';
import { RequestTypeEnum } from '@app/main/_custom_helpers/RequestType.enum';
import { CommentModalComponent } from '../comment-modal/comment-modal.component';

@Component({
    selector: 'app-view-request',
    templateUrl: './view-request.component.html',
    styleUrls: ['./view-request.component.css'],
    animations: [appModuleAnimation()]
})
export class ViewRequestComponent extends AppComponentBase implements OnInit, AfterViewChecked {

    @ViewChild('requestAreaLookupTableModal') requestAreaLookupTableModal: RequestAreaLookupTableModalComponent;
    @ViewChild('requestDomainLookupTableModal') requestDomainLookupTableModal: RequestDomainLookupTableModalComponent;
    @ViewChild('userLookupTableModal') userLookupTableModal: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal2') userLookupTableModal2: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal3') userLookupTableModal3: UserLookupTableModalComponent;
    @ViewChild('clientListLookupTableModal') clientListLookupTableModal: ClientListLookupTableModalComponent;
    @ViewChild('userLookupTableModal4') userLookupTableModal4: UserLookupTableModalComponent;
    @ViewChild('userLookupTableModal5') userLookupTableModal5: UserLookupTableModalComponent;
    @ViewChild('returnCommentModal') returnCommentModal: CommentModalComponent;

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
    attachedDocsIds = [];
    //Added for file upload
    uploadUrl: string;
    uploadedFiles: any[] = [];

    techTeamMembers = [];
    _callType: number;

    createByAuditInfo = '';
    dateCreatedAuditInfo = '';
    updatedByAuditInfo = '';
    dateUpdatedAuditInfo = '';
    cmacsManagerApprover = '';
    cmacsManagerApprovalDate = '';
    torApprovalsAuditInfo: ApprovalAuditInfo[] = new Array();
    requestApprovalsAuditInfo: ApprovalAuditInfo[] = new Array();
    requestAttachments: AttachedDocDto[];
    signedTORAttachments: AttachedDocDto[];
    reviewedFsAttachments: AttachedDocDto[];
    requestNextAction = '';

    subAreas = 'Sample, Sample2';

    requestTypeEnum = RequestType;
    requestStatusEnum = RequestStatus;

    statusClassHelper: StatusClassHelper = new StatusClassHelper();

    constructor(
        injector: Injector,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _commonServiceProxy: CommonLookupServiceProxy,
        private _techTeamsServiceProxy: TechTeamsServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _demoUiComponentsService: DemoUiComponentsServiceProxy,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _location: Location,
        private _router: Router
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles';
        //this.uploadUrl = 'http://localhost:22742/TestAppFileUpload/UploadFile';
        this.requestAttachments = new Array();
        this.signedTORAttachments = new Array();
    }

    ngOnInit() {
        this._activatedRoute.paramMap.subscribe(params => {
            let requestId = +params.get('requestId');
            console.log(requestId);

            this.show(requestId);
        });
    }

    ngAfterViewChecked(): void {
        //Temporary fix for: https://github.com/valor-software/ngx-bootstrap/issues/1508
        // $('tabset ul.nav').addClass('m-tabs-line');
        // $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }

    show(requestId: number): void {
        console.log(requestId);
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
                this.createByAuditInfo = result.createdBy;
                this.updatedByAuditInfo = result.lastUpdatedBy;
                this.dateCreatedAuditInfo = result.dateCreated;
                this.dateUpdatedAuditInfo = result.lastUpdatedDate;
                this.torApprovalsAuditInfo = result.torApprovalsAuditInfo;
                this.requestApprovalsAuditInfo = result.requestApprovalsAuditInfo;
                this.cmacsManagerApprover = result.cmacsManagerApprovalInfo != null ? result.cmacsManagerApprovalInfo.approverName : '';
                this.cmacsManagerApprovalDate = result.cmacsManagerApprovalInfo != null ? result.cmacsManagerApprovalInfo.approvalDate.format('LL') : '';
                this.requestAttachments = result.requestAttachments ? result.requestAttachments : new Array();
                this.signedTORAttachments = result.signedTORAttachments ? result.signedTORAttachments : new Array();
                this.reviewedFsAttachments = result.reviewedFSAttachments ? result.reviewedFSAttachments : new Array();
                this.requestNextAction = result.nextAction;

                console.log(result.requestAttachments);
                console.log(result.signedTORAttachments);

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

                this.getRequestSubAreas(requestId);
                this.active = true;
            });

        }
    }


    goBack(): void {
        this.active = false;
        this._location.back();
    }

    getRequestSubAreas(requestId: number): void {
        this._commonServiceProxy.getRequestsSubAreas(requestId).subscribe(result => {
            this.subAreas = result.map(x => x.name).join(', ');
        });
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

    isLoggedInUserOnTechTeam(): boolean {
        if (this.techTeamMembers.length === 0) {
            return false;
        }

        let isOnTechTeam = this.techTeamMembers.find(x => x.CMACSUserid === abp.session.userId);
        if (isOnTechTeam) {
            return true;
        }

        return false;
    }

    deleteRequest(): void {
        if (this.request.requestStatusId !== 0) {
            this.notify.error('Request in progress, cannot be deleted');
            return;
        }

        this.message.confirm(
            '#' + this.request.requestNo, 'Are you sure you want to delete request?',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.delete(this.request.id)
                        .subscribe(() => {
                            this.message.success(this.l('SuccessfullyDeleted'));
                            this.goBack();
                        });
                }
            }
        );
    }

    editRequest(): void {
        if (this.request.requestStatusId !== 0) {
            this.message.info('Request in progress, cannot be edited');
            return;
        }

        this.navigateToCreateOrEdit(RequestStatusEnum.Requested);
    }

    assignRequest(): void {

        if (this.request.requestStatusId == RequestStatusEnum.Completed.valueOf()) {
            this.notify.error('This request should be in requested state');
            return;
        }

        this.navigateToTreatRequestPage(RequestStatusEnum.Assigned);
    }

    createTOR(): void {

        if (this.request.requestTypeId == RequestStatusEnum.Assigned.valueOf()) {
            this.notify.error('You don\'t need a TOR for an enquiry');
            return;
        }

        if (this.request.requestStatusId != RequestStatusEnum.Assigned.valueOf()) {
            this.notify.error('This request has not been assigned');
            return;
        }

        this.navigateToTreatRequestPage(RequestStatusEnum.Completed);
    }

    respondToEnquiry(): void {

        if (this.request.requestTypeId != RequestStatusEnum.Assigned.valueOf()) {
            this.notify.error('You don\'t need a TOR for an enquiry');
            return;
        }

        if (this.request.requestStatusId != RequestStatusEnum.Assigned.valueOf()) {
            this.notify.error('This request has not been assigned');
            return;
        }

        this.navigateToTreatRequestPage(RequestStatusEnum.Prepared);
    }

    approveTOR(): void {

        if (this.request.requestStatusId != RequestStatusEnum.AwaitingTOR.valueOf()) {
            this.notify.error('This request is not awaiting TOR');
            return;
        }

        if (this.request.requestTypeId == RequestStatusEnum.Assigned.valueOf()) {
            this.notify.error('You don\'t need a TOR for an enquiry');
            return;
        }

        this._requestsServiceProxy.approveTOR(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.success(this.l('TOR successfully approved!'));
                this.goBack();
                //this.modalSave.emit(null);
            });

        //this.navigateToTreatRequestPage(4);
    }

    treatRequest(): void {

        // if (this.request.requestTypeId == RequestStatusEnum.Requested.valueOf() && !this.request.termsOfRefApproved) {
        //     this.notify.error('TOR has not been approved, request cannot be treated');
        //     return;
        // }

        // if (this.request.requestTypeId == RequestStatusEnum.Requested.valueOf() && this.request.requestStatusId != RequestStatusEnum.WIP.valueOf()) {
        //     this.notify.error('This request is not in WIP');
        //     return;
        // }

        this.navigateToTreatRequestPage(RequestStatusEnum.Prepared);
    }

    sendTOR(): void {
        let tor: CreateRequestTORDto = new CreateRequestTORDto();
        tor.hasSignedTOR = this.request.hasSignedTOR;
        tor.requestId = this.request.id;
        tor.tor = this.request.termsOfRef;

        console.log(tor);

        this._requestsServiceProxy.sendTOR(tor)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    sendRequestForApproval(): void {

        if (this.request.requestStatusId === RequestStatusEnum.WIP.valueOf() || this.request.requestStatusId === RequestStatusEnum.CMASManagerRequestReview.valueOf()) {
            this.message.confirm(
                'Please confirm that this request can be sent to the requesting team for approval', 'Send for approval?',
                (isConfirmed) => {
                    if (isConfirmed) {
                        this._requestsServiceProxy.sendRequestForApproval(this.request.id)
                            .subscribe(() => {
                                this.message.success(this.l('Response successfully sent for approval!'));
                                this.goBack();
                            });
                    }
                }
            );
        } else {
            this.notify.error('This request is not in WIP');
            return;
        }
    }

    sendForCMACSManagerApproval(): void {
        this.message.confirm(
            'Please confirm request can be sent for approval', 'Send for approval?',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.submitToCMASManagerForApproval(this.request.id)
                        .subscribe(() => {
                            this.message.success(this.l('Response successfully sent for approval!'));
                            this.goBack();
                        });
                }
            }
        );
    }

    approveRequest(): void {

        if (this.request.requestStatusId != RequestStatusEnum.Prepared.valueOf()) {
            this.notify.error('This request has not been sent for approval');
            return;
        }

        this._requestsServiceProxy.approveRequest(this.request.id)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('Request successfully approved!'));
                this.goBack();
                //this.modalSave.emit(null);
            });
    }

    cmacsManagerApprove(): void {

        if (this.request.requestTypeId == RequestStatusEnum.Requested.valueOf()) {
            // if (!this.request.termsOfRefApproved) {
            //     this.notify.error('The TOR has not been approved');
            //     return;
            // }

            if (this.request.requestStatusId != RequestStatusEnum.Accepted.valueOf()) {
                this.notify.error('This request has not been approved');
                return;
            }
        }

        if (this.request.requestTypeId == RequestStatusEnum.Assigned.valueOf()) {

            if (this.request.requestStatusId == RequestStatusEnum.Requested.valueOf()) {
                this.notify.error('This enquiry was never assigned');
                return;
            }
        }

        this.message.confirm(
            'Are you sure?', 'Approve Request',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.cmacsManagerApproval(this.request.id)
                        .subscribe(() => {
                            this.message.success(this.l('Successfully Approved'));
                            this.goBack();
                        });
                }
            }
        );
    }

    markAsCompleted(): void {

        if (this.request.requestTypeId == RequestStatusEnum.Requested.valueOf()) {
            // if (!this.request.termsOfRefApproved) {
            //     this.notify.error('The TOR has not been approved');
            //     return;
            // }

            if (this.request.requestStatusId < 7 && this.request.requestStatusId >= 8) {
                this.notify.error('This request has not been approved by the requesting team\'s partner');
                return;
            }
        }

        if (this.request.requestTypeId == RequestStatusEnum.Assigned.valueOf()) {

            if (this.request.requestStatusId == RequestStatusEnum.Requested.valueOf()) {
                this.notify.error('This enquiry was never assigned');
                return;
            }
        }

        this.message.confirm(
            'Please confirm the request can be marked as completed', 'Mark as completed?',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._requestsServiceProxy.markTreated(this.request.id)
                        .subscribe(() => {
                            this.message.success(this.l('Successfully Completed'));
                            this.goBack();
                        });
                }
            }
        );
    }

    navigateToCreateOrEdit(callType: RequestStatusEnum): void {
        this._router.navigate(['app/main/requests/edit', this.request.id, callType.valueOf()]);
        //this.createOrEditRequestModal.show(request.id, 2);
    }

    navigateToTreatRequestPage(callType: RequestStatusEnum): void {
        this._router.navigate(['app/main/requests/treat', this.request.id, callType.valueOf()]);
        //this.createOrEditRequestModal.show(request.id, 2);
    }

    getStatusClass(statusId: number): string {
        return this.statusClassHelper.getStatusClass(statusId);
    }

    downloadResourceFile(attachment: AttachedDocDto): string {
        return AppConsts.remoteServiceBaseUrl +
            '/File/DownloadBinaryFile?id=' +
            attachment.documentId +
            '&contentType=application/pdf&fileName=' +
            attachment.fileName;
    }

    printRequest(): void {
        window.print();
    }

    showReturnCommentModal(): void {
        this.returnCommentModal.show();
    }

    sendBack($event): void {
        this._requestsServiceProxy.updateRequestStatus(this.request.id, RequestStatus.Returned, $event)
                                  .subscribe(() => {
                                    this.notify.info(this.l('Request successfully returned!'));
                                    this.goBack();
                                  });
    }
}
