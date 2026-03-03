/*

You need to create a game that consists of asking the player what's the result of a math question (i.e. 9 x 9 = ?), collecting the input and adding a point in case of a correct answer.

A game needs to have at least 5 questions.

The divisions should result on INTEGERS ONLY and dividends should go from 0 to 100. Example: Your app shouldn't present the division 7/2 to the user, since it doesn't result in an integer.

Users should be presented with a menu to choose an operation

You should record previous games in a List and there should be an option in the menu for the user to visualize a history of previous games.

You don't need to record results on a database. Once the program is closed the results will be deleted.

*/

using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using MathGame;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int num1;
int num2;
string operation;
int userMenuSelection;
int score = 0;
bool gameOver = false;

DifficultyLevel difficultyLevel = DifficultyLevel.Easy;
while(!gameOver)
{
    userMenuSelection = GetUserMenuSelection(mathGame);

    num1 = random.Next(1, 101);
    num2 = random.Next(1, 101); // Avoid division by zero

    switch (userMenuSelection)
    {
        case 1:
            operation = "+";
            score = await PerformOperation(mathGame, num1, num2, operation, difficultyLevel, score);
            break;
        case 2:
            operation = "-";
            score = await PerformOperation(mathGame, num1, num2, operation, difficultyLevel, score);
            break;
        case 3:
            operation = "x";
            score = await PerformOperation(mathGame, num1, num2, operation, difficultyLevel, score);
            break;
        case 4:
            operation = "/";
            score = await PerformOperation(mathGame, num1, num2, operation, difficultyLevel, score);
            break;
        case 5:
            int randomOperation = random.Next(1, 5);
            switch (randomOperation)
            {
                case 1:
                    operation = "+";
                    break;
                case 2:
                    operation = "-";
                    break;
                case 3:
                    operation = "x";
                    break;
                case 4:
                    operation = "/";
                    break;
                default:
                    operation = "+";
                    break;
            }
            score = await PerformOperation(mathGame, num1, num2, operation, difficultyLevel, score);
            break;
        case 6:
            difficultyLevel = ChangeDifficulty();
            Console.WriteLine($"Difficulty level set to: {difficultyLevel}");
            break;
        case 7:
            Console.WriteLine("Game History:");
            foreach (string history in mathGame.GameHistory)
            {
                Console.WriteLine(history);
            }
            Console.WriteLine($"Current Score: {score}");
            break;
        case 8:
            gameOver = true;
            Console.WriteLine($"Thanks for playing! Your final score was: {score}");
            break;
    }
}

static DifficultyLevel ChangeDifficulty()
{
    int userSelection = 0;
    while (userSelection < 1 || userSelection > 3)
    {
        Console.WriteLine("Please select a difficulty level:");
        Console.WriteLine("1. Easy");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. Hard");

        try
        {
            userSelection = Convert.ToInt32(Console.ReadLine());
        }
        catch (System.Exception)
        {
            // do nothing, just loop again
        }
    }
    switch (userSelection)
    {
        case 1:
            return DifficultyLevel.Easy;
        case 2:
            return DifficultyLevel.Medium;
        case 3:
            return DifficultyLevel.Hard;
        default:
            return DifficultyLevel.Easy; // Default to Easy if something goes wrong
    }
}

static void DisplayMathGameQuesiton(int num1, int num2, string operation)
{
    Console.WriteLine($"{num1} {operation} {num2} = ??");
}

static int GetUserMenuSelection(MathGameLogic mathGame)
{
    int selection = -1;
    mathGame.ShowMenu();
    while (selection < 1 || selection > 8)
    {
        while(!int.TryParse(Console.ReadLine(), out selection))
        {
            Console.WriteLine("Please enter a valid number between 1 and 8.");
        }

        if(!(selection >= 1 && selection <= 8))
        {
            Console.WriteLine("Please enter a valid number between 1 and 8.");
        }
    }

    return selection;
}

static async Task<int?> GetUserResponse(DifficultyLevel difficulty)
{
    int response = 0;
    int timeout = (int)difficulty;

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Task<string?> getUserInputTask = Task.Run(() => Console.ReadLine());

    try
    {
        string? result = await Task.WhenAny(getUserInputTask, Task.Delay(timeout * 1000)) == getUserInputTask ? getUserInputTask.Result : null;

        stopwatch.Stop();

        if (result != null && int.TryParse(result, out response))
        {
            Console.WriteLine($"You answered in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
            return response;
        }
        else
        {
            throw new OperationCanceledException();
        }
    }
        catch (OperationCanceledException)
        {
            Console.WriteLine("You took too long to answer.");
            return null;
        }
}

static int ValidateResult(int result, int? userResponse, int score)
{
    if (result == userResponse)
    {
        Console.WriteLine("Correct! 1 point added.");
        return score += 1;
    }
    else
    {
        Console.WriteLine("Incorrect.");
        Console.WriteLine($"The correct answer was: {result}");
    }
    return score;
}

static async Task<int> PerformOperation(MathGameLogic mathGame, int num1, int num2, string operation, DifficultyLevel difficulty, int score)
{
    int result;
    int? userResponse;
    DisplayMathGameQuesiton(num1, num2, operation);
    result = mathGame.MathOperation(num1, num2, operation);
    userResponse = await GetUserResponse(difficulty);
    score += ValidateResult(result, userResponse, score);
    return score;
}
public enum DifficultyLevel
{
    Easy = 45,
    Medium = 30,
    Hard = 15
}

