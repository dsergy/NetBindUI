using System;
using NetBindUI.Contracts.Events;

namespace NetBindUI.Contracts.Interfaces.EventBus;

/// <summary>
/// Interface for an event bus implementation
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Subscribes to an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="handler">Event handler</param>
    void Subscribe<T>(Action<T> handler) where T : EventBase;

    /// <summary>
    /// Unsubscribes from an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="handler">Event handler</param>
    void Unsubscribe<T>(Action<T> handler) where T : EventBase;

    /// <summary>
    /// Publishes an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="event">Event to publish</param>
    void Publish<T>(T @event) where T : EventBase;
}