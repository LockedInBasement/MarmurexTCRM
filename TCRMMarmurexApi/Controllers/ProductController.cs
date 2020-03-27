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
        private readonly IProductData productData;

        public ProductController(IProductData productData)
        {
            this.productData = productData;
        }

        [HttpGet]
        public List<ProductModel> Get()
        {
            return productData.GetProducts();
        }

    }
}