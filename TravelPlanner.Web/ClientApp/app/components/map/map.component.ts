import { Component, NgZone, OnInit, Input, AfterViewInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { MapsAPILoader, GoogleMapsAPIWrapper, AgmMap } from '@agm/core';
import { } from 'googlemaps';
import { GoogleMap } from "@agm/core/services/google-maps-types";
import { TripWaypointViewModel } from "../../models/trip/trip-waypoint";
import { TripRouteViewModel } from "../../models/trip/trip-route";
import { SightObjectViewModel } from "../../models/sight-object";
import { MapObsService } from "../../services/observables/map.service";
import { Subject } from "rxjs/Subject";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/takeUntil';

@Component({
    selector: 'map',
    templateUrl: './map.component.html',
    styles: [` agm-map {
            height: 500px;
        }`],
    providers: [GoogleMapsAPIWrapper]
})
export class MapComponent implements OnInit, OnDestroy, AfterViewInit  {
    private readonly defaultZoom = 7;
    private readonly defaultLng = 50.4501;
    private readonly defaultLat = 30.5234;

    private zoom: number = this.defaultZoom;
    private lat: number = this.defaultLat;
    private lng: number = this.defaultLng;
    private infoWindowOpened = null;
    private updateWaypointIndex = -1;

    private waypoints: TripWaypointViewModel[] = [];
    private markers: SightObjectViewModel[] = [];

    @ViewChild("search")
    private searchElementRef: ElementRef;

    @ViewChild(AgmMap)
    private agmMap: AgmMap;

    private placeAutocomplete: google.maps.places.Autocomplete;
    private directionsDisplay: any;
    private directionsService: google.maps.DirectionsService;

    private unsubscribe = new Subject<any>();

    @Input() isReadOnlyMode = false;

    estimatedTime = "";
    estimatedDistance = "";

    constructor(private mapsLoader: MapsAPILoader,
                private gmapsApi: GoogleMapsAPIWrapper,
                private ngZone: NgZone,
                private mapObsService: MapObsService) {
    }

    ngOnInit() {
        this.mapsLoader.load().then(() => {
            this.setCurrentPosition();
            this.placeAutocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement);
            this.placeAutocomplete.addListener("place_changed", () => this.placeSelected());

            this.directionsDisplay = new google.maps.DirectionsRenderer();
            this.directionsService = new google.maps.DirectionsService();
        });
    }

    ngAfterViewInit() {
        this.agmMap.mapReady.subscribe(map => {
            this.directionsDisplay.setMap(map);
            this.subscribeToObservables();
        });
    }

    ngOnDestroy() {
        this.unsubscribe.next();
        this.unsubscribe.complete();
    }

    getDirections() {
        if (this.waypoints.length < 2) return;

        this.closeInfoWindow();

        var origin = this.waypoints[0].latLng; // first location
        var destination = this.waypoints[this.waypoints.length - 1].latLng; // last location

        var googleWaypoints: google.maps.DirectionsWaypoint[] = [];
        // set waypoints if there are more then 2 locations
        if (this.waypoints.length > 2) {
            this.waypoints.forEach((r) => googleWaypoints.push({ location: r.latLng, stopover: false }));
        }

        this.routeDirections({
            destination: destination,
            origin: origin,
            waypoints: googleWaypoints
        });
    }

    clearDirections() {
        if (this.isReadOnlyMode) return;

        this.waypoints = [];
        this.estimatedTime = "";
        this.estimatedDistance = "";
        this.directionsDisplay.setDirections({ routes: [] });
        this.setCurrentPosition();
    }

    addWaypoint(marker: SightObjectViewModel, infoWindow) {
        if (this.isReadOnlyMode) return;

        this.closeInfoWindow();
        this.infoWindowOpened = infoWindow;

        setTimeout(() => this.closeInfoWindow(), 5000);
        if (!this.isWaypointUnique(marker.label)) return;

        this.waypoints.push({
            id: marker.id,
            tripRouteId: 0,
            latLng: marker.latLng,
            name: marker.label
        });
    }

    updateWaypoint(waypoint: TripWaypointViewModel, index: number) {
        this.searchElementRef.nativeElement.value = waypoint.name;
        this.updateWaypointIndex = index;
    }

    removeWaypoint(waypoint: TripWaypointViewModel, index: number) {
        this.waypoints.splice(index, 1);
    }

    private subscribeToObservables() {
        this.mapObsService.sightObjects$
            .takeUntil(this.unsubscribe)
            .subscribe((sights) => {
                this.markers = sights;
            });

        this.mapObsService.waypoints$
            .takeUntil(this.unsubscribe)
            .subscribe((waypoints) => {
                this.waypoints = waypoints;
                if (this.waypoints.length >= 2) {
                    this.getDirections();
                }
            });
    }

    private routeDirections(requestDirection: { origin, destination, waypoints }) {
        this.directionsDisplay.setDirections({ routes: [] });

        this.directionsService.route({
                origin: requestDirection.origin,
                destination: requestDirection.destination,
                waypoints: requestDirection.waypoints,
                travelMode: google.maps.TravelMode.DRIVING
            },
            (resp, status) => this.onDirectionsReceived(resp, status, this));
    }

    private onDirectionsReceived(response: any, status: any, that: MapComponent) {
        if (status === 'OK') {
            that.directionsDisplay.setDirections(response);

            var point = response.routes[0].legs[0];
            this.estimatedTime = point.duration.text;
            this.estimatedDistance = point.distance.text;

            that.mapObsService.routeBuilt({
                id: 0,
                distance: this.estimatedDistance,
                time: this.estimatedTime,
                tripWaypoints: that.waypoints
            });
        } else {
            console.log('Directions request failed due to ' + status);
        }
    }



    private placeSelected() {
        this.ngZone.run(() => {
            let place: google.maps.places.PlaceResult = this.placeAutocomplete.getPlace();
            var placeName = place.address_components[0].short_name;

            if (place.geometry === undefined || place.geometry === null || !this.isWaypointUnique(placeName)) {
                return;
            }

            // if not update mode
            if (this.updateWaypointIndex !== -1) {
                this.waypoints[this.updateWaypointIndex].id = place.place_id;
                this.waypoints[this.updateWaypointIndex].latLng = {
                    lat: place.geometry.location.lat(),
                    lng: place.geometry.location.lng()
                };
                this.waypoints[this.updateWaypointIndex].name = placeName;

                this.updateWaypointIndex = -1;
            } else {
                this.waypoints.push({
                    id: "",
                    tripRouteId: 0,
                    latLng: { lat: place.geometry.location.lat(), lng: place.geometry.location.lng() },
                    name: placeName
                });
            }
            this.searchElementRef.nativeElement.value = "";
        });
    }

    private isWaypointUnique(name) {
        var isUnique = this.waypoints.every((waypoint) => {
            return waypoint.name !== name;
        });

        return isUnique;
    }

    private closeInfoWindow() {
        if (this.infoWindowOpened !== null)
            this.infoWindowOpened.close();
    }

    private setCurrentPosition() {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition((position) => {
                this.zoom = this.defaultZoom;
                this.lat = position.coords.latitude;
                this.lng = position.coords.longitude;
            });
        }
    }
}







