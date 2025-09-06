using System.Text;
using System.Text.Json;
using Project_Euler.Interfaces;
using Project_Euler.Models;

namespace Project_Euler.Services;

public class OutputHandler(IStatisticsCalculator statisticsCalculator) : IOutputHandler {
    public void GenerateFullReport(List<ProblemData> results, BenchmarkData testData) {
        var calculatedData = statisticsCalculator.CalculateBenchmarkData(results);
        
        WriteBenchmarkReport(results, calculatedData);
        WriteBenchmarkJson(results, calculatedData);
        WriteBenchmarkHtml(results, calculatedData);
    }
    
    public void WriteBenchmarkReport(List<ProblemData> results, BenchmarkData testData) {
        try {
            var fileContent = new StringBuilder();
            
            fileContent.AppendLine("Project Euler Benchmark Report");
            fileContent.AppendLine($"Generated: {testData.Timestamp:yyyy-MM-dd HH:mm:ss}");
            fileContent.AppendLine($"System: {testData.SystemInfo}");
            fileContent.AppendLine(new string('=', 50));
            fileContent.AppendLine();
            
            foreach (var result in results) {
                var stats = result.GetStatistics(statisticsCalculator);
                
                fileContent.AppendLine($"Problem {result.Index:D2}: {result.Result}");
                fileContent.AppendLine($"    Best:   {stats.Best:F3} ms");
                fileContent.AppendLine($"    Worst:  {stats.Worst:F3} ms");
                fileContent.AppendLine($"    Avg:    {stats.Average:F3} ms");
                fileContent.AppendLine($"    Median: {stats.Median:F3} ms");
                fileContent.AppendLine($"    StdDev: {stats.StandardDeviation:F3} ms");
                fileContent.AppendLine();
            }
            
            fileContent.AppendLine(new string('=', 50));
            fileContent.AppendLine($"Total Time: {testData.TotalTime:F3} ms");
            fileContent.AppendLine($"Average solution time: {testData.TotalTime / results.Count:F3} ms");
            fileContent.AppendLine($"Fastest Problem: {testData.FastestProblem} with {testData.FastestTime:F3} ms");
            fileContent.AppendLine($"Slowest Problem: {testData.SlowestProblem} with {testData.SlowestTime:F3} ms");
            
            File.WriteAllText(BenchmarkConfig.LogFile, fileContent.ToString());
        } catch (Exception ex) {
            throw new InvalidOperationException($"Failed to write benchmark report: {ex.Message}", ex);
        }
    }
    
    public void WriteBenchmarkJson(List<ProblemData> results, BenchmarkData testData) {
        try {
            var jsonOutput = new {
                metadata = new {
                    timestamp = testData.Timestamp,
                    systemInfo = testData.SystemInfo
                },
                summary = new {
                    totalProblems = results.Count,
                    totalTimeMs = testData.TotalTime,
                    averageTimeMs = testData.TotalTime / results.Count,
                    fastestProblem = new {
                        index = testData.FastestProblem,
                        timeMs = testData.FastestTime
                    },
                    slowestProblem = new {
                        index = testData.SlowestProblem,
                        timeMs = testData.SlowestTime
                    }
                },
                problems = results.Select(r => {
                    var stats = r.GetStatistics(statisticsCalculator);
                    return new {
                        index = r.Index,
                        result = r.Result,
                        statistics = new {
                            bestTimeMs = stats.Best,
                            worstTimeMs = stats.Worst,
                            averageTimeMs = stats.Average,
                            medianTimeMs = stats.Median,
                            standardDeviationMs = stats.StandardDeviation,
                            runs = r.Times.Count
                        }
                    };
                })
            };
            
            string json = JsonSerializer.Serialize(jsonOutput, BenchmarkConfig.JsonOptions);
            File.WriteAllText(BenchmarkConfig.JsonFile, json);
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to write JSON benchmark: {ex.Message}", ex);
        }
    }
    
    public void WriteBenchmarkHtml(List<ProblemData> results, BenchmarkData testData) {
        try {
            var problems = results.Select(r => {
                var stats = r.GetStatistics(statisticsCalculator);
                return new {
                    index = r.Index,
                    result = r.Result,
                    bestTimeMs = stats.Best,
                    worstTimeMs = stats.Worst,
                    averageTimeMs = stats.Average,
                    medianTimeMs = stats.Median,
                    standardDeviationMs = stats.StandardDeviation
                };
            }).ToList();
            
            var summary = new {
                totalProblems = results.Count,
                totalTimeMs = testData.TotalTime,
                averageTimeMs = testData.TotalTime / results.Count,
                fastestProblem = new {
                    index = testData.FastestProblem,
                    timeMs = testData.FastestTime
                },
                slowestProblem = new {
                    index = testData.SlowestProblem,
                    timeMs = testData.SlowestTime
                },
                timestamp = testData.Timestamp,
                systemInfo = testData.SystemInfo
            };
            
            string jsonData = JsonSerializer.Serialize(new { summary, problems });
            
            if (!File.Exists(BenchmarkConfig.HtmlTemplate))
                throw new FileNotFoundException($"HTML template not found: {BenchmarkConfig.HtmlTemplate}");
            
            string htmlTemplate = File.ReadAllText(BenchmarkConfig.HtmlTemplate);
            string finalHtml = htmlTemplate.Replace("{{DATA}}", jsonData);
            
            File.WriteAllText(BenchmarkConfig.HtmlFile, finalHtml);
        } catch (Exception ex) {
            throw new InvalidOperationException($"Failed to write HTML benchmark: {ex.Message}", ex);
        }
    }
}
