using System;

namespace Lab_solution.Lab_3
{
    public class IndexOfRange
    {
        public IndexOfRange()
        {
            int[] numbers = new int[5];

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Write("Enter number " + (i + 1) + ": ");
                    numbers[i] = Convert.ToInt32(Console.ReadLine());
                }

                Console.WriteLine("Accessing 6th number: " + numbers[5]);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("IndexOutOfRangeException !");
            }
            
        }
    }
}