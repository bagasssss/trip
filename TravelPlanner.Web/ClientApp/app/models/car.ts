import { Mapper } from "../utils/helpers";

export class Car {
    constructor(car?: Car) {
        Mapper.map(car, this);
    }

    id: string;
    userId: string;
    name: string;
    petrolUsage: string;
}