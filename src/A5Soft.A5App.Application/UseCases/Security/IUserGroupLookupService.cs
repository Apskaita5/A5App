using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A service that gets a list of <see cref="UserGroupLookup"/>
    /// if the user can invoke the use case, that requests lookup,
    /// and the use case requires the lookup.
    /// </summary>
    [RemoteService(ServiceLifetime.Transient)]
    public interface IUserGroupLookupService
    {
        /// <summary>
        /// Gets a list of <see cref="UserGroupLookup"/>.
        /// </summary>
        /// <param name="requesterType">a type of the use case that requests the lookup</param>
        [RemoteMethod]
        Task<List<UserGroupLookup>> FetchAsync(Type requesterType);
    }
}