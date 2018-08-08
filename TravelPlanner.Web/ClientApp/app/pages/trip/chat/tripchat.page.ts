import { Component } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: './tripchat.page.html'
})
export class TripChatPage {
    chatId: number;

    constructor(private route: ActivatedRoute) {
    }

    ngOnInit() {
        this.chatId = this.route.parent.snapshot.params["id"];
    }
}
