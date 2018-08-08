import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { MapObsService } from "../../../services/observables/map.service";
import { BackendService } from "../../../services/backend.service";
import { TripRouteViewModel } from "../../../models/trip/trip-route";
import { TripViewModel } from "../../../models/trip/trip";
import { UserHelper } from "../../../utils/helpers";

@Component({
    templateUrl: './tripmap.page.html'
})
export class TripMapPage implements OnInit, OnDestroy {
    isMapReadOnly = true;

    constructor(private backendService: BackendService,
        private route: ActivatedRoute,
        private mapObsService: MapObsService) {
    }

    ngOnInit() {
        var tripId = this.route.parent.snapshot.params['id'];
        this.backendService.getTrip(tripId, UserHelper.getUserId()).then((trip: TripViewModel) => {
            if (trip.tripRoute) {
                this.mapObsService.setWaypoints(trip.tripRoute.tripWaypoints);
            }
        });
    }

    ngOnDestroy() {

    }
}
