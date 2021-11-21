using System;
using System.Collections.Generic;
using System.Linq;

namespace PolybiusPlayfair
{
    class Program
    {
        static void Main(string[] args)
        {
            const string key = "ADA LOVELACE";
            const string cipherText = "LVFPPNMQDOORAYJVKHPGPBTPKHNZFOAY?";

            var square = GeneratePolybiusSquare(key);

            var message = Decrypt(cipherText, square);

            Console.WriteLine(message);
        }

        static char[][] GeneratePolybiusSquare(string key)
        {
            var polybiusSquare = new char[5][];
            for (var i = 0; i < 5; i++) polybiusSquare[i] = new char[5];
            var alphabets = new List<char>();

            for (var letter = 'A'; letter <= 'Z'; letter++)
                if (letter != 'J')
                    alphabets.Add(letter);

            var distinctLetters = new string(key.ToUpper().Where(k => alphabets.Contains(k)).Distinct().ToArray());

            var tableLetters = distinctLetters;
            var alphaIndex = 0;

            while (tableLetters.Length < 25)
            {
                if (!tableLetters.Contains(alphabets[alphaIndex])) tableLetters += alphabets[alphaIndex];
                alphaIndex++;
            }

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    polybiusSquare[i][j] = tableLetters[i * 5 + j];
                }
            }

            return polybiusSquare;
        }

        static string Decrypt(string cipherText, char[][] polybiusSquare)
        {
            var specialCharacter = '?';
            var hasSpecialCharacter = false;
            var plainText = "";
            var textToDecrypt = cipherText.ToUpper().Replace("J", "I");

            if (cipherText.Length % 2 != 0)
            {
                hasSpecialCharacter = true;
                specialCharacter = cipherText[cipherText.Length - 1];
                textToDecrypt = textToDecrypt.Substring(0, cipherText.Length - 1);
            }


            for (var i = 0; i < textToDecrypt.Length; i += 2)
            {
                var firstCharPosition = GetPostion(polybiusSquare, textToDecrypt[i]);
                var secondCharPosition = GetPostion(polybiusSquare, textToDecrypt[i + 1]);

                //If the first character is in the same row as the second character
                if (firstCharPosition[0] == secondCharPosition[0])
                {
                    plainText +=
                        polybiusSquare[firstCharPosition[0]][firstCharPosition[1] == 0 ? 4 : firstCharPosition[1] - 1];
                    plainText +=
                        polybiusSquare[secondCharPosition[0]][
                            secondCharPosition[1] == 0 ? 4 : secondCharPosition[1] - 1];
                }
                //If the first character is in the same column as the second character
                else if (firstCharPosition[1] == secondCharPosition[1])
                {
                    plainText +=
                        polybiusSquare[firstCharPosition[0] == 0 ? 4 : firstCharPosition[0] - 1][firstCharPosition[1]];
                    plainText +=
                        polybiusSquare[secondCharPosition[0] == 0 ? 4 : secondCharPosition[0] - 1][
                            secondCharPosition[1]];
                }
                //If the first character is in the same box as the second character
                else
                {
                    plainText += polybiusSquare[firstCharPosition[0]][secondCharPosition[1]];
                    plainText += polybiusSquare[secondCharPosition[0]][firstCharPosition[1]];
                }
            }

            return hasSpecialCharacter ? plainText + specialCharacter : plainText;
        }

        static int[] GetPostion(char[][] polybiusSquare, char character)
        {
            var position = new int[2];

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (polybiusSquare[i][j] == character)
                    {
                        position[0] = i;
                        position[1] = j;
                    }
                }
            }

            return position;
        }
    }
}