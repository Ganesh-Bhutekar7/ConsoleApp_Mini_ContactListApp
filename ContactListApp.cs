using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ContactListApp
{
    // ========== Represents a single contact with Name, Phone, and Email
    class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            // Display contact information in aligned format
            return $"Name: {Name,-20} Phone: {PhoneNumber,-15} Email: {Email}";
        }
    }

    class Program
    {
        //========== Main contact storage
        static List<Contact> contacts = new List<Contact>();
        //========== HashSets to ensure uniqueness
        static HashSet<string> contactNames = new HashSet<string>();
        static HashSet<string> contactPhones = new HashSet<string>();
        static HashSet<string> contactEmails = new HashSet<string>();

        static void Main()
        {
            Console.Title = "Professional Contact List App";
            Console.ForegroundColor = ConsoleColor.Cyan;

            bool exit = false;
            while (!exit)
            {
                ShowMenu();

                Console.Write("\nSelect an option (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddContact(); break;   // Add a new contact
                    case "2": ViewContacts(); break; // View all contacts alphabetically
                    case "3": SearchContact(); break; // Search for a contact by name
                    case "4": DeleteContact(); break; // Delete a contact by name
                    case "5": exit = true; break;    // Exit the application
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option! Try again.");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear(); // Clear screen for better readability
                }
            }

            Console.WriteLine("\nThank you for using Contact List App. Goodbye!");
        }

        // ==========Displays main menu options
        static void ShowMenu()
        {
            Console.WriteLine("===============================================");
            Console.WriteLine("           Professional Contact List App       ");
            Console.WriteLine("===============================================");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. View All Contacts");
            Console.WriteLine("3. Search Contact");
            Console.WriteLine("4. Delete Contact");
            Console.WriteLine("5. Exit");
            Console.WriteLine("===============================================");
        }

        // =========Add a new contact with validations for uniqueness and format
        static void AddContact()
        {
            string name;
            while (true)
            {
                Console.Write("Enter Name: ");
                name = Console.ReadLine().Trim();
                if (string.IsNullOrEmpty(name))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Name cannot be empty!");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (contactNames.Contains(name.ToLower()))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Contact with this name already exists!");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                    break;
            }

            string phone;
            while (true)
            {
                Console.Write("Enter Phone Number: ");
                phone = Console.ReadLine().Trim();
                if (!Regex.IsMatch(phone, @"^\d{10}$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid phone number! Must be 10 digits.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (contactPhones.Contains(phone))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Phone number already exists!");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                    break;
            }

            string email;
            while (true)
            {
                Console.Write("Enter Email: ");
                email = Console.ReadLine().Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid email format!");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (contactEmails.Contains(email.ToLower()))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Email already exists!");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else
                    break;
            }

            var newContact = new Contact
            {
                Name = name,
                PhoneNumber = phone,
                Email = email
            };

            //=========== Add contact to list and update uniqueness sets
            contacts.Add(newContact);
            contactNames.Add(name.ToLower());
            contactPhones.Add(phone);
            contactEmails.Add(email.ToLower());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nContact '{name}' added successfully!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        //========= Display all contacts in alphabetical order by name
        static void ViewContacts()
        {
            if (contacts.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No contacts found.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                return;
            }

            var sortedContacts = contacts.OrderBy(c => c.Name).ToList();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n{"Name",-20} {"Phone",-15} {"Email"}");
            Console.WriteLine(new string('-', 50));
            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (var contact in sortedContacts)
                Console.WriteLine(contact);
        }

        // =========== Search for contacts by name (case-insensitive)
        static void SearchContact()
        {
            Console.Write("\nEnter Name to search: ");
            string searchName = Console.ReadLine().Trim().ToLower();

            var result = contacts.Where(c => c.Name.ToLower().Contains(searchName)).ToList();

            if (result.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No contacts found with that name.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                return;
            }

            Console.WriteLine("\n--- Search Results ---");
            foreach (var contact in result)
                Console.WriteLine(contact);
        }

        //=========== Delete contact by name and update uniqueness sets
        static void DeleteContact()
        {
            Console.Write("\nEnter Name to delete: ");
            string deleteName = Console.ReadLine().Trim().ToLower();

            var contactToRemove = contacts.FirstOrDefault(c => c.Name.ToLower() == deleteName);

            if (contactToRemove == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Contact not found!");
                Console.ForegroundColor = ConsoleColor.Cyan;
                return;
            }

            contacts.Remove(contactToRemove);
            contactNames.Remove(deleteName);
            contactPhones.Remove(contactToRemove.PhoneNumber);
            contactEmails.Remove(contactToRemove.Email.ToLower());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nContact '{contactToRemove.Name}' deleted successfully!");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}
