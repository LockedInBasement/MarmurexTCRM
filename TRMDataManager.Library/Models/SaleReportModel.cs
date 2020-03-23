using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmurexTCRMDataManager.Library.Models
{
    public class SaleReportModel
    {
        public DateTime SaleData { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public string FistName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
