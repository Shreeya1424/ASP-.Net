using System;

namespace Lab_solution.Lab_3
{
    class StringMethodsDemo
    {
        static void Main(string[] args)
        {
            string str = "  Hello, World!  ";

            Console.WriteLine("Original String: '" + str + "'");
            Console.WriteLine("Length: " + str.Length);

            Console.WriteLine("ToUpper: " + str.ToUpper());
            Console.WriteLine("ToLower: " + str.ToLower());

            Console.WriteLine("Trim: '" + str.Trim() + "'");
            Console.WriteLine("Substring (7,5): " + str.Substring(7, 5));

            Console.WriteLine("Contains 'World': " + str.Contains("World"));
            Console.WriteLine("IndexOf 'World': " + str.IndexOf("World"));

            Console.WriteLine("Replace 'World' with 'C#': " + str.Replace("World", "C#"));

            string str2 = "hello, world!";
            Console.WriteLine("Equals (case-sensitive): " + str.Equals(str2));
            Console.WriteLine("Equals (ignore case): " + str.Equals(str2, StringComparison.OrdinalIgnoreCase));

            string data = "apple,banana,grape";
            string[] fruits = data.Split(',');
            Console.WriteLine("Split:");
            foreach (string fruit in fruits)
            {
                Console.WriteLine("- " + fruit);
            }
        }
    }
}
