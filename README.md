# Todo App

A task management application built with ASP.NET Core Web API and a console client.

---

## Project Checklist

### Repository & Solution

- [x] GitHub repository created
- [x] `.gitignore` added
- [x] `.NET` solution file created
- [x] Web API project added to solution
- [x] Consuming project added to solution
- [x] Test project added to solution

### Web API

- [x] API builds successfully
- [x] CRUD endpoints implemented (Create, Read, Update, Delete)
- [x] Data stored in-memory and persisted in todos.json
- [x] Swagger enabled
- [x] LINQ used for filtering/sorting

### Consuming Project

- [x] Connects to Web API via `HttpClient`
- [x] Uses `async`/`await`
- [x] Supports all four CRUD operations

### Testing

- [x] Unit test project created
- [x] Tests build without errors
- [x] Tests pass
- [x] Significant execution paths covered (including edge cases)

### README

- [x] App name and purpose documented
- [x] Build and run instructions included
- [x] Reflection questions answered
  - [x] What did you learn from this project?
  - [x] What did you learn from this course?
  - [x] What would you have done differently or added with more time?

### AI Use (Code:You Policy)

- [x] All AI use cited per the Code:You AI policy

---

## App Purpose

This app is an online version of my paper notepad to keep track of the tasks I want to accomplish, allow them to either have a specified due date or to be completed in priority order when no due date is set.
I hope to integrate this app into my Web Dev capstone, which was a calendar and habit tracker website.

---

## How to Build and Run

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [VS Code](https://code.visualstudio.com/)

### Steps

1. Clone or download the repository from GitHub
2. Open VS Code, select **File → Open Folder**, navigate to the downloaded `code-you-csharp-capstone-todoapp` folder, and click **Select Folder**
3. Open a terminal in VS Code and navigate to the API project:

   ```
    a. cd TodoApp.Api
    b. dotnet run
   ```

4. Open a second terminal and run the tests from the solution root:

   ```
    a. dotnet test
   ```

5. Once tests pass, navigate to the console project and start it:

   ```
    a. cd TodoApp.Console
    b. dotnet run
   ```

6. The console menu will guide you through all available operations

> **Note:** The API must be running before starting the console app. Swagger is available at `http://localhost:1221/swagger` while the API is running.

---

## Reflection

### What did you learn from this project?

Building an API was not as hard as I expected going into this project — I always assumed they were
far more complex than they turned out to be. This project also reinforced that simple solutions can
still be effective at accomplishing what you set out to do.

### What did you learn from this course?

Many of the principles I learned here, like DRY, overlap with what I learned in the Web Dev track.
The difference is that these principles are now part of my thinking from the beginning of a project
rather than something I apply after the fact.

### If you had more time, what would you have done differently or added?

- I would have built a simple web interface in addition to the console app
- I would have spread the work out over a longer time frame instead of doing it all in the last two weeks

---

## AI Use Citations

- I fed the OpenClass text from the lessons that covered the final project, including the scoring rubric, to have AI generate the checklist and template to guide me as I wrote the project.

- Had AI create a sample json file with 6 default todo items.

- Needed AI to help debug an error with the second TodoController method, it had to be changed from

  from: `public TodoController(List<TodoItem> todos) `

  to: `internal TodoController(List<TodoItem> todos)`

- that fix also required that this code be added to `TodoApp.Api.csproj`

  ```xml
  <ItemGroup>
    <InternalsVisibleTo Include="TodoApp.Tests" />
  </ItemGroup>
  ```
