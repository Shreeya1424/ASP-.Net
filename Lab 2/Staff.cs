using System;

namespace Lab_solution.Lab_2
{
    public class Staff
    {
        public string Name;
        public string Dept;
        public string Des;
        public int Exp;
        public decimal Salary;

        public void GetDetails()
        {
           
            Console.Write("Enter a value of Name :");
            Name = Convert.ToString(Console.ReadLine());
            Console.Write("Enter a value of Dept :");
            Dept = Convert.ToString(Console.ReadLine());
            Console.Write("Enter a value of Des :");
            Des = Convert.ToString(Console.ReadLine());
            Console.Write("Enter a value of Exp :");
            Exp = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter a value of Salary :");
            Salary = Convert.ToDecimal(Console.ReadLine());
        }
        public void DisplayDetails()
        {


            if (Dept == "hod") {
                Console.WriteLine(Name);
                Console.WriteLine(Salary);
            }
        }
    }
}
