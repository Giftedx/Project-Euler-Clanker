using Project_Euler.Interfaces;
using Project_Euler.Services;

namespace Project_Euler;

public static class ServiceContainer {
    private static readonly Dictionary<Type, object> _services = new();

    private static void RegisterSingleton<TInterface, TImplementation>(TImplementation instance)
        where TImplementation : class, TInterface {
        _services[typeof(TInterface)] = instance;
    }
    
    public static T GetService<T>() {
        if (_services.TryGetValue(typeof(T), out object? service)) {
            return (T)service;
        }
        throw new InvalidOperationException($"Service of type {typeof(T).Name} not registered");
    }
    
    public static void ConfigureServices() {
        var statisticsCalculator = new StatisticsCalculator();
        var outputHandler = new OutputHandler(statisticsCalculator);
        var problemSolver = new ProblemSolver(outputHandler, statisticsCalculator);
        
        RegisterSingleton<IStatisticsCalculator, StatisticsCalculator>(statisticsCalculator);
        RegisterSingleton<IOutputHandler, OutputHandler>(outputHandler);
        RegisterSingleton<IProblemSolver, ProblemSolver>(problemSolver);
    }
}