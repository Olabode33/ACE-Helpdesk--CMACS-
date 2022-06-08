import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-comment-modal',
  templateUrl: './comment-modal.component.html',
  styleUrls: ['./comment-modal.component.css']
})
export class CommentModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal') modal: ModalDirective;

    comment: string;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    show(): void {
        this.active = true;
        this.modal.show();
    }

    save(): void {
        if (!this.comment || this.comment === '') {
            this.notify.error('Comment is required!');
            return;
        }

        this.active = false;
        this.modal.hide();
        this.modalSave.emit(this.comment);
    }

    close(): void {
        this.active = false;
        this.modal.hide();
        this.modalSave.emit(null);
    }

}
