using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticationHeaderValue> Authenticate(string username, string password);
    }
}