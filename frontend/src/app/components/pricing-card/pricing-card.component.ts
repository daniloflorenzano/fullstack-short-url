import {Component, Input, numberAttribute} from '@angular/core';

@Component({
  selector: 'app-pricing-card',
  standalone: true,
  imports: [],
  templateUrl: './pricing-card.component.html',
  styleUrl: './pricing-card.component.css'
})
export class PricingCardComponent {
  @Input() planTitle!: string;
  @Input() price!: string;
  @Input({transform: numberAttribute}) credits!: number;
}
