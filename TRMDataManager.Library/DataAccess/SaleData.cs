using MarmurexTCRMDataManager.Library.Internal.DataAccess;
using MarmurexTCRMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess sqlDataAccess;
        private readonly IProductData productData;

        public SaleData(ISqlDataAccess sqlDataAccess, IProductData productData)
        {
            this.sqlDataAccess = sqlDataAccess;
            this.productData = productData;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();

            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = productData.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            try
            {
                sqlDataAccess.StartTransaction("TCRMMarmurexDatabase");

                //Save the sale model
                sqlDataAccess.SaveDataInTransaction("dbo.spSale_Insert", sale);

                //Get the ID from the sale mode
                sale.Id = sqlDataAccess.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { sale.CashierId, sale.SaleDate }).FirstOrDefault();

                foreach (var item in details)
                {
                    item.SaleId = sale.Id;
                    //Save the detail model
                    sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }

                sqlDataAccess.CommitTransaction();
            }
            catch
            {
                sqlDataAccess.RollbackTransaction();
                throw;
            }

        }

        public List<SaleReportModel> GetSaleReport()
        {
            var output = sqlDataAccess.LoadData<SaleReportModel, dynamic>("dbo.spSale_Report", new { }, "TCRMMarmurexDatabase");

            return output;
        }
    }
}
