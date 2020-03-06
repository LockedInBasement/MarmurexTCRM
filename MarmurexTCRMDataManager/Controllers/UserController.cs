using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace MarmurexTCRMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        // GET: User/Details/5
        public List<UserModel> GetById()
        {
            string uderId = RequestContext.Principal.Identity.GetUserId();

            UserData userData = new UserData();

            return userData.GetUserById(uderId);
        }
    }
}
