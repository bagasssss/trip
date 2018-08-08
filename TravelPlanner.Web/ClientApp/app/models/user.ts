import { Mapper } from "../utils/helpers";
import { Car } from "./car";

export class User {
    constructor(user?: User) {
        Mapper.map(user, this);
    }

    id: string;
    userName: string;
    email: string;
    phone: string;
    cars?: Car[];
}