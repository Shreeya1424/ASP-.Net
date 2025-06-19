using System;


namespace Lab_solution.Lab_4
{
     class BankAccount
    {
        private double balance;
        private string accountHolderName;

        public BankAccount(double initialBalance, string accountHolderName)
        {
            this.balance = initialBalance;
            this.accountHolderName = accountHolderName;
            Console.WriteLine($"Account created for {accountHolderName} with initial balance: {balance}");
        }

       
        public void Deposit(double cashAmount)
        {
            balance += cashAmount;
            Console.WriteLine($"Deposited cash: {cashAmount}. Current balance: {balance}");
        }

        public void Deposit(string chequeNumber, double chequeAmount)
        {
            Console.WriteLine($"Cheque No. {chequeNumber} deposited.");
            balance += chequeAmount;
            Console.WriteLine($"Deposited by cheque: {chequeAmount}. Current balance: {balance}");
        }

        public void Withdraw(double cashAmount)
        {
            if (balance >= cashAmount)
            {
                balance -= cashAmount;
                Console.WriteLine($"Withdrawn cash: {cashAmount}. Remaining balance: {balance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance for cash withdrawal.");
            }
        }

        public void Withdraw(string chequeNumber, double chequeAmount)
        {
            Console.WriteLine($"Processing cheque No. {chequeNumber} for withdrawal...");
            if (balance >= chequeAmount)
            {
                balance -= chequeAmount;
                Console.WriteLine($"Withdrawn by cheque: {chequeAmount}. Remaining balance: {balance}");
            }
            else
            {
                Console.WriteLine("Insufficient balance for cheque withdrawal.");
            }
        }
    }
}
