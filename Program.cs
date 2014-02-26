using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InflationLinkedBond
{
    class Program
    {
        static void Main(string[] args)
        {
            double cleanPrice, accruedInterest;
            int settlementDay = 3;
            float notional = 1000000;
            DateTime pricingDate = DateTime.Today;
            DateTime issuedDate = pricingDate.AddYears(-1).AddMonths(2);
            DateTime maturityDate = pricingDate.AddYears(3).AddMonths(2);
            DateTime nextPaymentDate = pricingDate.AddMonths(2);
            float coupon = 3.1f;
            float cpiGrowth = 3.0f;
            int  frequancy = 1;


            Pricing cpiBond = new Pricing(settlementDay, notional, pricingDate, issuedDate, maturityDate, nextPaymentDate, coupon, cpiGrowth, frequancy);
            cpiBond.calculate(out cleanPrice,out accruedInterest);
            Console.WriteLine("The clean price for this bond is : " + cleanPrice + "\nThe accrued interest is : " + accruedInterest);
            Console.ReadLine();
        }
    }
}
