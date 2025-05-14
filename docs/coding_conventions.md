# Coding Conventions

This document describes the coding conventions used in the NetBindUI project.  
Following these conventions will help ensure code readability, maintainability, and consistency.

## General Principles

*   **Readability:** Code should be easy to read and understand.
*   **Maintainability:** Code should be easy to modify and extend.
*   **Consistency:** Code should follow a consistent style throughout the project.
*   **Static Analysis:** Use Roslyn analyzers and StyleCop to enforce coding conventions.

## Naming

*   **Types (Classes, Interfaces, Structs, Enums):** PascalCase (e.g., `NetworkService`, `INetworkInterface`)
*   **Methods:** PascalCase (e.g., `GetNetworkInterfacesAsync`, `StartMonitoring`)
*   **Properties:** PascalCase (e.g., `BytesReceived`, `InterfaceId`)
*   **Local Variables:** camelCase (e.g., `networkService`, `interfaceId`)
*   **Parameters:** camelCase (e.g., `executablePath`, `arguments`)
*   **Fields:** \_camelCase (e.g., `_networkService`, `_isMonitoring`)
*   **Interfaces:** Prefix with `I` (e.g., `INetworkService`, `INetworkInterface`)
*   **Attributes:** Suffix with `Attribute` (e.g., `CustomAttribute`)
*   **Constants:** ALL_CAPS (e.g., `MAX_CONNECTIONS`)

## Formatting

*   **Indentation:** 4 spaces (no tabs)
*   **Braces:** K&R style (opening brace on the same line)
*   **Spacing:** Spaces around operators (e.g., `x = y + z`)
*   **Line Length:** Maximum 120 characters
*   **Blank Lines:** Use blank lines to separate logical blocks of code

## Comments

*   **XML Comments:** Use XML comments to document all public APIs.
    *   Describe parameters, return values, and exceptions.
    *   Provide a brief description of the purpose of the class, method, or property.
*   **Code Comments:** Use comments to explain complex logic or non-obvious code.

## Error Handling

*   **Try-Catch Blocks:** Use try-catch blocks to handle exceptions.
*   **Logging:** Log errors and exceptions.
*   **Error Codes/Exceptions:** Return error codes or throw exceptions depending on the situation.

## Asynchronous Programming

*   **Async/Await:** Use async/await for asynchronous operations.
*   **Async Suffix:** Add the suffix `Async` to asynchronous methods (e.g., `GetNetworkInterfacesAsync`).
*   **Exception Handling:** Handle exceptions in asynchronous methods.

## Other Conventions

*   **var:** Use `var` for local variables when the type is obvious.
*   **readonly:** Use `readonly` for fields that are initialized only once.
*   **const:** Use `const` for constants.
*   **?? and ?.:** Use `??` and `?.` operators to simplify code.
*   **nameof():** Use `nameof()` to get the name of a variable or property.

## Example

```csharp
public interface INetworkService
{
    /// <summary>
    /// Gets a list of all available network interfaces.
    /// </summary>
    /// <returns>A collection of network interfaces.</returns>
    Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesAsync();
}

public class NetworkService : INetworkService
{
    private readonly ILogger<NetworkService> _logger;

    public NetworkService(ILogger<NetworkService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<INetworkInterface>> GetNetworkInterfacesAsync()
    {
        try
        {
            var interfaces = await Task.Run(() => NetworkInterface.GetAllNetworkInterfaces());
            return interfaces.Select(i => new NetworkInterface(i));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting network interfaces");
            throw;
        }
    }
}