using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MarmurexTCRMDataManager.Library.DataAccess;
using MarmurexTCRMDataManager.Library.Models;

namespace MarmurexTCRMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [Authorize(Roles = "Manager,Admin")]
        public List<InventoryModel> GetSalesReport()
        {
            InventoryData data = new InventoryData();

            return data.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData();

            data.SaveInventoryRecord(item);
        }
    }
}
