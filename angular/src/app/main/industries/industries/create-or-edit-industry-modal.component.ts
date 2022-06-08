import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { IndustriesServiceProxy, CreateOrEditIndustryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';


@Component({
    selector: 'createOrEditIndustryModal',
    templateUrl: './create-or-edit-industry-modal.component.html'
})
export class CreateOrEditIndustryModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    industry: CreateOrEditIndustryDto = new CreateOrEditIndustryDto();


    constructor(
        injector: Injector,
        private _industriesServiceProxy: IndustriesServiceProxy
    ) {
        super(injector);
    }

    show(industryId?: number): void {
        if (!industryId) {
            this.industry = new CreateOrEditIndustryDto();
            this.industry.id = industryId;

            this.active = true;
            this.modal.show();
        } else {
            this._industriesServiceProxy.getIndustryForEdit(industryId).subscribe(result => {
                this.industry = result.industry;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this._industriesServiceProxy.createOrEdit(this.industry)
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
