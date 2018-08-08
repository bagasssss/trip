import { Mapper } from "../utils/helpers";

export class MessageViewModel {
    constructor(msg?: MessageViewModel) {
        if (msg) {
            Mapper.map(msg, this);
        }
    }

    id: string;
    chatId: number;
    userId: string;
    author: string;
    text: string;
    sentDate?: string;
}