import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetRequestDocForView, RequestDocDto } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewRequestDocModal',
    templateUrl: './view-requestDoc-modal.component.html'
})
export class ViewRequestDocModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetRequestDocForView;
    // staffEntityType = RequestDocDtoPreparerTypeId;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetRequestDocForView();
        this.item.requestDoc = new RequestDocDto();
    }

    show(item: GetRequestDocForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
