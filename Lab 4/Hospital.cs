using System;


namespace Lab_solution.Lab_4
{
    class Hospital
    {
        public virtual void HospitalDetails()
        {
            Console.WriteLine("General Hospital Details");
        }
    }

    class Apollo : Hospital
    {
        public override void HospitalDetails()
        {
            Console.WriteLine("Apollo Hospital.");
        }
    }
    class Wockhardt : Hospital
    {
        public override void HospitalDetails()
        {
            Console.WriteLine("Wockhardt Hospital.");
        }
    }

    class Gokul_Superspeciality : Hospital
    {
        public override void HospitalDetails()
        {
            Console.WriteLine("Gokul Superspeciality.");
        }
    }
}
