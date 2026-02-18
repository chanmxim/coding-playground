package main

import "fmt"

type Contact struct{
	ID int
	Name string
	Email string
	Phone string
}

var contactList []Contact
var contactIndexByName map[string]int
var nextID int = 1

func init(){
	contactList = make([]Contact, 0)
	contactIndexByName = make(map[string]int)
}

func addContact(name, email, phone string){
	if _, exists := contactIndexByName[name]; exists{
		fmt.Printf("Contact already exists: %v\n", name)
		return
	}

	newContact := Contact{
		ID: nextID,
		Name: name,
		Email: email,
		Phone: phone,
	}

	contactList = append(contactList, newContact)
	contactIndexByName[name] = nextID
	nextID++

	fmt.Printf("Contact added: %v\n", name)
}

func findContact(name string) *Contact{
	index, exists := contactIndexByName[name]

	if exists{
		return &contactList[index]
	}

	return nil
}

func ListContacts(){
	fmt.Println("--- Listing Contacts ---")
	if len(contactList) == 0{
		fmt.Println("No contacts found")
		return
	}

	for i, contact := range contactList{
		fmt.Printf("%d. | ID: %d | Name: %s\n", i + 1, contact.ID, contact.Name)
	}

	fmt.Println("")
}

func main(){
	
	addContact("Max", "max@gmail.com", "111-2222")
	addContact("Carol", "carol@gmail.com", "222-3333")
	addContact("Alice", "alice@gmail.com", "222-4444")

	ListContacts()

	bob := findContact("Bob")
	if bob == nil{
		fmt.Println("Bob not found")
	} else {
		fmt.Println("Bob is found", bob.Name)
	}
}
