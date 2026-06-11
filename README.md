<div align="center">

# Project-Euler-Clanker

**Project Euler once again, but this time in C#. Forked and ruined by a Clanker.**

![C#](https://img.shields.io/badge/C%23-512BD4?style=flat&logo=dotnet&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-console-512BD4?style=flat)
![Over-engineered](https://img.shields.io/badge/architecture-too_much_for_sums-b88f2e?style=flat)

</div>

A fork of a perfectly innocent set of [Project Euler](https://projecteuler.net/) solutions that an AI agent ("the Clanker") got its hands on. It now has a service container, a problem factory, interfaces, benchmark configuration, and input handlers. For maths puzzles.

## What's in here

| Path | What |
|---|---|
| `Problems/` | The actual Euler solutions |
| `Services/`, `Interfaces/`, `Models/` | The Clanker's enterprise ambitions |
| `ProblemFactory.cs`, `ServiceContainer.cs` | Yes, really |
| `BenchmarkConfig.cs` | Because solving sums fast wasn't enough — it must be *measured* |
| `data/` | Problem inputs |

## Run it

```bash
dotnet run --project "Project Euler.csproj"
```

Pick a problem, watch it solve, enjoy the dependency injection that brought you the answer to "what is the 10001st prime".
