using MarmurexTCRMDataManager.Library.Models;
using System.Collections.Generic;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReport();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}