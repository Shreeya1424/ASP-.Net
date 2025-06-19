using System;


namespace Lab_solution.Lab_3
{
    class LongestWordFinder
    {
        public string FindLongestWord(string sentence)
        {
            string[] words = sentence.Split(' ');
            string longestWord = "";
            int maxLength = 0;

            foreach (string word in words)
            {
                if (word.Length > maxLength)
                {
                    maxLength = word.Length;
                    longestWord = word;
                }
            }

            return longestWord;
        }
    }
}
