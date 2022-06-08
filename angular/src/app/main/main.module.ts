import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { EditorModule } from 'primeng/editor';

import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { AssignRequestsComponent } from './assignRequests/assignRequests/assignRequests.component';
import { AttachedDocsComponent } from './attachedDocs/attachedDocs/attachedDocs.component';
import { BinaryObjectLookupTableModalComponent } from './attachedDocs/attachedDocs/binaryObject-lookup-table-modal.component';
import { CreateOrEditAttachedDocModalComponent } from './attachedDocs/attachedDocs/create-or-edit-attachedDoc-modal.component';
import { ViewAttachedDocModalComponent } from './attachedDocs/attachedDocs/view-attachedDoc-modal.component';
import { ClientListsComponent } from './clientLists/clientLists/clientLists.component';
import { CreateOrEditClientListModalComponent } from './clientLists/clientLists/create-or-edit-clientList-modal.component';
import { IndustryLookupTableModalComponent } from './clientLists/clientLists/industry-lookup-table-modal.component';
import { ReportingTerritoryLookupTableModalComponent } from './clientLists/clientLists/reportingTerritory-lookup-table-modal.component';
import { StockExchangeLookupTableModalComponent } from './clientLists/clientLists/stockExchange-lookup-table-modal.component';
import { ViewClientListModalComponent } from './clientLists/clientLists/view-clientList-modal.component';
import { CreateOrEditIndustryModalComponent } from './industries/industries/create-or-edit-industry-modal.component';
import { IndustriesComponent } from './industries/industries/industries.component';
import { ViewIndustryModalComponent } from './industries/industries/view-industry-modal.component';
import { CreateOrEditReportingTerritoryModalComponent } from './reportingTerritories/reportingTerritories/create-or-edit-reportingTerritory-modal.component';
import { ReportingTerritoriesComponent } from './reportingTerritories/reportingTerritories/reportingTerritories.component';
import { ViewReportingTerritoryModalComponent } from './reportingTerritories/reportingTerritories/view-reportingTerritory-modal.component';
import { CreateOrEditRequestApprovalModalComponent } from './requestApprovals/requestApprovals/create-or-edit-requestApproval-modal.component';
import { RequestApprovalsComponent } from './requestApprovals/requestApprovals/requestApprovals.component';
import { ViewRequestApprovalModalComponent } from './requestApprovals/requestApprovals/view-requestApproval-modal.component';
import { CreateOrEditRequestAreaModalComponent } from './requestAreas/requestAreas/create-or-edit-requestArea-modal.component';
import { RequestAreasComponent } from './requestAreas/requestAreas/requestAreas.component';
import { ViewRequestAreaModalComponent } from './requestAreas/requestAreas/view-requestArea-modal.component';
import { CreateOrEditRequestSubAreaModalComponent } from './requestAreas/requestSubAreas/create-or-edit-requestSubArea-modal.component';
import { RequestSubAreaRequestAreaLookupTableModalComponent } from './requestAreas/requestSubAreas/requestSubArea-requestArea-lookup-table-modal.component';
import { RequestSubAreasComponent } from './requestAreas/requestSubAreas/requestSubAreas.component';
import { CreateOrEditRequestDocModalComponent } from './requestDocs/requestDocs/create-or-edit-requestDoc-modal.component';
import { RequestDocsComponent } from './requestDocs/requestDocs/requestDocs.component';
import { ViewRequestDocModalComponent } from './requestDocs/requestDocs/view-requestDoc-modal.component';
import { CreateOrEditRequestDomainModalComponent } from './requestDomains/requestDomains/create-or-edit-requestDomain-modal.component';
import { RequestDomainsComponent } from './requestDomains/requestDomains/requestDomains.component';
import { ViewRequestDomainModalComponent } from './requestDomains/requestDomains/view-requestDomain-modal.component';
import { ClientListLookupTableModalComponent } from './requests/requests/clientList-lookup-table-modal.component';
import { CreateOrEditRequestModalComponent } from './requests/requests/create-or-edit-request-modal.component';
import { RequestAreaLookupTableModalComponent } from './requests/requests/requestArea-lookup-table-modal.component';
import { RequestDomainLookupTableModalComponent } from './requests/requests/requestDomain-lookup-table-modal.component';
import { RequestsComponent } from './requests/requests/requests.component';
import { UserLookupTableModalComponent } from './requests/requests/user-lookup-table-modal.component';
import { ViewRequestModalComponent } from './requests/requests/view-request-modal.component';
import { CommentModalComponent } from './requests/_subs/comment-modal/comment-modal.component';
import { CreateEditRequestComponent } from './requests/_subs/create-edit-request/create-edit-request.component';
import { TreatRequestComponent } from './requests/_subs/treat-request/treat-request.component';
import { ViewRequestComponent } from './requests/_subs/view-request/view-request.component';
import { CreateOrEditRequestThreadModalComponent } from './requestThreads/requestThreads/create-or-edit-requestThread-modal.component';
import { RequestThreadsComponent } from './requestThreads/requestThreads/requestThreads.component';
import { ViewRequestThreadModalComponent } from './requestThreads/requestThreads/view-requestThread-modal.component';
import { CreateOrEditStockExchangeModalComponent } from './stockExchanges/stockExchanges/create-or-edit-stockExchange-modal.component';
import { StockExchangesComponent } from './stockExchanges/stockExchanges/stockExchanges.component';
import { ViewStockExchangeModalComponent } from './stockExchanges/stockExchanges/view-stockExchange-modal.component';
import { CreateOrEditTechTeamModalComponent } from './techTeams/techTeams/create-or-edit-techTeam-modal.component';
import { RequestLookupTableModalComponent } from './techTeams/techTeams/request-lookup-table-modal.component';
import { TechTeamsComponent } from './techTeams/techTeams/techTeams.component';
import { ViewTechTeamModalComponent } from './techTeams/techTeams/view-techTeam-modal.component';
import { CreateOrEditTORApprovalModalComponent } from './torApprovals/torApprovals/create-or-edit-torApproval-modal.component';
import { TORApprovalsComponent } from './torApprovals/torApprovals/torApprovals.component';
import { ViewTORApprovalModalComponent } from './torApprovals/torApprovals/view-torApproval-modal.component';
import { TreatRequestsComponent } from './treatRequests/treatRequests/treatRequests.component';
import { WorkspaceComponent } from './workspace/workspace/workspace.component';
import { AutoCompleteModule, DragDropModule, FileUploadModule, PaginatorModule, TableModule, TreeModule } from 'primeng';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ModalModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        FileUploadModule,
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        TooltipModule.forRoot(),
        TableModule,
        TreeModule,
        DragDropModule,
        PaginatorModule,
        // PrimeNgFileUploadModule,
        AutoCompleteModule,
        EditorModule,
    ],
    declarations: [
        CommentModalComponent,
        RequestSubAreasComponent,
        CreateOrEditRequestSubAreaModalComponent,
        RequestSubAreaRequestAreaLookupTableModalComponent,
        WorkspaceComponent,
        ViewRequestComponent,
        TreatRequestComponent,
        CreateEditRequestComponent,
        AttachedDocsComponent,
        ViewAttachedDocModalComponent, CreateOrEditAttachedDocModalComponent,
        BinaryObjectLookupTableModalComponent,
        RequestApprovalsComponent,
        ViewRequestApprovalModalComponent, CreateOrEditRequestApprovalModalComponent,
        TORApprovalsComponent,
        ViewTORApprovalModalComponent, CreateOrEditTORApprovalModalComponent,
        RequestDocsComponent,
        ViewRequestDocModalComponent, CreateOrEditRequestDocModalComponent,
        RequestThreadsComponent,
        ViewRequestThreadModalComponent, CreateOrEditRequestThreadModalComponent,
        TechTeamsComponent,
        ViewTechTeamModalComponent, CreateOrEditTechTeamModalComponent,
        RequestLookupTableModalComponent,
        RequestsComponent,
        ViewRequestModalComponent, CreateOrEditRequestModalComponent,
        RequestAreaLookupTableModalComponent,
        RequestDomainLookupTableModalComponent,
        UserLookupTableModalComponent,
        ClientListLookupTableModalComponent,
        ClientListsComponent,
        ViewClientListModalComponent, CreateOrEditClientListModalComponent,
        StockExchangeLookupTableModalComponent,
        StockExchangesComponent,
        ViewStockExchangeModalComponent, CreateOrEditStockExchangeModalComponent,
        ClientListsComponent,
        ViewClientListModalComponent, CreateOrEditClientListModalComponent,
        IndustryLookupTableModalComponent,
        ReportingTerritoryLookupTableModalComponent,
        IndustriesComponent,
        ViewIndustryModalComponent, CreateOrEditIndustryModalComponent,
        ReportingTerritoriesComponent,
        ViewReportingTerritoryModalComponent, CreateOrEditReportingTerritoryModalComponent,
        RequestAreasComponent,
        ViewRequestAreaModalComponent, CreateOrEditRequestAreaModalComponent,
        RequestDomainsComponent,
        ViewRequestDomainModalComponent, CreateOrEditRequestDomainModalComponent,
        DashboardComponent, AssignRequestsComponent, TreatRequestsComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
