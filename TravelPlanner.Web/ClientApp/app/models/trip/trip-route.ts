import { Mapper } from "../../utils/helpers";
import { TripWaypointViewModel } from "./trip-waypoint";

export class TripRouteViewModel {
    constructor(route?: TripRouteViewModel) {
        Mapper.map(route, this);
    }

    public id: number;
    public distance: string;
    public time: string;
    public tripWaypoints: TripWaypointViewModel[];
}