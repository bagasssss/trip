import { Constants } from "../models/constants";

export class Mapper {
    static map(source, destination) {
        if (source) {
            for (var prop in source) {
                if (source.hasOwnProperty(prop))
                    destination[prop] = source[prop];
            }
        }
    }
}

export class UserHelper {
    static getUserId() {
        return localStorage.getItem(Constants.userIdKey);
    }
}
