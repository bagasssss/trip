import { Component, ViewContainerRef, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NotificationObsService } from "../../services/observables/notification.service";
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    constructor(private translate: TranslateService,
        private notificationService: NotificationObsService,
        private toastr: ToastsManager,
        private viewRef: ViewContainerRef) {

        this.translate.use('en');
        this.toastr.setRootViewContainerRef(viewRef);
    }

    ngOnInit() {
        this.notificationService.validationErrors.subscribe((error: string) => {
            setTimeout(() => {
                var translation = this.translate.instant(error);
                this.toastr.error(translation);
            });
        });

        this.notificationService.serverErrors.subscribe((error: string) => {
            setTimeout(() => {
                this.toastr.error("Server Error Happens.... Message: " + error);
            });
        });

        this.notificationService.success.subscribe((text: string) => {
            setTimeout(() => {
                var translation = this.translate.instant(text);
                this.toastr.success(translation);
            });
        });
    }
}
