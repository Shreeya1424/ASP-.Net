using System;

namespace Lab_solution.Lab_3
{
    public class EvenNumber
    {
        public EvenNumber() 
        {
        Console.Write("Enter a value of N : ");
        int N = Convert.ToInt32(Console.ReadLine());
            try
            {
                if (N % 2 != 0)
                {
                    Console.WriteLine("Exception occured");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("No is even");
            }
            Console.WriteLine(N);

        }
    }
}
