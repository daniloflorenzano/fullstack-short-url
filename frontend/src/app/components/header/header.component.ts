import {Component} from '@angular/core';
import * as Session from "supertokens-web-js/recipe/session";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  public rootId = "rootId";
  public userId = "";
  public session = false;

  ngAfterViewInit() {
    this.getUserInfo();
  }

  async getUserInfo() {
    this.session = await Session.doesSessionExist();
    if (this.session) {
      this.userId = await Session.getUserId();
    }
  }

  async onLogout() {
    await Session.signOut();
    window.location.reload();
  }

  redirectToLogin() {
    window.location.href = "auth";
  }

  redirectToHome() {
    window.location.href = "/";
  }
}
