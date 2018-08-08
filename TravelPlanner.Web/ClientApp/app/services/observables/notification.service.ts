import { Subject } from "rxjs/Subject";
import { Injectable } from '@angular/core';

@Injectable()
export class NotificationObsService {
    public validationErrors = new Subject<string>();
    public serverErrors = new Subject<string>();
    public success = new Subject<string>();
}
