using MarmurexTCRMDataManager.Library.Models;
using System.Collections.Generic;

namespace MarmurexTCRMDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        List<InventoryModel> GetInventory();
        void SaveInventoryRecord(InventoryModel item);
    }
}