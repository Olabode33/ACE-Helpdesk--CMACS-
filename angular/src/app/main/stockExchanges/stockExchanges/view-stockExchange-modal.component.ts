import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { GetStockExchangeForView, StockExchangeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'viewStockExchangeModal',
    templateUrl: './view-stockExchange-modal.component.html'
})
export class ViewStockExchangeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStockExchangeForView;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetStockExchangeForView();
        this.item.stockExchange = new StockExchangeDto();
    }

    show(item: GetStockExchangeForView): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
