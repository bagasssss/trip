import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { WebSocketService } from './websocket.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/filter';
import { MessageViewModel } from "../models/message";

@Injectable()
export class ChatService {
    public messages: Subject<MessageViewModel> = new Subject<MessageViewModel>();

    constructor(private wsService: WebSocketService) {
        var uri = "ws://" + window.location.host + "/ws";
        this.messages = <Subject<MessageViewModel>>this.wsService
            .connect(uri)
            .map((response: MessageEvent): MessageViewModel => {
                let data = JSON.parse(response.data);
                return {
                    id: data.id,
                    userId: data.userId,
                    chatId: data.chatId,
                    author: data.author,
                    text: data.text,
                    sentDate: data.sentDate
                }
            });
    }
}