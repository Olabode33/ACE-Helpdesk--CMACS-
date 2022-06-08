import { RequestStatusEnum } from './RequestStatus.enum';

export class StatusClassHelper {
    getStatusClass(statusId: number): string {
        switch (statusId) {
            case RequestStatusEnum.Assigned:
            case RequestStatusEnum.CMASManagerRequestReview:
                return 'focus';
            case RequestStatusEnum.Prepared:
                return 'info';
            case RequestStatusEnum.Completed:
                return 'success';
            case RequestStatusEnum.Rejected:
                return 'danger';
            case RequestStatusEnum.AwaitingTOR:
                return 'brand';
            case RequestStatusEnum.WIP:
                return 'warning';
            case RequestStatusEnum.Accepted:
                return 'accent';
            case RequestStatusEnum.CMASManagerApproved:
                return 'primary';
            default:
                return 'metal';
        }
    }
}
