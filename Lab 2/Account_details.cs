using System;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_solution.Lab_2
{
    public class Account_details
    {
        public string name;
        public int accountNumber;
        public double balance;
        public double rate;
        public int time;

        public void GetDetails()
        {
            Console.Write("Enter Name: ");
            name = Console.ReadLine();

            Console.Write("Enter Account Number: ");
            accountNumber = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Balance: ");
            balance = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Rate of Interest: ");
            rate = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Time (in years): ");
            time = Convert.ToInt32(Console.ReadLine());
        }
    }
    public class Interest : Account_details
    {
        public void CalculateInterest()
        {
            double interest = (balance * rate * time) / 100;
            Console.WriteLine($"\nTotal Interest: {interest}");
        }
    }
}
