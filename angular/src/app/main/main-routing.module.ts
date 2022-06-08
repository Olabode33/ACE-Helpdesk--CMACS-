import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AssignRequestsComponent } from './assignRequests/assignRequests/assignRequests.component';
import { AttachedDocsComponent } from './attachedDocs/attachedDocs/attachedDocs.component';
import { ClientListsComponent } from './clientLists/clientLists/clientLists.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { IndustriesComponent } from './industries/industries/industries.component';
import { ReportingTerritoriesComponent } from './reportingTerritories/reportingTerritories/reportingTerritories.component';
import { RequestApprovalsComponent } from './requestApprovals/requestApprovals/requestApprovals.component';
import { RequestAreasComponent } from './requestAreas/requestAreas/requestAreas.component';
import { RequestSubAreasComponent } from './requestAreas/requestSubAreas/requestSubAreas.component';
import { RequestDocsComponent } from './requestDocs/requestDocs/requestDocs.component';
import { RequestDomainsComponent } from './requestDomains/requestDomains/requestDomains.component';
import { RequestsComponent } from './requests/requests/requests.component';
import { CreateEditRequestComponent } from './requests/_subs/create-edit-request/create-edit-request.component';
import { TreatRequestComponent } from './requests/_subs/treat-request/treat-request.component';
import { ViewRequestComponent } from './requests/_subs/view-request/view-request.component';
import { RequestThreadsComponent } from './requestThreads/requestThreads/requestThreads.component';
import { StockExchangesComponent } from './stockExchanges/stockExchanges/stockExchanges.component';
import { TechTeamsComponent } from './techTeams/techTeams/techTeams.component';
import { TORApprovalsComponent } from './torApprovals/torApprovals/torApprovals.component';
import { TreatRequestsComponent } from './treatRequests/treatRequests/treatRequests.component';
import { WorkspaceComponent } from './workspace/workspace/workspace.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'requestAreas/requestSubAreas', component: RequestSubAreasComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'attachedDocs/attachedDocs', component: AttachedDocsComponent, data: { permission: 'Pages.AttachedDocs' }  },
                    { path: 'requestApprovals/requestApprovals', component: RequestApprovalsComponent, data: { permission: 'Pages.RequestApprovals' }  },
                    { path: 'torApprovals/torApprovals', component: TORApprovalsComponent, data: { permission: 'Pages.TORApprovals' }  },
                    { path: 'requestDocs/requestDocs', component: RequestDocsComponent, data: { permission: 'Pages.RequestDocs' }  },
                    { path: 'requestThreads/requestThreads', component: RequestThreadsComponent, data: { permission: 'Pages.RequestThreads' }  },
                    { path: 'techTeams/techTeams', component: TechTeamsComponent, data: { permission: 'Pages.TechTeams' }  },
                    { path: 'requests/requests', component: RequestsComponent, data: { permission: 'Pages.Requests' }  },
                    { path: 'requests/create', component: CreateEditRequestComponent, data: { permission: 'Pages.Requests' }  },
                    { path: 'requests/edit/:requestId/:callType', component: CreateEditRequestComponent, data: { permission: 'Pages.Requests' }  },
                    { path: 'requests/treat/:requestId/:callType', component: TreatRequestComponent, data: { permission: 'Pages.Requests' }  },
                    { path: 'requests/view/:requestId', component: ViewRequestComponent, data: { permission: 'Pages.Requests' }  },
                    { path: 'assignRequests/assignRequests', component: AssignRequestsComponent, data: { permission: 'Pages.AssignRequests' }  },
                    { path: 'treatRequests/treatRequests', component: TreatRequestsComponent, data: { permission: 'Pages.TreatRequests' }  },
                    { path: 'stockExchanges/stockExchanges', component: StockExchangesComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'clientLists/clientLists', component: ClientListsComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'industries/industries', component: IndustriesComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'reportingTerritories/reportingTerritories', component: ReportingTerritoriesComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'requestAreas/requestAreas', component: RequestAreasComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'requestDomains/requestDomains', component: RequestDomainsComponent, data: { permission: 'Pages.Requests.Configuration' }  },
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: 'home', component: WorkspaceComponent, data: { permission: 'Pages.Requests' } },
                    { path: '', redirectTo: 'home', pathMatch: 'full' },
                    { path: '**', redirectTo: 'home' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
