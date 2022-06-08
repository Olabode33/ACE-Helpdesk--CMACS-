import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetTechTeamForView, TechTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewTechTeamModal',
    templateUrl: './view-techTeam-modal.component.html'
})
export class ViewTechTeamModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTechTeamForView;
    // staffCategory = TechTeamDtoRole;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTechTeamForView();
        this.item.techTeam = new TechTeamDto();
    }

    show(item: GetTechTeamForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
