import { Component } from '@angular/core';
import { TripViewModel } from "../../models/trip/trip";
import { BackendService } from "../../services/backend.service";
import { UserHelper } from "../../utils/helpers";

@Component({
    selector: 'invitedtripspage',
    templateUrl: './invitedtrips.page.html'
})
export class InvitedTripsPage {
    trips: TripViewModel[];

    constructor(private backendService: BackendService) {
    }

    ngOnInit(): void {
        this.backendService.getInvitedTrips(UserHelper.getUserId()).then((trips) => {
            this.trips = trips;
        });
    }
}
