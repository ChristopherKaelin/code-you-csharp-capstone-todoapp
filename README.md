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
- [x] Data stored in-memory
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
- [ ] Build and run instructions included
- [ ] Reflection questions answered
  - [ ] What did you learn from this project?
  - [ ] What did you learn from this course?
  - [ ] What would you have done differently or added with more time?

### AI Use (Code:You Policy)

- [x] All AI use cited per the Code:You AI policy

---

## App Purpose

This app is an online version of my paper notepad to keep track of the tasks I want to accomplish, allow them to either have a specified due date or to be completed in priority order when no due date is set.
I hope to integrate this app into my Web Dev capstone, which was a calendar and habit tracker website.

---

## How to Build and Run

---

## Reflection

### What did you learn from this project?

### What did you learn from this course?

### If you had more time, what would you have done differently or added?

- I would have made a simple web interface
- I would have spread the work out over a longer time frame instead of doing all the work in the last two weeks.

---

## AI Use Citations

- I fed the OpenClass text from the lessons that covered the final project, including the scoring rubric, to have AI generate the checklist and template to guide me as I wrote the project.

- Had AI create a sample json file with 6 default todo items.

- Needed AI to help debug an error with the second TodoController method, it had to be changed from

  from: public TodoController(List<TodoItem> todos)

  to: internal TodoController(List<TodoItem> todos)

- Also required that this code be added to TodoApp.Api.csproj

  \<ItemGroup>

  \<InternalsVisibleTo Include="TodoApp.Tests" />

  \</ItemGroup>
