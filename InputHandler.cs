namespace Project_Euler;

public static class InputHandler {
    private const string SolveAllCommand = "a";
    private const string RunTestsCommand = "t";

    public static string GetMenuSelection() {
        string input;

        while (true) {
            input = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();
            if (IsValidSelection(input)) break;
            Library.FunPrint("Invalid input. Please try again.");
        }

        return input;
    }

    public static bool ShouldRunAgain() {
        Library.FunPrint("Press any key to run program again, Space to exit.");
        var input = Console.ReadKey(true).Key;
        return input != ConsoleKey.Spacebar;
    }

    public static (int start, int end) GetBenchmarkRange() {
        int maxProblems = ProblemFactory.SolvedProblems();
        
        Library.FunPrint($"Enter start problem number (1-{maxProblems}): ");
        int start = GetValidProblemNumber(1, maxProblems);
        
        Library.FunPrint($"Enter end problem number ({start}-{maxProblems}): ");
        int end = GetValidProblemNumber(start, maxProblems);
        
        return (start, end);
    }

    private static int GetValidProblemNumber(int min, int max) {
        while (true) {
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            
            if (int.TryParse(input, out int number) && number >= min && number <= max) return number;
            
            Library.FunPrint($"Invalid input. Please enter a number between {min} and {max}: ");
        }
    }

    private static bool IsValidSelection(string input) {
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (int.TryParse(input, out int num)) return num > 0 && num <= ProblemFactory.SolvedProblems();
        return input.Equals(SolveAllCommand, StringComparison.OrdinalIgnoreCase) ||
               input.Equals(RunTestsCommand, StringComparison.OrdinalIgnoreCase);
    }
}