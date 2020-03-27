using MarmurexTCRMDataManager.Library.Models;
using System.Collections.Generic;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}