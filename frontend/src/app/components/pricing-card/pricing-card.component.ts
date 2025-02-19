import {Component, input, Input, numberAttribute} from '@angular/core';
import {Product} from "../../models/products.type";

@Component({
  selector: 'app-pricing-card',
  standalone: true,
  imports: [],
  templateUrl: './pricing-card.component.html',
  styleUrl: './pricing-card.component.css'
})
export class PricingCardComponent {
  @Input() product!: Product;
}
