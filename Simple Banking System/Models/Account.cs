
using Simple_Banking_System.Interfaces;

namespace Simple_Banking_System;
public class Account : IAccount, IEntity
{
    public Guid Id { get; private set; }
    public double Number { get; private set; }
    public string Name { get; init; }
    public decimal Balance { get; set; }
    public DateTime Created { get; }
    public DateTime LastUpdated { get; set; }

    public Account(string name, double number, DateTime? created = null, decimal balance = 0)
    {
        Number = number;
        Name = name;
        Balance = balance;
        Created = created ?? DateTime.UtcNow;
    }

    public bool SetId(Guid id)
    {
        if (id != default && (this.Id == Guid.Empty || this.Id == default))
        {
            this.Id = id;
            return true;
        }

        return false;
    }

    public bool SetNumber(double number)
    {
        if (this.Number == default && number != default)
        {
            this.Number = number;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $@"
            Account Id:     {Id}
            Account Number: {Number}
            Account Name:   {Name}
            -----------------------
            Actual Balance: {Balance}
            Date Created {Created:yyyy-MM-dd hh:mm:ss}
            ";
    }
}

public record AccountRecord(Guid Id, string Name, double Number, decimal Balance, DateTime DateCreated, DateTime LastUpdated)
{
    public AccountRecord(Account acc) : this(acc.Id, acc.Name, acc.Number, acc.Balance, acc.Created, acc.LastUpdated) {}

    public override string ToString()
    {
        return $@"
            Account Id:     {Id}
            Account Number: {Number}
            Account Name:   {Name}
            -----------------------
            Actual Balance: {Balance}
            Date Created {DateCreated:yyyy-MM-dd hh:mm:ss}
            Last Updated {LastUpdated:yyyy-MM-dd hh:mm:ss}
            "; ;
    }
}