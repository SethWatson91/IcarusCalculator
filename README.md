# Icarus Item Calculator – Crafting Dependency Analyzer

A web-based tool built with ASP.NET Core MVC that analyzes and resolves nested crafting dependencies from the survival game **Icarus**. This app allows players to calculate total base resources required to craft complex items by tracing recursive recipes across multiple tiers.

![screenshot-placeholder](https://via.placeholder.com/800x400?text=Icarus+Calculator+UI)

---

## 🔧 Tech Stack

- ASP.NET Core MVC (C#)
- Entity Framework Core (SQL Server)
- Razor Views & ViewModels
- Recursive algorithms for dependency resolution
- Git / GitHub for version control

---

## 🧠 Features

- 🧮 **Crafting Chain Resolution** – Calculates all base ingredients for a selected item across all nested levels.
- 🔁 **Recursive Algorithm** – Dynamically computes resources even for deeply nested recipes.
- 🗂️ **Relational Recipe Database** – Models complex item relationships using EF Core.
- 🧑‍💻 **Clean UI with ViewModels** – Presents crafting results in a readable, flattened format.
- 🧪 **Test Items & Recipes** – Pre-populated with example recipes for demonstration and testing.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server Express or LocalDB
- Visual Studio 2022+ or VS Code

### Running the Project

1. Clone the repository:

```bash
git clone https://github.com/SethWatson91/IcarusCalculator.git
cd IcarusCalculator
dotnet ef database update
dotnet run
