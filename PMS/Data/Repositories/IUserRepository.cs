using PMS.Models;

namespace PMS.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<(int, bool)> LogInUser(string email, string password);
    }
}
