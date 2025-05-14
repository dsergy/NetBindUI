# NetBindUI Solution Structure

## Overview

NetBindUI is a modern Windows application that allows users to bind any program’s network traffic to a specific network interface. The solution is built on .NET 8.0 and follows clean architecture principles, ensuring modularity, testability, and maintainability.

---

## Solution Architecture

### Core Principles

- **Separation of concerns:** Each project has a clear, single responsibility.
- **Interface-based design:** All core logic is defined via interfaces in the Contracts project.
- **Asynchronous operations:** All service methods are asynchronous for responsive UI and scalability.
- **Event-driven architecture:** Services communicate via events for decoupling.
- **MVVM pattern:** The UI uses the Model-View-ViewModel pattern for maintainability and testability.

---

## Project Structure

NetBindUI/
├── NetBindUI.Contracts/ # Interfaces, models, and events (contracts)
├── NetBindUI.Core/ # Business logic and service implementations
├── NetBindUI.UI/ # WPF user interface
└── NetBindUI.Test/ # Unit tests (xUnit)


---

## Project Details

### NetBindUI.Contracts

Defines all interfaces, models, and event argument types used throughout the solution.  
**No implementation or platform-specific code.**

**Key folders:**
- `Interfaces/` — Service contracts (e.g., `INetworkService`, `IProcessService`, `IProfileService`, `IEventBus`)
- `Models/` — Data models (e.g., `INetworkInterface`, `IProcessInfo`, `IProfile`, `NetworkInterfaceStatistics`)
- `Events/` — Event argument types (e.g., `NetworkInterfaceChangedEventArgs`, `ProcessStateChangedEventArgs`)

---

### NetBindUI.Core

Implements the business logic and all service interfaces from Contracts.

**Key components:**
- `NetworkService` — Provides information and monitoring for network interfaces.
- `ProcessService` — Manages processes and their network bindings.
- `ProfileService` — Handles user profiles and configuration sets.
- `EventBus` — Centralized event management and dispatching.

**Depends on:**  
- `NetBindUI.Contracts`

---

### NetBindUI.UI

Implements the user interface using WPF and the MVVM pattern.

**Key components:**
- **Views:** Main window and dialogs for managing interfaces, processes, and profiles.
- **ViewModels:** Bind UI to services from Core, handle user commands, and manage state.
- **Controls:** Custom controls for displaying network/process information.

**Depends on:**  
- `NetBindUI.Core`  
- `NetBindUI.Contracts`

---

### NetBindUI.Test

Contains unit tests for all core services and business logic.

**Key components:**
- Tests for `NetworkService`, `ProcessService`, `ProfileService`, and `EventBus`
- Uses **xUnit** for modern, parallelizable testing

**Depends on:**  
- `NetBindUI.Core`  
- `NetBindUI.Contracts`

---

## Project Dependencies

NetBindUI.UI ──────► NetBindUI.Core ──────► NetBindUI.Contracts
▲
│
NetBindUI.Test ───────────┘


- **NetBindUI.UI** uses services from **Core** and contracts from **Contracts**
- **NetBindUI.Core** implements logic defined in **Contracts**
- **NetBindUI.Test** tests **Core** (and indirectly, **Contracts**)

---

## Build Order

1. NetBindUI.Contracts
2. NetBindUI.Core
3. NetBindUI.UI
4. NetBindUI.Test

---

## Technologies Used

- **.NET 8.0** — Modern, cross-platform runtime (Windows-specific for UI)
- **WPF** — Desktop UI framework (MVVM pattern)
- **xUnit** — Unit testing framework
- **C# 12** — Latest language features

---

## Coding Conventions

- **Naming:** PascalCase for types/members, camelCase for locals/parameters, English only
- **Documentation:** XML comments for all public APIs
- **Error Handling:** Consistent use of exceptions and error reporting
- **Async:** All I/O and long-running operations are asynchronous
- **Events:** Event-driven updates for UI and services

---

## Development Requirements

- **Visual Studio 2022** or newer
- **.NET 8.0 SDK**
- **Windows 10/11** (for UI and testing)

---

## Future Development

- Support for additional network interface types and advanced binding scenarios
- Enhanced process monitoring and diagnostics
- Import/export and sharing of user profiles
- Plugin system for extensibility
- Performance and UX improvements

---

## Contribution Guidelines

- All code, comments, and UI must be in English
- All communication in the team (issues, PRs, documentation) — in Russian
- Follow the structure and conventions described in this document

---

## Contacts

For questions, suggestions, or contributions, please contact the project maintainer.