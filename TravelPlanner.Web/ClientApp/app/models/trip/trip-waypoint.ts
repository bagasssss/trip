import { Mapper } from "../../utils/helpers";

export class TripWaypointViewModel {
    constructor(wapypoint?: TripWaypointViewModel) {
        Mapper.map(wapypoint, this);
    }

    public id: string;
    public tripRouteId: number;
    public latLng: google.maps.LatLngLiteral;
    public name: string;
}