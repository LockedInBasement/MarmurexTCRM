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
    public class ProductData
    {
        private readonly IConfiguration configuration;

        public ProductData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TCRMMarmurexDatabase");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TCRMMarmurexDatabase").FirstOrDefault();

            return output;
        }
    }
}
