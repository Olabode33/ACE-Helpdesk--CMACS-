import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetRequestForView, RequestDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestModal',
    templateUrl: './view-request-modal.component.html'
})
export class ViewRequestModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestForView;
    // requestStatus = RequestDtoRequestStatusId;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestForView();
        this.item.request = new RequestDto();
    }

    show(item: GetRequestForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
