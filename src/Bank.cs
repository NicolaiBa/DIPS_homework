using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Account
{
    public string Person { get; set; }
    public float  Money { get; set; }
    public Account(string person, float money)
    {
        Person = person;
        Money = money;
    }
}

public class Bank
{

    public List<Account> Accounts;


    public Bank()
    {
        List<Account> Accounts = new List<Account>();

        Console.WriteLine("Pre");
        foreach (Account acc in Accounts)
        {
            Console.WriteLine(acc.Person);    
        }
        
        Console.WriteLine("Post");
    }

}

class TestClass
{
    static void Main()
    {
        Bank main_bank = new Bank();
    }
}
