using System;


namespace Lab_solution.Lab_2
{
    public class Furniture
    {
        public string material;
        public double price;

        public void GetFurnitureDetails(string mat, double pr)
        {
            material = mat;
            price = pr;
        }

        public void ShowFurnitureDetails()
        {
            Console.WriteLine("Material: " + material);
            Console.WriteLine("Price: " + price);
        }
    }

    class Table : Furniture
    {
        public double height;
        public double surface_area;

        public void GetTableDetails(double h, double area)
        {
            height = h;
            surface_area = area;
        }

        public void ShowTableDetails()
        {
            Console.WriteLine("Height: " + height);
            Console.WriteLine("Surface Area: " + surface_area);
        }
    }
}
