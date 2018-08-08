import { Component, OnInit } from '@angular/core';
import { TripViewModel } from "../../models/trip/trip";
import { BackendService } from "../../services/backend.service";
import { UserHelper } from "../../utils/helpers";

@Component({
    selector: 'mytrips',
    templateUrl: './mytrips.page.html'
})
export class MyTripsPage implements OnInit {
    trips: TripViewModel[];

    constructor(private backendService: BackendService) {
    }

    ngOnInit(): void {
        this.backendService.getOwnTrips(UserHelper.getUserId()).then((trips) => {
            this.trips = trips;
        });
    }

    removeTrip(tripId: number) {
        if (!confirm("Are you sure?")) return;
        this.backendService.removeTrip(tripId, UserHelper.getUserId()).then(() => {
            alert("Done");
            
        });
    }
}
