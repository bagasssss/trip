import { Mapper } from '../../utils/helpers';
import { User } from "../user";
import { TripRouteViewModel } from "./trip-route";

export class TripViewModel {
    constructor(trip?: TripViewModel) {
        if (trip) {
            Mapper.map(trip, this);
        }
    }

    id: number;
    creatorId: string;
    title: string;
    description: string;
    tripRoute: TripRouteViewModel;
    users: User[];
}