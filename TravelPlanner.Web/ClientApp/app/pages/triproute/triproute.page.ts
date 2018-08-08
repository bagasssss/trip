import { Component, OnInit } from '@angular/core';
import { TripViewModel } from "../../models/trip/trip";
import { BackendService } from "../../services/backend.service";
import { Constants } from "../../models/constants";
import { UserHelper } from "../../utils/helpers";
import { TripRouteViewModel } from "../../models/trip/trip-route";
import { SightObjectViewModel } from "../../models/sight-object";
import { MapObsService } from "../../services/observables/map.service";
import { Subject } from "rxjs/Subject";
import { GlobalService } from "../../services/global.service";
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: './triproute.page.html'
})
export class TripRoutePage implements OnInit {
    trip = new TripViewModel();
    tripId: string;

    constructor(private backendService: BackendService,
        private mapObsService: MapObsService,
        private globalService: GlobalService,
        private activatedRoute: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.tripId = this.activatedRoute.snapshot.params["id"];
        this.backendService.getTrip(this.tripId, UserHelper.getUserId()).then(trip => {
            this.trip = trip;
        });

        this.backendService.getSights().then((sights: SightObjectViewModel[]) => {
            this.mapObsService.setSightObjects(sights);
        });

        this.mapObsService.route$
            .subscribe((tripRoute: TripRouteViewModel) => {
                this.trip.tripRoute = tripRoute;
            });

        this.mapObsService.setWaypoints([]);
    }

    saveRoute() {
        this.backendService.updateTrip(this.trip).then(() => {
            this.globalService.showSuccessMsg("tripUpdated");
            this.globalService.navigateToRoute(`/trip/${this.tripId}`);
        });
    }
}
