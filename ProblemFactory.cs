using System.Diagnostics;
using Project_Euler.Problems;
using static System.Reflection.Assembly;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace Project_Euler;

public static class ProblemFactory {
    private static readonly Dictionary<int, Type> ProblemTypes;

    static ProblemFactory() =>
        ProblemTypes = GetExecutingAssembly().GetTypes()
                                             .Where(t => typeof(Problem).IsAssignableFrom(t) && !t.IsAbstract)
                                             .Select(t => new {
                                                 Type = t,
                                                 Id = ExtractProblemId(t.Name)
                                             })
                                             .Where(x => x.Id.HasValue)
                                             .ToDictionary(x => {
                                                 Debug.Assert(x.Id != null);
                                                 return x.Id.Value;
                                             }, x => x.Type);

    public static Problem CreateProblem(int id) {
        if (ProblemTypes.TryGetValue(id, out var type))
            return (Problem?)Activator.CreateInstance(type) ??
                   throw new InvalidOperationException($"Could not create instance of Problem{id:D3}");

        throw new ArgumentOutOfRangeException(nameof(id), $"Problem with ID {id} not found.");
    }

    public static int SolvedProblems() => ProblemTypes.Count;

    private static int? ExtractProblemId(string typeName) {
        if (typeName.StartsWith("Problem") && int.TryParse(typeName.AsSpan(7), out int id)) return id;

        return null;
    }
}