using MarmurexTCRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();

        Task<Dictionary<string, string>> GetAllRoles();

        Task AddUserToRole(string userId, string roleName);

        Task RemoveUserFromRole(string userId, string roleName);
    }
}