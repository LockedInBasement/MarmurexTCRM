using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarmurexTCRMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["tax rate"];

            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal output);

            if (!isValidTaxRate)
            {
                throw new ConfigurationErrorsException("Tax rate is not set up properly");
            }

            return output;
        }
    }
}
