import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetRequestAreaForView, RequestAreaDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestAreaModal',
    templateUrl: './view-requestArea-modal.component.html'
})
export class ViewRequestAreaModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestAreaForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestAreaForView();
        this.item.requestArea = new RequestAreaDto();
    }

    show(item: GetRequestAreaForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
