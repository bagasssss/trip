import { Injectable } from "@angular/core";
import { NotificationObsService } from "./observables/notification.service";
import { Router, ActivatedRoute } from "@angular/router";
import { UserHelper } from "../utils/helpers";

@Injectable()
export class GlobalService {
    constructor(public notificationService: NotificationObsService,
        public router: Router) {
    }

    showSuccessMsg(message) {
        this.notificationService.success.next(message);
    }

    navigateToRoute(route) {
        this.router.navigate([route]);
    }

    getUserId(): string {
        return UserHelper.getUserId();
    }
}