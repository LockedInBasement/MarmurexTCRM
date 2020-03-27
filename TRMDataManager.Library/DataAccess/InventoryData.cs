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
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration configuration;
        private readonly ISqlDataAccess sqlDataAccess;

        public InventoryData(IConfiguration configuration, ISqlDataAccess sqlDataAccess)
        {
            this.configuration = configuration;
            this.sqlDataAccess = sqlDataAccess;
        }

        public List<InventoryModel> GetInventory()
        {
            var output = sqlDataAccess.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TCRMMarmurexDatabase");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            sqlDataAccess.SaveData("dbo.spInventory_Insert", item, "TCRMMarmurexDatabase");
        }
    }
}
