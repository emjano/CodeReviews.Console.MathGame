using System.Buffers;

namespace MathGame
{
    public class MathGameLogic
    {
        public List<string> GameHistory { get; set; } = new List<string>();

        public void ShowMenu()
        {
            Console.WriteLine("Please enter the operation you want to practice:");
            Console.WriteLine("1. Addition");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Random Mode");
            Console.WriteLine("6. Select Difficulty");
            Console.WriteLine("7. View Game History");
            Console.WriteLine("8. Exit");

        }

        public int MathOperation(int num1, int num2, string operation)
        {
            switch (operation)
            {
                case "+":
                    GameHistory.Add($"{num1} + {num2} = {num1 + num2}");
                    return num1 + num2;

                case "-":
                    GameHistory.Add($"{num1} - {num2} = {num1 - num2}");
                    return num1 - num2;

                case "x":
                    GameHistory.Add($"{num1} x {num2} = {num1 * num2}");
                    return num1 * num2;

                case "/":
                    while (num1 < 0 || num1 > 100)
                    {
                        try
                        {
                            Console.WriteLine("Please enter a number between 0 and 100 for the dividend:");
                            num1 = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (System.Exception)
                        {
                            // do nothing, just loop again
                        }
                    }
                    GameHistory.Add($"{num1} / {num2} = {num1 / num2}");
                    return num1 / num2;
            }
            return 0;
        }
    }
}