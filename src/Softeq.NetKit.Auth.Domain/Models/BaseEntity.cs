// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel.DataAnnotations;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.Domain.Models
{
    public class Entity<T> : IBaseEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }
    }
}