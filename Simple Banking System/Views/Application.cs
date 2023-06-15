using Simple_Banking_System.Data;
using Simple_Banking_System.Interfaces;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;


namespace Simple_Banking_System.Views
{
    public class Application : IApplication
    {
        private readonly Store _store;
        private readonly List<char> _validOptions = new() { 'A', 'B', 'C', 'D', 'E', 'F' };

        public Application(Store store)
        {
            _store = store;
        }

        public void CheckAccountBalance()
        {
            ClearAndContinue();

            Console.WriteLine("Check existing account balance:\n");
            while (true)
            {
                var (accNumber, captured) = AskForSimpleValue<double>("\nInsert your account number", false);
                if (captured && accNumber != default)
                {
                    if (!_store.IsAccountFound(accNumber))
                    {
                        Console.WriteLine("Account not found. Please check your input data and try again later...");
                        ClearAndContinue();

                        return;
                    }

                    Console.WriteLine($@"Your account details:
                        {_store.GetAccountDetails(accNumber)}");

                    return;
                }
                else
                {
                    var (opt, isCapture) = AskForSimpleValue<char>("Press R to try again or C to cancel current process...", false);
                    if (char.ToUpper(opt) == 'R' && isCapture)
                    {
                        ClearAndContinue();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Process cancelled...");
                        ClearAndContinue();

                        return;
                    }
                }
            }
        }

        public (T?, bool) AskForSimpleValue<T>(string message, bool enableEscaping = true) where T : IConvertible
        {
            IConvertible? value = default(T?);
            bool valueCaptured = false;
            message = enableEscaping
                ? message + " or write [EXIT] to Cancel!!!"
                : message;

            while(true)
            {
                Console.WriteLine(message);
                var rawValue = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(rawValue))
                {
                    Console.WriteLine("\nMust supply the required data...");

                    continue;
                }

                if(rawValue.ToUpper() == "[EXIT]" && enableEscaping)
                {
                    Console.WriteLine("\nAborting data capture. Press a key to continue...");
                    Console.ReadKey();

                    break;
                }

                try
                {
                    if (typeof(T) == typeof(decimal))
                    {
                        value = Convert.ToDecimal(rawValue);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        value = Convert.ToInt32(rawValue);
                    }
                    else if (typeof(T) == typeof(long))
                    {
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        value = rawValue;
                    }
                    else if (typeof(T) == typeof(char))
                    {
                        value = rawValue[0];
                    }
                    else if(typeof(T) == typeof(double))
                    {
                        value = Convert.ToDouble(rawValue);
                    }
                    else
                    {
                        throw new Exception("Unexpected input value supplied...");
                    }
                    valueCaptured = true;
                    break;
                } catch (Exception)
                {
                    Console.WriteLine($"Invalid input value supplied. Expected input data of type {typeof(T)}");

                    continue;
                }
            }

            return ((T?)value, valueCaptured);
        }

        public void CreateNewAccount()
        {
            Console.WriteLine("Let's create your new account:");
            while(true)
            {
                ClearAndContinue();
                var (value, captured) = AskForSimpleValue<string>("\nInsert your desired account name, ");
                if (captured && value != null)
                {
                    Account acc = new Account(value, default);
                    if (_store.AddAccount(acc))
                    {
                        Console.WriteLine($"Account created successfully. \nYour new account data is:\n{acc}");

                        return;
                    } else
                    {
                        Console.WriteLine("Error setting up your new account. Please try again later...");

                        return;
                    }
                } else
                {
                    var (opt, isCapture) = AskForSimpleValue<char>("Press R to try again or C to cancel account creation process...", false);
                    if(char.ToUpper(opt) == 'R' && isCapture)
                    {
                        continue;
                    } else 
                    { 
                        Console.WriteLine("Process cancelled...");

                        return;
                    }
                }
            };
        }

