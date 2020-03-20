using MarmurexTCRMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel saleModel);
    }
}