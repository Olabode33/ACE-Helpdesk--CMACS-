import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetRequestApprovalForView, RequestApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestApprovalModal',
    templateUrl: './view-requestApproval-modal.component.html'
})
export class ViewRequestApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestApprovalForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestApprovalForView();
        this.item.requestApproval = new RequestApprovalDto();
    }

    show(item: GetRequestApprovalForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
