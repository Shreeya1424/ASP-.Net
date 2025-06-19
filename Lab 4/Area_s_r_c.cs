using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_solution.Lab_4
{
    public class Area_s_r_c
    {
        public void Area(double side)
        {
            double result = side * side;
            Console.WriteLine("Area of Square: " + result);
        }

        public void Area(double length, double breadth)
        {
            double result = length * breadth;
            Console.WriteLine("Area of Rectangle: " + result);
        }

        public void Area(float radius)
        {
            double result = Math.PI * radius * radius;
            Console.WriteLine("Area of Circle: " + result);
        }
    }
}
