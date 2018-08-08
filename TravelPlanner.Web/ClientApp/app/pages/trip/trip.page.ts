import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { TripViewModel } from "../../models/trip/trip";
import { BackendService } from "../../services/backend.service";
import { AuthService } from "../../services/auth.service";
import { UserHelper } from "../../utils/helpers";
import { User } from "../../models/user";
import { Car } from "../../models/car";
import { Observable } from "rxjs/Rx";

import { Subject } from "rxjs/Subject";
import { TripRouteViewModel } from "../../models/trip/trip-route";

@Component({
    templateUrl: './trip.page.html'
})
export class TripPage implements OnInit {
    private currentUser: User = null;

    trip = new TripViewModel();

    constructor(private backendService: BackendService,
        private route: ActivatedRoute,
        private authService: AuthService) {
    }

    ngOnInit() {
        this.currentUser = this.authService.user;
        var tripId = this.route.snapshot.params['id'];
        this.backendService.getTrip(tripId, this.currentUser.id).then((trip: TripViewModel) => {
            this.trip = trip;
        });
    }
}
