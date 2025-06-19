using System;

namespace Lab_solution.Lab_3
{
    class CaseChanger
    {
        public char ChangeCase(char ch)
        {
            if (char.IsUpper(ch))
            {
                return char.ToLower(ch);
            }
            else if (char.IsLower(ch))
            {
                return char.ToUpper(ch);
            }
            else
            {
                return ch;
            }
        }
    }
}
