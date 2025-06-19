using System;


namespace Lab_solution.Lab_4
{
    public class RBI
    {
        public virtual void calculateInterest(int p,int r,int t)
        {
            Console.WriteLine("intrest is : ",+((p*r*t)/100));
        }
    }
    public class HDFC : RBI 
    {
        public override void calculateInterest(int p, int r, int t)
        {
            Console.WriteLine("intrest is : ", +((p * r * t) / 100));
        }
    }
    public class SBI : RBI 
    {
        public override void calculateInterest(int p, int r, int t)
        {
            Console.WriteLine("intrest is : ", +((p * r * t) / 100));
        }
    }
    public class ICICI : RBI
    {
        public override void calculateInterest(int p, int r, int t)
        {
            Console.WriteLine("intrest is : ", +((p * r * t) / 100));
        }
    }
}
