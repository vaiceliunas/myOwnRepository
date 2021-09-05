using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Idp4.Models.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Idp4.Models.Stores
{
    public class UserProfileService : IProfileService
    {
        public readonly UserDbContext _context;
        public UserProfileService(UserDbContext context)
        {
            _context = context;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _context.UserClaims.Where(t => t.UserId.ToString() == subjectId).ToList();

            context.IssuedClaims = claimsForUser.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}
