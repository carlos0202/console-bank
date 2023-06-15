//namespace Simple_Banking_System;

//public class BankController
//{
//    public IAccountController this[int key] => Bank.Instance.Accounts[key];
    
//    public static int NextAccountIndex() => Bank.Instance.Accounts.Count + 1;

//    public static bool AccountExists(int id)
//    {
//        var acc = Bank.Instance.Accounts[id];
//        return Bank.Instance.Accounts.ContainsKey(id);
//    }
    
//    public static int CreateAccount()
//    {
//        Console.WriteLine("Enter your name:");
//        string name = Console.ReadLine() ?? "";
//        while (name.Length == 0)
//        {
//            Console.WriteLine("Name length must be greater than 0, please re-enter.");
//            name = Console.ReadLine() ?? "";
//        }
//        while (name.Any(c => char.IsDigit(c)))
//        {
//            Console.WriteLine("Please enter only characters for name (no numbers).");
//            name = Console.ReadLine() ?? "";
//        }
//        var account = new Account(name);
//        Bank.Instance.Accounts.Add(account.Number, new AccountController(account));
//        return account.Number;
//    }
//}