// Developed by Softeq Development Corporation
// http://www.softeq.com
namespace Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure
{
    public interface IBaseEntity<T> : IEntity
    {
        T Id { get; set; }
    }
}