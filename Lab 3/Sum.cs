using System;

namespace Lab_solution.Lab_3
{
    abstract class Sum
    {
        public abstract int SumOfTwo(int a, int b);
        public abstract int SumOfThree(int a, int b, int c);
        
        
    }
    class Calcute : Sum 
    {
        public override int SumOfTwo(int a, int b)
        {
            return a + b;
        }
        public override int SumOfThree(int a, int b, int c)
        {
            return (a + b) + c;
        }
    }
}
