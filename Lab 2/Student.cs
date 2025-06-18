using System;

namespace Lab_solution.Lab_2
{
    public class Student
    {
        public int Id;
        public string Name;
        public int Age;
        public double Weight;
        public double Height;

        public  Student()
        {
            Console.Write("Enter a value of Id :");
            Id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter a value of Name :");
            Name = Convert.ToString(Console.ReadLine());
            Console.Write("Enter a value of Age :");
            Age = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter a value of Weight :");
            Weight = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter a value of Height :");
            Height = Convert.ToDouble(Console.ReadLine());
        }
        public void DisplayDetails()
        {
            Console.WriteLine(Id);
            Console.WriteLine(Name);
            Console.WriteLine(Age);
            Console.WriteLine(Weight);
            Console.WriteLine(Height);

        }
    }
}
