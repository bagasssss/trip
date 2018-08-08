import { Component } from '@angular/core';
import { TripViewModel } from "../../models/trip/trip";
import { BackendService } from "../../services/backend.service";
import { Constants } from "../../models/constants";
import { UserHelper } from "../../utils/helpers";
import { GlobalService } from "../../services/global.service";

@Component({
    selector: 'newtrip',
    templateUrl: './newtrip.page.html'
})
export class NewTripPage {
    newtrip = new TripViewModel();

    constructor(private backendService: BackendService,
                private globalService: GlobalService) {

        this.newtrip.creatorId = UserHelper.getUserId();
    }

    createTrip() {
        this.backendService.createTrip(this.newtrip).then((id) => {
            this.globalService.showSuccessMsg("tripCreated");
            this.globalService.navigateToRoute('/triproute/' + id);
        });
    }
}
