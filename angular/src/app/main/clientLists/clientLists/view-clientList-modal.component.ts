import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetClientListForView, ClientListDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewClientListModal',
    templateUrl: './view-clientList-modal.component.html'
})
export class ViewClientListModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetClientListForView;
    // finYearEnd = ClientListDtoFinancialYearEnd;
    // channelType = ClientListDtoChannelTypeName;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetClientListForView();
        this.item.clientList = new ClientListDto();
    }

    show(item: GetClientListForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
