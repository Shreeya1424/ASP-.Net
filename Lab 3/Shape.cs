using System;

interface Shape
{
    void Circle(double radius);
    void Triangle(double b, double h);
    void Square(double side);
}

class AreaCalculator : Shape
{
    public void Circle(double radius)
    {
        double area = Math.PI * radius * radius;
        Console.WriteLine("Area of Circle: " + area);
    }

    public void Triangle(double b, double h)
    {
        double area = 0.5 * b * h;
        Console.WriteLine("Area of Triangle: " + area);
    }

    public void Square(double side)
    {
        double area = side * side;
        Console.WriteLine("Area of Square: " + area);
    }
}
