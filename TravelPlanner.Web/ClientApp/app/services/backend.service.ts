import { Injectable } from '@angular/core';
import { Http, ResponseContentType, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';

import { LoginViewModel } from "../models/auth/login";
import { RegistrationViewModel } from "../models/auth/registration";
import { User } from "../models/user";
import { JWTTokens } from "../models/auth/jwttokens";
import { TripViewModel } from "../models/trip/trip";
import { InvitesViewModel } from "../models/invites";
import { MessageViewModel } from "../models/message";
import { SightObjectViewModel } from "../models/sight-object";

@Injectable()
export class BackendService {
    constructor(private http: Http) {
    }

    // Auth
    login(model: LoginViewModel): Promise<JWTTokens> {
        return this.http.post("/api/auth/login", model).toPromise()
            .then((result) => { return new JWTTokens(result.json()); });
    }

    register(model: RegistrationViewModel): Promise<JWTTokens> {
        return this.http.post("/api/auth/register", model).toPromise()
            .then((result) => { return new JWTTokens(result.json()); })
            .catch(result => { return Promise.reject(result); });
    }

    // Trip
    createTrip(model: TripViewModel): Promise<number> {
        return this.http.post("/api/trip/create", model, { responseType: ResponseContentType.Text }).toPromise()
            .then((result: Response) => { return parseInt(result.text()); });
    }

    updateTrip(model: TripViewModel): Promise<boolean> {
        return this.http.post("/api/trip/update", model).toPromise()
            .then((result: Response) => { return true; });
    }

    removeTrip(tripId: number, userId: string): Promise<boolean> {
        var model = { id: tripId, userId: userId };
        return this.http.post("/api/trip/remove", model).toPromise().then(() => { return true; });
    }

    getTrip(id: string, userId: string): Promise<TripViewModel> {
        return this.http.get(`/api/trip/get/${id}/${userId}`).toPromise()
            .then((result) => { return new TripViewModel(result.json()); });
    }

    getOwnTrips(userId: string): Promise<TripViewModel[]> {
        return this.http.get(`/api/trip/getown/${userId}`).toPromise()
            .then((result) => {
                return result.json();
            });
    }

    getInvitedTrips(userId: string): Promise<TripViewModel[]> {
        return this.http.get(`/api/trip/getinvited/${userId}`).toPromise()
            .then((result) => {
                return result.json();
            });
    }

    // Invites
    sendInvites(model: InvitesViewModel): Promise<boolean> {
        return this.http.post("/api/invite/send", model).toPromise().then(() => { return true });
    }

    acceptInvite(inviteId: number, userId: string): Promise<number> {
        var model = { inviteId: inviteId, userId: userId };
        return this.http.post("/api/invite/accept", model)
            .toPromise()
            .then((response) => { return parseInt(response.text()); });
    }

    // Messages
    sendMessage(model: MessageViewModel): Promise<boolean> {
        return this.http.post("/api/message/send", model).toPromise().then(() => { return true });
    }

    getAllMessages(chatId: number): Promise<MessageViewModel[]> {
        return this.http.get(`/api/message/getall/${chatId}`)
            .toPromise()
            .then((response) => { return response.json(); });
    }

    // Sights
    getSights(): Promise<SightObjectViewModel[]> {
        return this.http.get("/api/sight/get")
            .toPromise()
            .then((response) => { return response.json(); });
    }
}