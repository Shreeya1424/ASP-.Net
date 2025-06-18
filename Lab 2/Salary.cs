using System;


namespace Lab_solution.Lab_2
{
    class Salary
    {
       
        public double Basic;
        public double TA;
        public double DA;
        public double HRA;

        public Salary(double basic, double ta, double da = 2000, double hra = 3000)
        {
            Basic = basic;
            TA = ta;
            DA = da;
            HRA = hra;
        }

        public void CalculateSalary()
        {
            double total = Basic + TA + DA + HRA;
            Console.WriteLine("Total Salary: " + total);
        }
    }
}

