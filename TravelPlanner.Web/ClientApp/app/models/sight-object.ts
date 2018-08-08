import { Mapper } from "../utils/helpers";

export class SightObjectViewModel {
    constructor(sight?: SightObjectViewModel) {
        if (sight) {
            Mapper.map(sight, this);
        }
    }

    id: string;
    latLng: google.maps.LatLngLiteral;
    label: string;
}