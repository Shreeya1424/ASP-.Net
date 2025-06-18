using System;


namespace Lab_solution.Lab_2
{
    public class Rectangle
    {
        double l;
        double w;

        public Rectangle()
        {
            Console.Write("Enter a value of length :");
            l = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter a value of Width :");
            w = Convert.ToDouble(Console.ReadLine());
        }
        public void Display()
        {
            Console.WriteLine("area of rectangle is : "+(l*w));
        }
    }
}
