---
name: reqnroll-bdd-tester
description: A workflow skill dedicated to using Reqnroll (C# BDD framework) for writing and implementing behavior-driven scenarios. Use when asked to write tests, add BDD scenarios, or implement testing logic in C#.
---

# Reqnroll BDD Tester (C#)

This skill provides guidance for implementing Behavior-Driven Development (BDD) in the Slow-Living Compass project using **Reqnroll** (the successor to SpecFlow), perfectly integrating with the existing C# Blazor WebAssembly architecture.

## When to Use

1. Translating feature requirements into executable `.feature` specifications.
2. Generating C# step definitions to bind Gherkin steps to code.
3. Guiding the automated testing of Blazor components (via bUnit) or full workflows.

## Core Principles

- **Unified Stack**: Since the app is Blazor WASM, stick to C# for step definitions to allow seamless dependency injection and reuse of business logic.
- **Language**: Scenarios (Gherkin) should ideally be written in Traditional Chinese to match the project's domain ("慢活指南" - "尋找避風港"), though C# class names and methods should remain in English convention.
- **Testing Approach**: 
  - For UI interactions: Use `bUnit` to mount Blazor components.
  - For full flow testing: Use `Playwright for .NET`.

## Resources

- **`references/reqnroll_syntax.md`**: Guide to writing Gherkin files (`.feature`) and how Reqnroll maps them to C# Attributes (`[Binding]`, `[Given]`, `[When]`, `[Then]`).
- **`assets/StepDefinitionsTemplate.cs`**: A boilerplate C# file for you to start implementing new test steps.

## Instructions

1. Identify the new feature or behavior.
2. Create or update a `.feature` file using standard T-SQL-like Gherkin strings (Given, When, Then).
3. Use `assets/StepDefinitionsTemplate.cs` as a starting point to generate the corresponding `[Binding]` classes.
4. If testing Blazor UI, reference bUnit syntax to render the component and perform `.Click()` or `.MarkupMatches()`.
