import {PermissionCheckerService} from 'abp-ng2-module';
import {AppSessionService} from '@shared/common/session/app-session.service';

import {Injectable} from '@angular/core';
import {AppMenu} from './app-menu';
import {AppMenuItem} from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Home', 'Pages.Requests', 'fa fa-home', '/app/main/home'),
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            // new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem('Configuration', '', 'flaticon-settings', '', [], [
                new AppMenuItem('RequestDomains', 'Pages.Requests.Configuration', 'flaticon-squares-1', '/app/main/requestDomains/requestDomains'),
                new AppMenuItem('RequestAreas', 'Pages.Requests.Configuration', 'flaticon-interface-3', '/app/main/requestAreas/requestAreas'),
                new AppMenuItem('RequestSubAreas', 'Pages.Requests.Configuration', 'flaticon-more', '/app/main/requestAreas/requestSubAreas'),
                new AppMenuItem('ReportingTerritories', 'Pages.Requests.Configuration', 'flaticon-location', '/app/main/reportingTerritories/reportingTerritories'),
                new AppMenuItem('Industries', 'Pages.Requests.Configuration', 'flaticon-layers', '/app/main/industries/industries'),
                new AppMenuItem('ClientLists', 'Pages.Requests.Configuration', 'flaticon-network', '/app/main/clientLists/clientLists'),
                new AppMenuItem('StockExchanges', 'Pages.Requests.Configuration', 'flaticon-coins', '/app/main/stockExchanges/stockExchanges'),
                //new AppMenuItem('TechTeams', 'Pages.TechTeams', 'flaticon-users', '/app/main/techTeams/techTeams'),
                //new AppMenuItem('RequestThreads', 'Pages.RequestThreads', 'flaticon-tabs', '/app/main/requestThreads/requestThreads'),
                //new AppMenuItem('RequestDocs', 'Pages.RequestDocs', 'flaticon-notes', '/app/main/requestDocs/requestDocs'),
                //new AppMenuItem('TORApprovals', 'Pages.TORApprovals', 'flaticon-more', '/app/main/torApprovals/torApprovals'),
                //new AppMenuItem('RequestApprovals', 'Pages.RequestApprovals', 'flaticon-more', '/app/main/requestApprovals/requestApprovals'),
                //new AppMenuItem('AttachedDocs', 'Pages.AttachedDocs', 'flaticon-interface', '/app/main/attachedDocs/attachedDocs')
            ]),
            new AppMenuItem('Administration', '', 'flaticon-interface-8', '', [], [
                // new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                // new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages', ['/app/admin/languages/{name}/texts']),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                // new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                // new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                // new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions'),
                // new AppMenuItem('DynamicProperties', 'Pages.Administration.DynamicProperties', 'flaticon-interface-8', '/app/admin/dynamic-property'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings')
            ]),
            // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components')
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach(menuItem => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach(subMenu => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
