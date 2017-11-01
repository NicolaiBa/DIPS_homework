using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
Development Task as requested by DIPS
Solution written by Nicolai Bakkeli 1.nov.2017
*/

namespace Bank.Services {

    public class Person {
        /* Class containing personal information on customers.
            Name:  name of customer.
            Money: money owned by the customer currently ouside the bank.
        */
        public string Name { get; set; }
        public float Money { get; set; }

        public Person (string name, float money) {
            Name = name;
            Money = money;
        }
    }

    public class Account {
        /* Class containing account information.
            Customer: see Person class.
            Money:    money in this Account.
            Serial:   Trailing number (unnessesary realy).
            Name:     Account name.
        */
        public Person Customer { get; set; }
        public float Money { get; set; }
        public int Serial { get; set; }
        public string Name;

        public Account (Person person, float firstDeposit, int serial) {
            Customer = person;
            Money = firstDeposit;
            Serial = serial;
            Name = person.Name + serial;
        }
    }

    public class BankClass {
        /* Class containing a list of Accounts (Class).*/
        public List<Account> Accounts = new List<Account> ();

        public Account CreateAccount (Person customer, float initialDeposit) {
            /* Creates an Account with a given balance for a target customer.
            Customer money is transfered to the new account.
            Customer balance must at lest meet initialDeposit.
            Inputs : customer Person (Class),
                    initialDeposit float.
            Output : Account (see Account class).*/

            List<Account> customer_Accounts = GetAccountsForCustomer (customer);

            // Avoid creating empty accounts.
            if (initialDeposit == 0f) {
                Console.WriteLine ("Too small deposit for account creation.");
            } // Account is created only if customer have enough money.
            else if (customer.Money < initialDeposit) {
                Console.WriteLine ("Insufficient funds for account creation.");
            } else {
                // Finds the next serial number for a new Account.
                int maxSerial = 0;
                foreach (Account acc in customer_Accounts) {
                    if (acc.Serial > maxSerial) {
                        maxSerial = acc.Serial;
                    }
                }

                // Creates the new account.
                Account new_acc = new Account (customer, initialDeposit, maxSerial + 1);
                Accounts.Add (new_acc); //

                // Removes money from Person.
                customer.Money -= initialDeposit;

                return new_acc;
            }
            return null;
        }

        public List<Account> GetAccountsForCustomer (Person customer) {
            /*  Returns a list containing every account belonging too target customer.
                Input:  customer (Class : Person)
                Output : List of Accounts (Class)
            */
            List<Account> return_list = new List<Account> ();

            foreach (Account acc in Accounts) {
                string account_name = acc.Customer.Name;

                bool owner_ship = (string.Compare (customer.Name, acc.Customer.Name) == 0);
                if (owner_ship) {
                    return_list.Add (acc);
                }
            }
            return return_list;
        }

        public void Deposit (Account to, float amount) {
            /* Moves money from the customer to the customers account.*/

            if (amount <= 0) {
                Console.WriteLine ("Amount to depost too low.");
            } else if (to.Customer.Money < amount) {
                Console.WriteLine ("Customer can't depost that amount.");
            } else {
                to.Customer.Money -= amount;
                to.Money += amount;
            }
        }

        public void Withdraw (Account from, float amount) {
            /* Moves money from the account to the customer. */

            if (amount <= 0) {
                Console.WriteLine ("Amount to withdraw too low.");
            } else if (from.Money < amount) {
                Console.WriteLine ("Account doesn't have that amount.");
            } else {
                from.Money -= amount;
                from.Customer.Money += amount;
            }
        }
        public void Transfer (Account from, Account to, float amount) {
            /* Transfers money between accounts. */
            if (amount <= 0) {
                Console.WriteLine ("Amount to transfer too low.");
            } else if (from.Money < amount) {
                Console.WriteLine ("Account doesn't have that amount.");
            } else {
                from.Money -= amount;
                to.Money += amount;
            }
        }
    }
}