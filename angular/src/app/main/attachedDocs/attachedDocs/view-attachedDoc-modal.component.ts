import { StaffEntityType } from './../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetAttachedDocForView, AttachedDocDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewAttachedDocModal',
    templateUrl: './view-attachedDoc-modal.component.html'
})
export class ViewAttachedDocModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAttachedDocForView;
    staffEntityType = StaffEntityType;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAttachedDocForView();
        this.item.attachedDoc = new AttachedDocDto();
    }

    show(item: GetAttachedDocForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
