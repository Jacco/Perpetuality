using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perpetuality.Utilities
{
    public class ReadablePassword
    {
        //Create a static instance of the Random class, on which to operate
        private static Random _randomInstance = new Random();

        public static string GenerateReadablePassword()
        {
            //simple short words
            string[] words = new string[6] {
        "solar", "energy", "earth", "volt", "ampere", "etc"
      };
            //alphabets: don't include those that can be misinterpreted for another letter/number
            string alphabets = "abcdefghjkmnpqrstuvwxyz";
            //numbers: exclude 0 (~= O or o), 1 (~= I, or l) and 2 (~= z)
            string numbers = "3456789";
            //common symbols: no brackets, parentheses or slashes
            string symbols = "!@#+=&*?";

            //put the elements together in a random format, but don't stick an alphabet immediately after a word
            string[] formats = new string[12] {
        "{1}{2}{3}{0}", "{1}{2}{0}{3}", "{3}{0}{2}{1}",
        "{0}{2}{3}{1}", "{1}{3}{0}{2}", "{3}{1}{2}{0}",
        "{0}{2}{1}{3}", "{1}{3}{2}{0}", "{2}{0}{3}{1}",
        "{0}{3}{1}{2}", "{2}{1}{3}{0}", "{0}{3}{2}{1}"
        };

            //combine the elements
            string password = string.Format(GetRandomString(formats), //random format
              GetRandomString(words), //0
              GetRandomStringCharacter(alphabets), //1
              GetRandomStringCharacter(numbers), //2
              GetRandomStringCharacter(symbols) //3
            );

            return password;
        }

        private static char GetRandomStringCharacter(string inputString)
        {
            return inputString[_randomInstance.Next(0, inputString.Length)];
        }

        private static string GetRandomString(string[] inputStringArray)
        {
            return inputStringArray[_randomInstance.Next(0, inputStringArray.Length)];
        }
    }
}