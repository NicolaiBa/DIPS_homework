using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bank.Services;
using Xunit;

/*
Development Task as requested by DIPS
Solution written by Nicolai Bakkeli 1.nov.2017
*/

namespace Bank.Tests.Services {
    public class BankService_Should {
        /* Class used to test Account, Person and BankClass from Bank.cs */
        private readonly Account _Account;
        private readonly BankClass _BankClass;
        private readonly Person _Person;
        private char[] numbers = "0123456789".ToCharArray ();

        public BankService_Should () {
            // Creates reoccuring class objects.
            _Person = new Person ("Ola", 50f);
            _Account = new Account (_Person, _Person.Money, 1);
            _BankClass = new BankClass ();
        }

        [Fact]
        public void TestPersonInitiation () {
            /* Tests initialization of the Person class. */
            int minNameLength = 3;

            // Class validity
            Assert.False ((!_Person.GetType ().IsClass), "Unexpected type");

            // Class attribute type tests
            Assert.True ((_Person.Name.GetType ().FullName is System.String), "Unexpected type; Class Person, Attribute Name");
            Assert.False ((_Person.Money.GetType () is System.Single), "Unexpected type; Class Person, Attribute Money");

            // Class attribute value tests
            Assert.False ((_Person.Name.Length < minNameLength), "Person's name is too short");
        }

        [Fact]
        public void TestAccountInitiation () {
            /* Tests initialization of the Account class. */
            int serialIndex = _Account.Name.IndexOfAny (numbers);
            int minNameLength = 3;
            float minStartBalance = 0f;

            // Class validity
            Assert.True ((_Account.GetType ().IsClass), "Unexpected type");

            // Class attribute type tests
            Assert.True ((_Account.Customer.Name.GetType ().FullName is System.String), "Unexpected type; Class Account, Attribute Person");
            Assert.False ((_Account.Money.GetType () is System.Single), "Unexpected type; Class Account, Attribute Money");
            Assert.False ((_Account.Serial.GetType () is System.Int32), "Unexpected type; Class Account, Attribute Serial");
            Assert.True ((_Account.Name.GetType ().FullName is System.String), "Unexpected type; Class Account, Attribute Name");

            // Class attribute value tests
            Assert.False ((_Account.Customer.Name.Length < minNameLength), "Account Holder's name is too short");
            Assert.False ((_Account.Money < minStartBalance), "Initial account balance too low");
            Assert.False ((_Account.Serial < 1), "Invalid serial number");
            Assert.False ((_Account.Name.Length < minNameLength), "Account name is too short");
            Assert.False ((serialIndex == -1), "Serial number not coupled with name");
        }

        [Fact]
        public void TestBankInitiation () {
            /* Tests initialization of the Bank class. */

            // Class validity
            Assert.False ((!_BankClass.GetType ().IsClass), "Unexpected type");

            // Crude class attribute type test
            Assert.True (_BankClass.Accounts is IList && _BankClass.Accounts.GetType ().IsGenericType);
        }

        [Fact]
        public void TestCreateAccount () {
            /* Tests CreateAccount (method) from the Bank class */  

            // Tests moneymovement from Person
            // Adds some money so there are enough.
            float deposit = 25f;
            _Person.Money += deposit;
            float preMoney = _Person.Money;
            Account returnAcc = _BankClass.CreateAccount (_Person, deposit);
            bool moneymoved = ((preMoney - _Person.Money - deposit) == 0);

            // Cheks if money is subtracted from Person.
            Assert.True (moneymoved, "Money not moved from Person.");

            // Cheks if subtracted amount is in account.
            Assert.True ((deposit == returnAcc.Money), "Deposited amount not stored in account");

            // Cheks if Account is created for broke Person.
            _Person.Money = (deposit - 1);
            bool illegalObject = _BankClass.CreateAccount (_Person, deposit) == null;
            Assert.True (illegalObject, "Class: Account illegally created.");

            // Cheks if serial number increments.
            _Person.Money = deposit;
            Account returnAcc2 = _BankClass.CreateAccount (_Person, deposit);
            Assert.False (returnAcc2.Serial < 2, "Serial not updated.");

            // Cheks if name combination is successful.
            string _name = returnAcc2.Name.Substring (0, returnAcc2.Name.IndexOfAny (numbers));
            string _serial = returnAcc2.Name.Substring (returnAcc2.Name.IndexOfAny (numbers));
            Assert.False (returnAcc2.Name.IndexOfAny (numbers) == -1, "Serial number not coupled with name");
            Assert.True ((_name == returnAcc2.Customer.Name), "Name Number != Serial Number.");
        }

