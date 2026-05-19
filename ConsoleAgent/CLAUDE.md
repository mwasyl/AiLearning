# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```powershell
# Build
dotnet build

# Run with default provider (OpenAI, gpt-4.1-mini)
dotnet run

# Run with a specific provider and model
dotnet run -- --provider openai --model gpt-4.1-mini
dotnet run -- --provider gemini --model gemini-2.0-flash
```

## Environment

API keys are loaded from `.env` at startup via `dotenv.net`. The `.env` file is not in `.gitignore` — do not commit it. Required keys:

- `OPENAI_API_KEY` — OpenAI provider
- `GEMINI_API_KEY` — Gemini provider
- `ANTHROPIC_API_KEY` — Claude provider (currently commented out in `Startup.cs`)
- `WEATHER_API_DOTCOM_KEY` — weatherapi.com key for `WeatherService`

## Architecture

The app is a multi-provider AI chat loop built on the `Microsoft.Extensions.AI` abstraction, so the chat client (`IChatClient`) is provider-agnostic.

**Startup flow:** `Program.cs` parses `--provider` / `--model` args → `Startup.ConfigureServices` registers the correct `IChatClient` implementation and all tool services → `ChatAgent.RunAsync` runs the REPL loop.

**Provider selection (`Startup.cs`):** A switch on the `provider` string constructs either an OpenAI or Gemini client, then wraps it with `ChatClientBuilder` to add logging and automatic function invocation middleware. Claude/Anthropic support is present but commented out.

**Tool system (`FunctionRegistry.cs`):** Tools are registered as `AITool` instances via `AIFunctionFactory.Create`, which reflects over service methods. All tools are injected into `ChatOptions.Tools` and executed automatically by the `UseFunctionInvocation` middleware — no manual dispatch needed. To add a new tool: create a service, register it in `Startup.cs`, and yield a new `AIFunctionFactory.Create(...)` entry in `FunctionRegistry.GetTools`.

**Services (`Services/`):**
- `WeatherService` — calls weatherapi.com; has intentional error for "London" to demonstrate tool-retry behavior
- `WardrobeService` — returns a hardcoded clothing list
- `EmailService` — simulates sending email by printing to console in yellow
