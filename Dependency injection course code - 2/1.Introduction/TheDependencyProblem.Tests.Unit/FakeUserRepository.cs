using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheDependencyProblem.Data;

namespace TheDependencyProblem.Tests.Unit;

public class FakeUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();
    
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult(_users.Values.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_users[id]);
    }

    public Task<bool> CreateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}