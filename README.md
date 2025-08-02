# CatfactCollector

[![.NET Build, Package and Release](https://github.com/CodeSencor/CatfactCollector/actions/workflows/main.yml/badge.svg)](https://github.com/CodeSencor/CatfactCollector/actions/workflows/main.yml)
[![Latest Release](https://img.shields.io/github/v/release/CodeSencor/CatfactCollector)](https://github.com/CodeSencor/CatfactCollector/releases/latest)

A simple .NET console application to fetch and display interesting cat facts. This project serves as a demonstration of a cross-platform build and release pipeline using GitHub Actions.

## ✨ Features

- Fetches random cat facts from an online API.
- Cross-platform: Standalone executables for Windows, Linux, and macOS.
- Automated builds and releases are handled by GitHub Actions.

## 🚀 Getting Started

You can download the latest pre-compiled executables for your operating system from the [**Releases**](https://github.com/CodeSencor/CatfactCollector/releases) page.

### Usage

1.  Download the appropriate executable for your system (e.g., `CatfactCollector-win-x64.exe` for Windows).
2.  Open a terminal or command prompt.
3.  Run the executable.

**For Linux/macOS:**

You may need to make the file executable first:

```bash
chmod +x ./CatfactCollector-linux-x64
./CatfactCollector-linux-x64
```

**For Windows:**

```powershell
./CatfactCollector-win-x64.exe
```

### Command-line Arguments

You can customize the application's behavior using the following command-line arguments:

| Flag | Alias | Description |
|---|---|---|
| `--loglevel` | `-l` | Sets the logging level (e.g., `Information`, `Debug`, `Error`). |
| `--output` | `-o` | Specifies the path to the output file where cat facts will be saved. |
| `--endpoint` | `-e` | Sets the URL for the cat fact API. |
| `--interval`| `-i` | Sets the interval in seconds for fetching new cat facts. |

**Example:**

Run the application with a 10-second interval, debug logging, and save the facts to a custom file named `my-cat-facts.txt`.

```bash
# For Linux/macOS
./CatfactCollector-linux-x64 --interval 10 --output my-cat-facts.txt --loglevel Debug

# For Windows
./CatfactCollector-win-x64.exe --interval 10 --output my-cat-facts.txt --loglevel Debug
```

## 🛠️ Building from Source

If you prefer to build the project yourself, follow these steps.

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

### Instructions

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/your-username/your-repository.git
    cd CatfactCollector
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Run the application:**
    ```bash
    dotnet run --project CatfactCollector
    ```

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.
