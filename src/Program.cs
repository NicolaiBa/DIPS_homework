using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TestClass {
    static void Main () {
        Bank main_bank = new Bank ();

        string string_test = "heiha";
        float float_test = 123.33f;

        Account oAcc = (Account) main_bank.CreateAccount (string_test, float_test);
        Account oAcc2 = (Account) main_bank.CreateAccount (string_test, float_test);

        Console.WriteLine ("oAcc.Person: " + oAcc.Person);
        Console.WriteLine ("oAcc2.Person: " + oAcc2.Person);

        List<Account> Accounts = main_bank.GetAccountForCustomer (string_test);

        foreach (Account acc in Accounts) {
            Console.WriteLine (acc.Person);
            Console.WriteLine (acc.Serial);
            Console.WriteLine (acc.Name);
            Console.WriteLine (acc.Money);
        }
    }
}

public class Account {
    public string Person { get; set; }
    public float Money { get; set; }
    public int Serial { get; set; }
    public string Name;
    public Account (string person, float money, int serial) {
        Person = person;
        Money = money;
        Serial = serial;
        Name = person + serial;
    }
}

public class Bank {
    public List<Account> Accounts = new List<Account> ();

    public Account CreateAccount (string customer, float initialDeposit) {
        List<Account> customer_Accounts = GetAccountForCustomer (customer);

        int maxSerial = 0;
        foreach (Account acc in customer_Accounts) {
            if (acc.Serial > maxSerial) {
                maxSerial = acc.Serial;
            }
        }

        Account new_acc = new Account (customer, initialDeposit, maxSerial + 1);
        Accounts.Add (new_acc);

        return new_acc;
    }

    public List<Account> GetAccountForCustomer (string customer) {
        List<Account> return_list = new List<Account> ();

        foreach (Account acc in Accounts) {
            string account_name = acc.Person;

            bool owner_ship = (string.Compare (customer, acc.Person) == 0);
            if (owner_ship) {
                return_list.Add (acc);
            }
        }
        return return_list;
    }

    public void Deposit (string to, float amount) {
        //if (amount <= 0f)
        //{
        //   
        //}

        foreach (Account acc in Accounts) {
            bool owner_ship = (string.Compare (to, acc.Name) == 0);
            if (owner_ship) {
                acc.Money += amount;
            }
        }
    }
}

/*
    public List<Account> GetAccountForCustomer(string customer)
    {
        char[] numbers = "0123456789".ToCharArray();
        List<Account> return_list = new List<Account>();

        foreach (Account acc in Accounts)
        {
            string account_name = acc.Person;

            // Index of first number.
            int number_index = account_name.IndexOfAny(numbers);
            if (number_index != -1)
            {
                string account_holder = account_name.Substring(0, number_index);

                // owner_ship = 0 -> True.
                bool owner_ship = (string.Compare(customer, account_holder) == 0);
                
                if (owner_ship)
                {
                    return_list.Add(acc);
                }
            }  
        }
        return return_list;
    }
*/