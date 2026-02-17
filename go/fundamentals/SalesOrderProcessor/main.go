package main

import (
	"fmt"
	"strings"
)

var productPrices = map[string]float64{
	"TSHIRT": 20.00,
	"MUG": 12.50,
	"HAT": 18.00,
	"BOOK": 25.99,
}

func calculateItemPrice(itemCode string) (float64, bool){
	basePrice, found := productPrices[itemCode]

	if !found{
		if strings.HasSuffix(itemCode, "_SALE"){
			originalItemCode := strings.TrimSuffix(itemCode, "_SALE")
			basePrice, found = productPrices[originalItemCode]

			if found{
				salePrice := basePrice * 0.90
				fmt.Printf(" - Item %s (Sale! Original: $%.2f, Sale Price: $%.2f)\n",
					originalItemCode, basePrice, salePrice)

				return salePrice, true
			}
		}

		fmt.Printf(" - Item: %s (Product Not Found)\n", itemCode)
		return 0.0, false
	}

	return basePrice, true
}

func main(){
	fmt.Println("------Sales Order Processor------")

	orderItems := []string{
		"TSHIRT", "MUG_SALE", "HAT", "BOOK", "PANTS",
	}

	var subtotal float64
	fmt.Println(" - Processing order items...")

	for _, item := range orderItems{
		price, found := calculateItemPrice(item)

		if found{
			subtotal += price
		}
	}

	fmt.Printf("Subtotal: $%.2f\n", subtotal)
}
