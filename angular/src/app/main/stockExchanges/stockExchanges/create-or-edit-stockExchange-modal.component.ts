import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { StockExchangesServiceProxy, CreateOrEditStockExchangeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditStockExchangeModal',
    templateUrl: './create-or-edit-stockExchange-modal.component.html'
})
export class CreateOrEditStockExchangeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    stockExchange: CreateOrEditStockExchangeDto = new CreateOrEditStockExchangeDto();


    constructor(
        injector: Injector,
        private _stockExchangesServiceProxy: StockExchangesServiceProxy
    ) {
        super(injector);
    }

    show(stockExchangeId?: number): void {
        if (!stockExchangeId) {
            this.stockExchange = new CreateOrEditStockExchangeDto();
            this.stockExchange.id = stockExchangeId;

            this.active = true;
            this.modal.show();
        } else {
            this._stockExchangesServiceProxy.getStockExchangeForEdit(stockExchangeId).subscribe(result => {
                this.stockExchange = result.stockExchange;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._stockExchangesServiceProxy.createOrEdit(this.stockExchange)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
