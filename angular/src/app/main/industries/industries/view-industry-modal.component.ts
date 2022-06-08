import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetIndustryForView, IndustryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewIndustryModal',
    templateUrl: './view-industry-modal.component.html'
})
export class ViewIndustryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetIndustryForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetIndustryForView();
        this.item.industry = new IndustryDto();
    }

    show(item: GetIndustryForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
