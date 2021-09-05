using IdentityServer4.Models;
using IdentityServer4.Stores;
using Idp4.Models.DbContexts;
using Idp4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Idp4.Models.Stores
{
    public class IdpPersistedGrantStore : IPersistedGrantStore
    {
        private UserDbContext _context;
        public IdpPersistedGrantStore (UserDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var res =  _context.GrantStore.Where(t => t.SubjectId == filter.SubjectId).Select(t => new PersistedGrant()
            {
                ClientId = t.ClientId,
                SubjectId = t.SubjectId,
                ConsumedTime = t.ConsumedTime,
                CreationTime = t.CreationTime,
                Type = t.Type,
                Data = t.Data,
                Description = t.Description,
                Expiration = t.Expiration,
                Key = t.Key,
                SessionId = t.SessionId
            }).AsEnumerable();
            _context.SaveChanges();

            return Task.FromResult(res);           
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var res = _context.GrantStore.Where(t => t.Key == key).Select(t => new PersistedGrant()
            {
                ClientId = t.ClientId,
                SubjectId = t.SubjectId,
                ConsumedTime = t.ConsumedTime,
                CreationTime = t.CreationTime,
                Type = t.Type,
                Data = t.Data,
                Description = t.Description,
                Expiration = t.Expiration,
                Key = t.Key,
                SessionId = t.SessionId
            }).FirstOrDefault();
            _context.SaveChanges();

            return Task.FromResult(res);
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            _context.GrantStore.RemoveRange(_context.GrantStore.Where(t => t.SubjectId == filter.SubjectId && t.ClientId == filter.ClientId));
            _context.SaveChanges();

            return Task.FromResult(0);
        }

        public Task RemoveAsync(string key)
        {
            _context.GrantStore.Remove(_context.GrantStore.Where(t => t.Key== key).FirstOrDefault());
            _context.SaveChanges();

            return Task.FromResult(0);
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            try
            {
                var _grant = new PersistedGrantModel()
                {
                    ClientId = grant.ClientId,
                    SubjectId = grant.SubjectId,
                    ConsumedTime = grant.ConsumedTime,
                    CreationTime = grant.CreationTime,
                    Type = grant.Type,
                    Data = grant.Data,
                    Description = grant.Description,
                    Expiration = grant.Expiration,
                    Key = grant.Key,
                    SessionId = grant.SessionId
                };
                _context.Add(_grant);
                _context.SaveChanges();

                return Task.FromResult(true);
            } catch(Exception ex)
            {
                return Task.FromResult(false);
            }

        }
    }
}
