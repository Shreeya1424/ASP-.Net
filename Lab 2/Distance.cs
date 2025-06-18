using System;


namespace Lab_solution.Lab_2
{
    public class Distance
    {
        int dist1;
        int dist2;
        int dist3;
        public Distance(int d1, int d2)
        {
            dist1 = d1;
            dist2 = d2;
        }

        public void AddDistances()
        {
            dist3 = dist1 + dist2;
        }

        public void Display()
        {
            Console.WriteLine("Addition of distances: " + dist3);
        }
    }
}