        [Fact]
        public void TestGetAccountsForCustomer () {
            /* Tests GetAccountsForCustomer (method) from the Bank class */  

            // Creates some accounts to gather.
            int accontsToAdd = 12;
            Person other_person = new Person ("Hans", 0f);
            Person thrd_person = new Person ("Anna", 250f);
            foreach (int x in Enumerable.Range (1, accontsToAdd)) {
                _Person.Money = x * 100f;
                other_person.Money = x * 100f;
                _BankClass.CreateAccount (_Person, _Person.Money);
                _BankClass.CreateAccount (other_person, other_person.Money);
            }

            List<Account> customer_Accounts = _BankClass.GetAccountsForCustomer (_Person);
            List<Account> customer_Accounts2 = _BankClass.GetAccountsForCustomer (other_person);
            List<Account> customer_Accounts3 = _BankClass.GetAccountsForCustomer (thrd_person);

            // Return value must be a list.
            Assert.True ((customer_Accounts is IList && customer_Accounts.GetType ().IsGenericType), "GetAccountsForCustomer should return list");

            // Checks if the amount of accounts compare to the number created.
            Assert.True ((customer_Accounts.Count () == accontsToAdd), "GetAccountsForCustomer returns too few accounts.");

            // Checks if unwanted accounts are returned.
            foreach (Account acc in customer_Accounts) {
                Assert.False (customer_Accounts2.Contains (acc), "GetAccountsForCustomer returns too many accounts.");
            }

            // Empty return value must be list.
            Assert.True ((customer_Accounts3 is IList && customer_Accounts3.GetType ().IsGenericType), "GetAccountsForCustomer should return list");

            // Empty list must be empty.
            Assert.True ((customer_Accounts3.Count () == 0), "GetAccountsForCustomer Returnlist should be empty.");
        }
        [Fact]
        public void TestDeposit () {
            /* Tests Deposit (method) from the Bank class */

            _Person.Money = 500f;
            _Account.Money = 300f;
            float depositAmount = 100f;
            float pMoneyPre = _Person.Money;
            float aMoneyPre = _Account.Money;

            // Moves money.
            _BankClass.Deposit (_Account, depositAmount);

            float pMoneyOut = pMoneyPre - _Person.Money;
            float aMoneyIn = aMoneyPre - _Account.Money;

            bool totalityEquality = ((-1 * pMoneyOut) == aMoneyIn);

            // Money must be subtracted from person.
            Assert.False ((pMoneyOut == 0), "Money needs to be moved from person");
            // Money must be added to account.
            Assert.False ((aMoneyIn == 0), "Money needs to be moved to account");
            // An equal amount must be transfered.
            Assert.True (totalityEquality, "An amount needs to be moved from the person to the account");
        }
        [Fact]
        public void TestWithdraw () {
            /* Tests Withdraw (method) from the Bank class */
            
            _Person.Money = 500f;
            _Account.Money = 300f;
            float withdrawAmount = 100f;

            float pMoneyPre = _Person.Money;
            float aMoneyPre = _Account.Money;

            _BankClass.Withdraw (_Account, withdrawAmount);

            float pMoneyIn = _Person.Money - pMoneyPre;
            float aMoneyOut = _Account.Money - aMoneyPre;

            bool totalityEquality = ((-1 * pMoneyIn) == aMoneyOut);

            // Money must be added to person.
            Assert.False ((pMoneyIn == 0), "Money needs to be moved from person");
            // Money must be subtracted from account.
            Assert.False ((aMoneyOut == 0), "Money needs to be moved to account");
            // An equal amount must be transfered.
            Assert.True (totalityEquality, "An amount needs to be moved from the person to the account");
        }
        [Fact]
        public void TestTransfer () {
            /* Tests Transfer (method) from the Bank class */

            Person other_Person = new Person ("Hans", 200f);
            Account other_Account = new Account (other_Person, other_Person.Money, 1);
            _Person.Money = 500f;
            _Account.Money = 300f;
            other_Person.Money = 200f;
            other_Account.Money = 200f;
            float transferAmount = 100f;

            float senderMoneyPre = _Account.Money;
            float receiveMoneyPre = other_Account.Money;

            _BankClass.Transfer(_Account, other_Account, transferAmount);

            float senderMoneyDif = senderMoneyPre - _Account.Money;
            float receiveMoneyDir = receiveMoneyPre - other_Account.Money;

            bool totalityEquality = ((-1 * senderMoneyDif) == receiveMoneyDir);

            // Money must be subtracted from Account.
            Assert.False ((senderMoneyDif == 0), "Money needs to be moved from Account (sender)");
            // Money must be added to Account.
            Assert.False ((receiveMoneyDir == 0), "Money needs to be moved to Account (receiver)");
            // An equal amount must be transfered.
            Assert.True (totalityEquality, "Transfered amount needs to be equal on each end.");
        }
    }
}