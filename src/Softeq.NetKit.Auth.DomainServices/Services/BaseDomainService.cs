// Developed by Softeq Development Corporation
// http://www.softeq.com

using AutoMapper;
using EnsureThat;
using Softeq.NetKit.Auth.Infrastructure.Interfaces.Infrastructure;

namespace Softeq.NetKit.Auth.DomainServices.Services
{
    public class BaseDomainService<T> where T: IUnitOfWork
    {
        protected readonly T UnitOfWork;
        protected readonly IMapper Mapper;

        protected BaseDomainService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            Ensure.That(unitOfWork, "unitOfWork").IsNotNull();
            Ensure.That(mapper, "mapper").IsNotNull();

            UnitOfWork = (T)unitOfWork;
            Mapper = mapper;
        }
    }
}
