using GrisAPI.Models;

namespace GrisAPI.Repositories.UserRepository;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task UpdateUserAsync(User user);
}