using System;
using System.Collections.Concurrent;
using NetBindUI.Contracts.Events;
using NetBindUI.Contracts.Interfaces.EventBus;

namespace NetBindUI.Core.Services;

/// <summary>
/// Implementation of the IEventBus interface
/// </summary>
public class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Action<EventBase>>> _handlers = new();

    /// <summary>
    /// Subscribes to an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="handler">Event handler</param>
    public void Subscribe<T>(Action<T> handler) where T : EventBase
    {
        if (!_handlers.ContainsKey(typeof(T)))
        {
            _handlers[typeof(T)] = new ConcurrentBag<Action<EventBase>>();
        }

        _handlers[typeof(T)].Add(e => handler((T)e));
    }

    /// <summary>
    /// Unsubscribes from an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="handler">Event handler</param>
    public void Unsubscribe<T>(Action<T> handler) where T : EventBase
    {
        if (_handlers.TryGetValue(typeof(T), out var handlers))
        {
            // This is not the most efficient way to unsubscribe, but it's thread-safe
            var newHandlers = new ConcurrentBag<Action<EventBase>>();
            foreach (var h in handlers)
            {
                if (h.Target != handler.Target || h.Method != handler.Method)
                {
                    newHandlers.Add(h);
                }
            }
            _handlers[typeof(T)] = newHandlers;
        }
    }

    /// <summary>
    /// Publishes an event of type T
    /// </summary>
    /// <typeparam name="T">Type of the event, must inherit from EventBase</typeparam>
    /// <param name="event">Event to publish</param>
    public void Publish<T>(T @event) where T : EventBase
    {
        if (_handlers.TryGetValue(typeof(T), out var handlers))
        {
            foreach (var handler in handlers)
            {
                handler(@event);
            }
        }
    }
}