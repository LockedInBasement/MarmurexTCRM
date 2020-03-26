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
    [Authorize(Roles = "Cashier,Manager,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public List<ProductModel> Get()
        {
            ProductData data = new ProductData(configuration);

            return data.GetProducts();
        }

    }
}