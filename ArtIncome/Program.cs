using System;

namespace ArtIncome
{
    class Program
    {
        static void Main(string[] args)
        {
            const double minimumTurnOver = 11000, baseSalary = 3900;
            
            double income = 5500;

            const double maximumOn17000 = 0.06 * (17000 - minimumTurnOver);

            var commissionRate = income > baseSalary + maximumOn17000 ? 9.0 : 6.0;
            
            var total = (100*(income-baseSalary))/commissionRate+minimumTurnOver;

            Console.WriteLine($"${Math.Round(total)}");
        }
    }
}