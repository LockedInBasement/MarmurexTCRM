using MarmurexTCRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}