import {AfterViewInit, Component, inject, signal} from "@angular/core";

import * as Session from "supertokens-web-js/recipe/session";
import {ProductsService} from "../services/products.service";
import {Product} from "../models/products.type";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent implements AfterViewInit {
  productService = inject(ProductsService);
  productItems = signal<Array<Product>>([]);

  public rootId = "rootId";
  public userId = "";
  public session = false;

  ngAfterViewInit() {
    this.getUserInfo();
    this.productItems.set(this.productService.productItems);
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
}
