import { NgModule } from "@angular/core";
import {CommonModule, NgOptimizedImage} from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomeComponent } from "./home.component";
import {PricingCardComponent} from "../components/pricing-card/pricing-card.component";
import {HeaderComponent} from "../components/header/header.component";

@NgModule({
    declarations: [HomeComponent],
  imports: [CommonModule, HomeRoutingModule, PricingCardComponent, HeaderComponent, NgOptimizedImage],
})
export class HomeModule {}
