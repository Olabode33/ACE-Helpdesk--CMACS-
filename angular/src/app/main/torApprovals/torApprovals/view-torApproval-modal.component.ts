import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetTORApprovalForView, TORApprovalDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewTORApprovalModal',
    templateUrl: './view-torApproval-modal.component.html'
})
export class ViewTORApprovalModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTORApprovalForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTORApprovalForView();
        this.item.torApproval = new TORApprovalDto();
    }

    show(item: GetTORApprovalForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
