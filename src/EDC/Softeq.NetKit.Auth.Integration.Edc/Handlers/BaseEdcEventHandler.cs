// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.NetKit.Auth.Integration.Edc.Events;
using Softeq.NetKit.Components.EventBus.Abstract;

namespace Softeq.NetKit.Auth.Integration.Edc.Handlers
{
    public abstract class BaseEdcEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : BaseEdcEvent
    {
        public async Task Handle(TEvent @event)
        {
            await HandleEvent(@event);
        }

        public abstract Task HandleEvent(TEvent @event);
    }
}