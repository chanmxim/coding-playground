package main

import (
	"errors"
	"fmt"
)

type Account struct{
	AccountNumber string
	Balance float64
	OwnerName string
}

func (acc *Account) Deposit(amount float64) error{
	if amount <= 0{
		return errors.New("[!] Deposit amount must be positive")
	}

	acc.Balance += amount
	fmt.Printf("[+] Deposited $%.2f to %s. New Balance: $%.2f\n", amount, acc.AccountNumber, acc.Balance)
	
	return nil
}

func (acc *Account) Withdraw(amount float64) error{
	if amount <= 0{
		return errors.New("[!] Withdrawal amount must be positive")
	}

	if acc.Balance < amount{
		return fmt.Errorf("[!] Insufficient funds in %s. Balance: %.2f, attempt to withdraw: $%.2f",
			acc.AccountNumber, acc.Balance, amount)
	}

	acc.Balance -= amount
	fmt.Printf("[+] Withdrew $%.2f from %s. New Balance: $%.2f\n", amount, acc.AccountNumber, acc.Balance)
	
	return nil
}

func (acc *Account) GetBalance() float64 {return acc.Balance}

func (acc *Account) String() string{
	return fmt.Sprintf("[*] Account [%s] Owner: %s, Balance: $%.2f",
		acc.AccountNumber, acc.OwnerName, acc.Balance)
}

type SavingsAccount struct{
	Account
	InterestRate float64
}

func (sa *SavingsAccount) AddInterest() error{
	interest := sa.Balance * sa.InterestRate
	
	fmt.Printf("[*] Adding interest $%.2f to savings account %s", interest, sa.AccountNumber)
	err := sa.Deposit(interest)

	if err != nil{
		fmt.Printf("[!] Error depositing $%.2f to savings account. %v\n", interest, err)
	}

	return nil
}

type OverdraftAccount struct{
	Account
	OverdraftLimit float64
}

func (oa *OverdraftAccount) Withdraw(amount float64) error{
	if amount <= 0{
		return errors.New("[!] Withdrawal amount must be positive")
	}

	if (oa.Balance + oa.OverdraftLimit) < amount{
		return fmt.Errorf("[!] Attempt to withdraw $%.2f exceed limit for overdraft account %s. Balance: $%.2f", 
			amount, oa.AccountNumber, oa.Balance)
	}

	oa.Balance -= amount
	fmt.Printf("[+] Withdrew $%.2f from overdraft account %s. New Balance: $%.2f\n", amount, oa.AccountNumber, oa.Balance)

	return nil
}

func main(){
	
	fmt.Println("--- Bank Account System ---")

	sa := SavingsAccount{
		Account: Account{
			AccountNumber: "SA001",
			Balance: 1000.00,
			OwnerName: "Alice",
		},
		InterestRate: 0.02,
	}

	fmt.Println("\n--- Savings Account Operations ---")
	fmt.Println(sa.String())

	err := sa.Deposit(200)
	if err != nil{
		fmt.Printf("%v\n", err)
	}

	err = sa.AddInterest()
	if err != nil{
		fmt.Printf("%v\n", err)
	}

	fmt.Println(sa.String())

	// ==============
	oa := OverdraftAccount{
		Account: Account{
			AccountNumber: "OVD001",
			Balance: 100.00,
			OwnerName: "Bob",
		},
		OverdraftLimit: 100.00,
	}

	fmt.Println("\n--- Overdraft Account Operations ---")
	fmt.Println(oa.String())

	err = oa.Withdraw(150.00)
	if err != nil{
		fmt.Printf("%v\n", err)
	}

	err = oa.Withdraw(100.00)
	if err != nil{
		fmt.Printf("%v\n", err)
	}

	fmt.Println(oa.String())
}