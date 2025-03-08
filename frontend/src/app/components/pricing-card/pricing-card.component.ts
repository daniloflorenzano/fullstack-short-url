import {Component, Input} from '@angular/core';
import {Product} from "../../models/products.type";
import {NgOptimizedImage} from "@angular/common";

@Component({
  selector: 'app-pricing-card',
  standalone: true,
  imports: [
    NgOptimizedImage
  ],
  templateUrl: './pricing-card.component.html',
  styleUrl: './pricing-card.component.css'
})
export class PricingCardComponent {
  @Input() product!: Product;
}
