import {Injectable} from '@angular/core';
import {Product} from "../models/products.type";

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  productItems: Array<Product> = [
    {
      id: 1,
      title: "Just need a few links?",
      subtitle: "Perfect for quick, one-time short links!",
      price: 0.99,
      valueInCredits: 5,
      description:
        [
          "1 credit = 1 shortened URL with full analytics",
          "Customize your link to make unique",
          "No expiration date"
        ],
      image: "/img/hand-give.svg",
    },
    {
      id: 2,
      title: "Frequent user?",
      subtitle: "Great for small businesses and regular use!",
      price: 4.99,
      valueInCredits: 20,
      description:
        [
          "1 credit = 1 shortened URL with full analytics",
          "Customize your link to make unique",
          "No expiration date"
        ],
      image: "/img/hand-give.svg",
    },
    {
      id: 3,
      title: "Power user pack!",
      subtitle: "For those ho shorten URLs like a pro!",
      price: 9.99,
      valueInCredits: 50,
      description:
        [
          "1 credit = 1 shortened URL with full analytics",
          "Customize your link for better branding",
          "Maximum savings per link"
        ],
      image: "/img/hand-give.svg",
    },
  ];

  constructor() {
  }
}
