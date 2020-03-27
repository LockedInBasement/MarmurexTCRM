using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarmurexTCRMDataManager.Library.Models;
using MarmurexTCRMDataManager.Library.Internal.DataAccess;
using Microsoft.Extensions.Configuration;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly IConfiguration configuration;
        private readonly ISqlDataAccess sqlDataAccess;

        public UserData(IConfiguration configuration, ISqlDataAccess sqlDataAccess)
        {
            this.configuration = configuration;
            this.sqlDataAccess = sqlDataAccess;
        }

        public List<UserModel> GetUserById(string Id)
        {
            var p = new { Id = Id };

            var output = sqlDataAccess.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "TCRMMarmurexDatabase");

            return output;
        }
    }
}
