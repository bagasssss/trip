import { Response } from '@angular/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { NotificationObsService } from "./observables/notification.service";

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
    constructor(private injector: Injector) { }

    handleError(error) {
        var response = <Response>error.rejection;
        if (response) {
            var notificationService = this.injector.get(NotificationObsService);

            // bad request
            if (response.status === 400) {
                notificationService.validationErrors.next(response.text());
            } else {
                console.log(error);
                throw error;
            }
        } else {
            throw error;
        }
    }
}