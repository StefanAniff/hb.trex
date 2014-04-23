using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebus;

namespace Trex.Server.Infrastructure.Implemented
{
    /// <summary>
    /// Decorator for <see cref="IBus"/> that delays all operations until the end of the current unit of work
    /// </summary>
    public class DelayBusOperationsDecorator : IBus, IAdvancedBus
    {
        readonly IBus rebusBus;

        [ThreadStatic]
        static IList<Action> stuffToDo;

        static IList<Action> StuffToDo
        {
            get { return stuffToDo ?? (stuffToDo = new List<Action>()); }
        }

        public static void Commit()
        {
            foreach (var action in StuffToDo)
            {
                action();
            }

            stuffToDo = null;
        }

        public static void Abort()
        {
            stuffToDo = null;
        }

        public DelayBusOperationsDecorator(IBus rebusBus)
        {
            this.rebusBus = rebusBus;
        }

        public void Dispose()
        {
            rebusBus.Dispose();
        }

        public void Send<TCommand>(TCommand message)
        {
            StuffToDo.Add(() => rebusBus.Send(message));
        }

        public void SendLocal<TCommand>(TCommand message)
        {
            StuffToDo.Add(() => rebusBus.SendLocal(message));
        }

        public void Reply<TResponse>(TResponse message)
        {
            StuffToDo.Add(() => rebusBus.Reply(message));
        }

        public void Subscribe<TEvent>()
        {
            StuffToDo.Add(() => rebusBus.Subscribe<TEvent>());
        }

        public void Unsubscribe<TEvent>()
        {
            StuffToDo.Add(() => rebusBus.Unsubscribe<TEvent>());
        }

        public void Publish<TEvent>(TEvent message)
        {
            StuffToDo.Add(() => rebusBus.Publish(message));
        }

        public void Defer(TimeSpan delay, object message)
        {
            StuffToDo.Add(() => rebusBus.Defer(delay, message));
        }

        public void AttachHeader(object message, string key, string value)
        {
            rebusBus.AttachHeader(message, key, value);
        }

        public IAdvancedBus Advanced
        {
            get { return this; }
        }

        public IRebusEvents Events
        {
            get { return rebusBus.Advanced.Events; }
        }

        public IRebusBatchOperations Batch
        {
            get { return new DelayedBatchOperations(rebusBus.Advanced.Batch); }
        }

        public IRebusRouting Routing
        {
            get { return new DelayedRoutingOperations(rebusBus.Advanced.Routing); }
        }

        private class DelayedBatchOperations : IRebusBatchOperations
        {
            private readonly IRebusBatchOperations _innerBatchOperations;

            public DelayedBatchOperations(IRebusBatchOperations innerBatchOperations)
            {
                _innerBatchOperations = innerBatchOperations;
            }

            public void Send(IEnumerable messages)
            {
                StuffToDo.Add(() => _innerBatchOperations.Send(messages));

            }

            public void Publish(IEnumerable messages)
            {
                StuffToDo.Add(() => _innerBatchOperations.Publish(messages));

            }

            public void Reply(IEnumerable messages)
            {
                StuffToDo.Add(() => _innerBatchOperations.Reply(messages));

            }
        }

        class DelayedRoutingOperations : IRebusRouting
        {
            readonly IRebusRouting innerRoutingOperations;

            public DelayedRoutingOperations(IRebusRouting innerRoutingOperations)
            {
                this.innerRoutingOperations = innerRoutingOperations;
            }

            public void Send<TCommand>(string destinationEndpoint, TCommand message)
            {
                StuffToDo.Add(() => innerRoutingOperations.Send(destinationEndpoint, message));
            }

            public void Subscribe<TEvent>(string publisherInputQueue)
            {
                StuffToDo.Add(() => innerRoutingOperations.Subscribe<TEvent>(publisherInputQueue));
            }

            public void ForwardCurrentMessage(string destinationEndpoint)
            {
                StuffToDo.Add(() => innerRoutingOperations.ForwardCurrentMessage(destinationEndpoint));
            }
        }
    }
}
