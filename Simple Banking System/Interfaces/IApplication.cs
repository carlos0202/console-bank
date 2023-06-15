namespace Simple_Banking_System.Interfaces;

public interface IApplication
{
    void MainMenu();
    (T?, bool) AskForSimpleValue<T>(string message, bool enableEscaping = true) where T : IConvertible;
    void CreateNewAccount();
    void CheckAccountBalance();
    void DoDeposit();
    void DoWithdraw();
    void DoTransfer();
}