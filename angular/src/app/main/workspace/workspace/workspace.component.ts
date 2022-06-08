import { RequestStatus, RequestType } from './../../../../shared/service-proxies/service-proxies';
import { StatusClassHelper } from './../../_custom_helpers/statusClasshelper';
import { AppComponentBase } from 'shared/common/app-component-base';
import { Component, OnInit, ViewEncapsulation, Injector, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { RequestsServiceProxy, TechTeamsServiceProxy, TenantSettingsServiceProxy, DemoUiComponentsServiceProxy, AttachedDocsServiceProxy, GetRequestForView } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { RequestStatusEnum } from '@app/main/_custom_helpers/RequestStatus.enum';
import { Paginator, LazyLoadEvent } from 'primeng';
import { Table } from 'primeng/table';

@Component({
    selector: 'app-workspace',
    templateUrl: './workspace.component.html',
    styleUrls: ['./workspace.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class WorkspaceComponent extends AppComponentBase implements OnInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    requests: GetRequestForView[] = new Array();
    completedRequests: GetRequestForView[] = new Array();
    numberOfRequests = 0;
    wipRequests = 0;
    outstandingRequests = 0;
    animations = true;
    filter = '';
    loadingRequests = false;
    numberOfRequestsCounter = 0;
    wipRequestsCounter = 0;
    outstandingRequestsCounter = 0;
    wipRequestChange = 0;
    outstandingRequestsChange = 0;
    advancedFiltersAreShown = false;
    requestStatusIdFilter = -1;

    requestStatus = RequestStatus;
    requestType = RequestType;
    requestStatusEnum = RequestStatusEnum;

    statusClassHelper: StatusClassHelper = new StatusClassHelper();

    colorScheme = {
        domain: ['#81D4FA']
    };

    constructor(
        injector: Injector,
        private _requestsServiceProxy: RequestsServiceProxy,
        private _techTeamsServiceProxy: TechTeamsServiceProxy,
        private _tenantSettingsService: TenantSettingsServiceProxy,
        private _demoUiComponentsService: DemoUiComponentsServiceProxy,
        private _attachedDocsServiceProxy: AttachedDocsServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _router: Router
    ) {
        super(injector);
    }

    ngOnInit() {
        //this.getRequests();
        //this.getCompletedRequest();
    }

    getRequests(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._requestsServiceProxy.getAllForWorkspaceNew(
            this.filter,
            this.requestStatusIdFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    // getRequests(): void {
    //     this.loadingRequests = true;
    //     this._requestsServiceProxy.getAllForWorkspace(this.filter, -1, '', 0, 1000)
    //     .pipe(finalize(() => { this.loadingRequests = false; }))
    //     .subscribe(result => {
    //         this.numberOfRequests = result.totalCount;
    //         this.requests = result.items;
    //     });
    // }

    // getCompletedRequest(): void {
    //     //this.loadingRequests = true;
    //     this._requestsServiceProxy.getCompletedForWorkspace(this.filter, this.requestStatusEnum.Completed, '', 0, 1000)
    //     //.pipe(finalize(() => { this.loadingRequests = false; }))
    //     .subscribe(result => {
    //         this.completedRequests = result.items;
    //     });
    // }

    getStatusClass(statusId: number): string {
        return this.statusClassHelper.getStatusClass(statusId);
    }

    navigateToViewRequestPage(requestId: number): void {
        this._router.navigate(['app/main/requests/view', requestId]);
    }

    navigateToCreateRequestPage(): void {
        this._router.navigate(['app/main/requests/create']);
    }

    navigateToDashboardPage(): void {
        this._router.navigate(['app/main/dashboard']);
    }

    navigateToCompletedRequestPage(): void {
        this._router.navigate(['app/main/requests/requests']);
    }

}
