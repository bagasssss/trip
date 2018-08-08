import { Subject } from "rxjs/Subject";
import { Injectable } from '@angular/core';
import { TripRouteViewModel } from "../../models/trip/trip-route";
import { TripWaypointViewModel } from "../../models/trip/trip-waypoint";
import { SightObjectViewModel } from "../../models/sight-object";
import { BehaviorSubject } from "rxjs/BehaviorSubject";

@Injectable()
export class MapObsService {
    private routeSource = new Subject<TripRouteViewModel>();
    private waypointsSource = new BehaviorSubject<TripWaypointViewModel[]>([]);
    private sightObjectsSource = new BehaviorSubject<SightObjectViewModel[]>([]);

    route$ = this.routeSource.asObservable();
    waypoints$ = this.waypointsSource.asObservable();
    sightObjects$ = this.sightObjectsSource.asObservable();

    routeBuilt(model: TripRouteViewModel) {
        this.routeSource.next(model);
    }

    setWaypoints(model: TripWaypointViewModel[]) {
        this.waypointsSource.next(model);
    }

    setSightObjects(model: SightObjectViewModel[]) {
        this.sightObjectsSource.next(model);
    }
}
