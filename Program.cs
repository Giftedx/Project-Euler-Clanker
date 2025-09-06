using Project_Euler.Interfaces;

namespace Project_Euler;

internal static class Program {
    private static readonly Dictionary<string, (string Description, Action Action)> 
        MenuActions = new(StringComparer.OrdinalIgnoreCase) {
            { "a", ("solve problems in range", () => ServiceContainer.GetService<IProblemSolver>().FullBenchmark()) },
            { "t", ("run test routine", () => ServiceContainer.GetService<IProblemSolver>().RunTest()) }
        };
    
    public static void Main() {
        Library.PrecomputePrimes(100000);
        ServiceContainer.ConfigureServices();
        RunInteractionLoop();
    }
    
    private static void RunInteractionLoop() {
        do {
            PrintMenu();
            string input = InputHandler.GetMenuSelection();
            HandleMenuSelection(input);
        } while (InputHandler.ShouldRunAgain());
    }
    
    private static void PrintMenu() {
        Console.Clear();
        Console.WriteLine("Project Euler Solver");
        Console.WriteLine();
        foreach ((string key, (string description, _)) in MenuActions)
            Library.FunPrint($"Enter '{key}' to {description}.");
        
        Library.FunPrint($"Enter individual Problem to solve (1 - " +
                         $"{ProblemFactory.SolvedProblems()}): ");
    }
    
    private static void HandleMenuSelection(string input) {
        if (MenuActions.TryGetValue(input, out var action)) action.Action.Invoke();
        else ServiceContainer.GetService<IProblemSolver>().IndividualBenchmark(input);
    }
}