using MarmurexTCRMDataManager.Library.Internal.DataAccess;
using MarmurexTCRMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public ProductData(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public List<ProductModel> GetProducts()
        {
            var output = sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TCRMMarmurexDatabase");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TCRMMarmurexDatabase").FirstOrDefault();

            return output;
        }
    }
}