        public void DoDeposit()
        {
            ClearAndContinue();

            Console.WriteLine("Deposit funds into an existing account:\n");
            while (true)
            {
                var (accNumber, captured) = AskForSimpleValue<double>("\nInsert your account number", false);
                if (captured && accNumber != default)
                {
                    if (!_store.IsAccountFound(accNumber))
                    {
                        Console.WriteLine("Account not found. Please check your input data and try again later...");

                        return;
                    }

                    var (amountToDeposit, isCaptured) = AskForSimpleValue<decimal>(
                        "\nInsert the amount to deposit into your account:", false);
                    if (isCaptured && amountToDeposit > 0)
                    {
                        try
                        {
                            if (_store.DoDeposit(accNumber, amountToDeposit))
                            {
                                Console.WriteLine(@$"
                                    Amount successfully added to your account No. {accNumber}. 
                                    Amount added: {amountToDeposit}.");

                                return;
                            }
                            else
                            {
                                Console.WriteLine("Selected account was not found. Check your data and try later...");

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error doing deposit to your account: {ex.Message}");

                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error inserting data. Please try again later...");

                        return;
                    }
                }
                else
                {
                    var (opt, isCapture) = AskForSimpleValue<char>("Press R to try again or C to cancel current process...", false);
                    if (char.ToUpper(opt) == 'R' && isCapture)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Process cancelled...");

                        return;
                    }
                }
            }
        }

        public void DoTransfer()
        {
            ClearAndContinue();

            Console.WriteLine("Transfer funds between existing accounts:\n");
            while (true)
            {
                var (sourceAccNumber, sourceAccCaptured) = AskForSimpleValue<double>("\nInsert source account number", false);
                var (targetAccNumber, targetAccCaptured) = AskForSimpleValue<double>("\nInsert target account number", false);
                if (sourceAccCaptured && targetAccCaptured)
                {
                    if (!_store.IsAccountFound(sourceAccNumber) )
                    {
                        Console.WriteLine("Source account data not found. Please check your input data and try again later...");

                        return;
                    }

                    if (!_store.IsAccountFound(targetAccNumber))
                    {
                        Console.WriteLine("Target account data not found. Please check your input data and try again later...");

                        return;
                    }

                    var (amountToTransfer, isCaptured) = AskForSimpleValue<decimal>(
                        "\nInsert the amount to transfer between accounts:", false);
                    if (isCaptured && amountToTransfer > 0)
                    {
                        try
                        {
                            if (_store.DoTransfer(sourceAccNumber, targetAccNumber, amountToTransfer))
                            {
                                Console.WriteLine(@$"
                                    Amount successfully transferred between the following accounts:
                                        Source Account No. {sourceAccNumber}
                                        Target Account No. {targetAccNumber}
                                    Amount transfered: {amountToTransfer}.");

                                return;
                            }
                            else
                            {
                                Console.WriteLine("Selected accounts data was not found. Check your data and try later...");

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error doing transfer between accounts: {ex.Message}");

                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error inserting data. Please try again later...");

                        return;
                    }
                }
                else
                {
                    var (opt, isCapture) = AskForSimpleValue<char>("Press R to try again or C to cancel current process...", false);
                    if (char.ToUpper(opt) == 'R' && isCapture)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Process cancelled...");

                        return;
                    }
                }
            }
        }

        public void DoWithdraw()
        {
            ClearAndContinue();
            Console.WriteLine("Withdraw funds from an existing account:\n");
            while (true)
            {
                var (accNumber, captured) = AskForSimpleValue<double>("\nInsert your account number", false);
                if (captured && accNumber != default)
                {
                    if (!_store.IsAccountFound(accNumber))
                    {
                        Console.WriteLine("Account not found. Please check your input data and try again later...\n");

                        return;
                    }

                    var (amountToWithdraw, isCaptured) = AskForSimpleValue<decimal>(
                        "\nInsert the amount to withdraw from your account:", false);
                    if (isCaptured && amountToWithdraw > 0)
                    {
                        try
                        {
                            if (_store.DoWithdraw(accNumber, amountToWithdraw))
                            {
                                Console.WriteLine(@$"
                                    Amount successfully taken from your account No. {accNumber}. 
                                    Amount taken: {amountToWithdraw}.");

                                return;
                            }
                            else
                            {
                                Console.WriteLine("Selected account was not found. Check your data and try later...\n");

                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error attempting to withdraw from your account: {ex.Message}\n");

                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error inserting data. Please try again later...\n");

                        return;
                    }
                }
                else
                {
                    var (opt, isCapture) = AskForSimpleValue<char>("Press R to try again or C to cancel current process...", false);
                    if (char.ToUpper(opt) == 'R' && isCapture)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Process cancelled...");

                        return;
                    }
                }
            }
        }

        public void MainMenu()
        {
            while (true)
            {
                ClearAndContinue();
                Console.WriteLine(@"
                    Welcome to Silicon Not Valley Bank!!

                    Select your desired option:
                        A) Create a new Account.
                        B) Deposit funds into an existing Account.
                        C) Withdraw funds from an existing Account.
                        D) View details from an existing Account.
                        E) Transfer funds between existing Accounts.
                        F) Exit application.
                    ");
                Console.Write("\n\nPlease chose your option [A-F]: ");
                var selectedOption = char.ToUpper(Console.ReadKey().KeyChar);

                if (!_validOptions.Contains(selectedOption))
                {
                    Console.WriteLine($"Invalid option selected: {selectedOption}; Must be between [A-F]");
                    continue;
                }

                Console.WriteLine("\n");
                switch (selectedOption)
                {
                    case 'A': CreateNewAccount(); break;
                    case 'B': DoDeposit(); break;
                    case 'C': DoWithdraw(); break;
                    case 'D': CheckAccountBalance(); break;
                    case 'E': DoTransfer(); break;
                    case 'F':
                        {
                            ClearAndContinue();

                            return;
                        };
                    default:
                        {
                            Console.WriteLine($"Invalid option selected: {selectedOption}; Must be between [A-F]");

                            continue;
                        };
                }
            }
        }

        private void ClearAndContinue()
        {
            Console.WriteLine("Please press a key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
