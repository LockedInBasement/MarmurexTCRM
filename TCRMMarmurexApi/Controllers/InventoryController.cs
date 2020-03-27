using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarmurexTCRMDataManager.Library.DataAccess;
using MarmurexTCRMDataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TCRMMarmurexApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public InventoryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData(configuration);

            return data.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post(InventoryModel item)
        {
            InventoryData data = new InventoryData(configuration);

            data.SaveInventoryRecord(item);
        }

    }
}