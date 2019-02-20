// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.NetKit.Auth.Domain.Models.User;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;

namespace Softeq.NetKit.Auth.IdentityServer
{
    public class TokenService
    {
        private readonly IPersistedGrantStore _persistedGrantStore;
        private readonly UserManager<User> _userManager;

        public TokenService(IPersistedGrantStore persistedGrantStore, UserManager<User> userManager)
        {
            _persistedGrantStore = persistedGrantStore;
            _userManager = userManager;
        }

        public async Task RevokeTokensAsync(string subjectId, string clientId)
        {
            await _persistedGrantStore.RemoveAllAsync(subjectId, clientId);

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                throw new InvalidOperationException($"Could not find user with the specified id. User id: {subjectId}");
            }

            user.TokenRevokedDate = DateTimeOffset.UtcNow;

            await _userManager.UpdateAsync(user);
        }
    }
}