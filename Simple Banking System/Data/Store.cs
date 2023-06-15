using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Banking_System.Data
{
    public class Store
    {
        private static readonly Store _instance = new();
        private double _accountNumberSeed;
        private readonly IDictionary<double, Account> AccountNumbersLookup;
        private readonly bool _initialied = false;
        
        public static Store Instance { get { return _instance; } }

        private Store()
        {
            if(_initialied) return;
            this.AccountNumbersLookup = new Dictionary<double, Account>();
            this._accountNumberSeed = 1000000000;
            _initialied = true;
        }

        public bool AddAccount(Account account)
        {
            if (IsAccountFound(account.Number))
            {
                return false;
            }

            _instance._accountNumberSeed++;
            account.SetId(Guid.NewGuid());
            account.SetNumber(_instance._accountNumberSeed);
            account.LastUpdated = DateTime.UtcNow;
            _instance.AccountNumbersLookup[account.Number] = account;

            return true;
        }

        public (bool, AccountRecord?) DoDeposit(double accountNumber, decimal amount)
        {
            if(amount <= 0)
            {
                throw new ArgumentException(
                    $"Invalid amount to deposit ({accountNumber}). Must be greater than zero.", nameof(accountNumber));
            }

            if (!IsAccountFound(accountNumber))
            {
                return (false, null);
            }

            var account = _instance.AccountNumbersLookup[accountNumber];
            account.Balance += amount;
            account.LastUpdated = DateTime.UtcNow;

            return (true, new AccountRecord(account));
        }

        public (bool, AccountRecord?) DoWithdraw(double accountNumber, decimal amount)
        {
            if (!IsAccountFound(accountNumber))
            {
                return (false, null);
            }

            var account = _instance.AccountNumbersLookup[accountNumber];

            if(account.Balance <  amount)
            {
                throw new ArgumentException("Account doesn't have enough founds", nameof(accountNumber));
            }

            account.Balance -= amount;
            account.LastUpdated = DateTime.UtcNow;

            return (true, new AccountRecord(account));
        }

        public (bool success, AccountRecord source, AccountRecord destination) DoTransfer(double sourceAccountNumber, 
            double destinationAccountNumber, decimal amount)
        {
            if (!IsAccountFound(sourceAccountNumber)) 
            {
                throw new ArgumentException($"Source account not found by given account number: {sourceAccountNumber}");
            }

            if (!IsAccountFound(destinationAccountNumber)) 
            {
                throw new ArgumentException($"Destination account not found by given account number: {destinationAccountNumber}");
            }

            var sourceAccount = _instance.AccountNumbersLookup[sourceAccountNumber];
            var destinationAccount = _instance.AccountNumbersLookup[destinationAccountNumber];

            if (sourceAccount.Balance < amount)
            {
                throw new ArgumentException("Source Account doesn't have enough founds", nameof(sourceAccountNumber));
            }

            sourceAccount.Balance -= amount;
            sourceAccount.LastUpdated = DateTime.UtcNow;
            destinationAccount.Balance += amount;
            destinationAccount.LastUpdated = DateTime.UtcNow;

            return (true, new AccountRecord(sourceAccount), new AccountRecord(destinationAccount));
        }

        public bool DeleteAccount(double accountNumber)
        {
            if (!IsAccountFound(accountNumber))
            {
                return false;
            }

            return _instance.AccountNumbersLookup.Remove(accountNumber);
        }

        public bool IsAccountFound(double accountNumber) => _instance.AccountNumbersLookup.ContainsKey(accountNumber);

        public AccountRecord GetAccountDetails(double accountNumber)
        {
            if (IsAccountFound(accountNumber))
            {
                var acc = _instance.AccountNumbersLookup[accountNumber];

                return new AccountRecord(acc);
            }

            throw new Exception($"Account data was not found with the account number provided: {accountNumber}.");
        }
    }
}
