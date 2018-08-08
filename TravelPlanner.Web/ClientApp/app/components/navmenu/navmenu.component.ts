import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from "../../services/auth.service";

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html'
})
export class NavMenuComponent {
    constructor(private authService: AuthService, private router: Router) {
    }

    isLoggedIn() {
        return this.authService.isLoggedIn;
    }

    getUserName() {
        return this.isLoggedIn() ? this.authService.user.userName : "";
    }

    logout = () => {
        this.authService.logout();
        this.router.navigate(['/login']);
    }

    showUserInfo() {
        console.log(this.authService.user);
    }
}
