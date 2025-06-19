using System;

    namespace Lab_solution.Lab_3
    {
        interface Calculate
        {
            int Addition(int a, int b);
            int Subtraction(int a, int b);
        }

       
        class Result : Calculate
        {
            public int Addition(int a, int b)
            {
                return a + b;
            }

            public int Subtraction(int a, int b)
            {
                return a - b;
            }
        }
    }
