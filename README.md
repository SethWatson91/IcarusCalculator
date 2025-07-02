# Icarus Item Calculator – Crafting Dependency Analyzer

A web-based tool built with ASP.NET Core MVC that analyzes and resolves nested crafting dependencies from the survival game **Icarus**. This app allows players to calculate total base resources required to craft complex items by tracing recursive recipes across multiple tiers.

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

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/icarus-crafting-analyzer.git
    cd icarus-crafting-analyzer
    dotnet ef database update
    dotnet run
    ```

---

## 🎮 Background

The survival game *Icarus* features a deep and multi-tiered crafting system. This project started as a passion tool to help players understand how many raw resources (like wood, ore, etc.) are needed to craft high-tier items like a Radar or Orbital Workbench. It evolved into a recursive dependency solver with reusable architecture.

---

## 📁 Project Structure

```text
├── Controllers/
├── Models/
├── ViewModels/
├── Views/
├── Data/
├── appsettings.json
└── Program.cs
```
