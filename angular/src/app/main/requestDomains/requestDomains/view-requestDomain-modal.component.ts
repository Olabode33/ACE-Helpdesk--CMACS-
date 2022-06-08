import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetRequestDomainForView, RequestDomainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestDomainModal',
    templateUrl: './view-requestDomain-modal.component.html'
})
export class ViewRequestDomainModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestDomainForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestDomainForView();
        this.item.requestDomain = new RequestDomainDto();
    }

    show(item: GetRequestDomainForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
