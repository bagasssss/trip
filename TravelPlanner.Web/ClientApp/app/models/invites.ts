import { Mapper } from "../utils/helpers";

export class InvitesViewModel {
    constructor(invite?: InvitesViewModel) {
        if (invite) {
            Mapper.map(invite, this);
        }
    }

    public phones: string[];
    public invitorUserId: string;
    public tripId: number;
}