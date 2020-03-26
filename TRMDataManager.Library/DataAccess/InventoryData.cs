using MarmurexTCRMDataManager.Library.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarmurexTCRMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration configuration;

        public InventoryData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TCRMMarmurexDatabase");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(configuration);

            sql.SaveData("dbo.spInventory_Insert", item, "TCRMMarmurexDatabase");
        }
    }
}
