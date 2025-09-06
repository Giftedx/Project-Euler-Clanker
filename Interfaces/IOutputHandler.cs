using Project_Euler.Models;

namespace Project_Euler.Interfaces;

public interface IOutputHandler {
    void GenerateFullReport(List<ProblemData> results, BenchmarkData testData);
    void WriteBenchmarkReport(List<ProblemData> results, BenchmarkData testData);
    void WriteBenchmarkJson(List<ProblemData> results, BenchmarkData testData);
    void WriteBenchmarkHtml(List<ProblemData> results, BenchmarkData testData);
}