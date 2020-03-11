using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task GetLoggedInUserInfo(string token);
    }
}