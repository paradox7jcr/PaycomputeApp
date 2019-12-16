using System;
using System.Collections.Generic;
using System.Text;

namespace Paycompute.Services.Implementation
{
    public class TaxService : ITaxService
    {
        private decimal taxRate;
        private decimal tax;
        public decimal TaxAmount(decimal totalAmount)
        {
            if (totalAmount <= 1042)
            {
                //Tax Free Rate
                taxRate = 0.0m;
                tax = totalAmount * taxRate;
            }
            else if(totalAmount <= 3125)
            {
                //Basic Tax Rate
                taxRate = 0.20m;
                tax = (totalAmount - 1042) * taxRate;
            }
            else if(totalAmount <= 12500)
            {
                //Higher Tax Rate
                taxRate = 0.40m;
                tax = (3125 - 1042) * 0.20m + (totalAmount - 3125) * taxRate;
            }
            else if(totalAmount > 12500)
            {
                //Additional Tax Rate
                taxRate = .45m;
                tax = (3125 - 1042) * 0.20m + (12500 - 3125) * 0.40m + (totalAmount - 12500) * taxRate;
            }
            return tax;
        }
    }
}
