import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetRequestThreadForView, RequestThreadDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestThreadModal',
    templateUrl: './view-requestThread-modal.component.html'
})
export class ViewRequestThreadModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestThreadForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestThreadForView();
        this.item.requestThread = new RequestThreadDto();
    }

    show(item: GetRequestThreadForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
