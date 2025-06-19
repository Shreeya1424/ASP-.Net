using System;
namespace Lab_solution.Lab_3
{
    public class ZerodivitionError
    {
        public ZerodivitionError()
        {
            int N = 5;
            int div = 0;
            try
            {
                div = 100 / N;
            }
            catch(DivideByZeroException) 
            { 
            Console.WriteLine("Exception Occured");
            }

            Console.WriteLine(div);
        }
    }
}
