using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InflationLinkedBond
{
    class Pricing
    {
        private int _settlementDays;
        private float _notional;
        private DateTime _issuedDate;
        private DateTime _pricingDate;
        private DateTime _maturityDate;
        private DateTime _nextPaymentDate;
        private float _cpiGrowth;
        private float _coupon;
        private int _frequancy;
        
        public Pricing(int settlementDays,float notional,DateTime pricingDate,DateTime issuedDate,DateTime maturityDate,DateTime nextPaymentDate,float coupon,float cpiGrowth,int frequancy)
        {
            if(settlementDays < 0)
            {
                Console.WriteLine("wrong settlement days");
            }
            _settlementDays = settlementDays;
            _notional = notional;
            _issuedDate = issuedDate;
            _pricingDate = pricingDate;
            _nextPaymentDate = nextPaymentDate;
            _cpiGrowth = cpiGrowth;
            _maturityDate = maturityDate;
            _coupon = coupon;
            _frequancy = frequancy;
        }
        public void calculate(out double cleanPrice,out double accruedInterest)
        {
            float[] rate = {3.0495f, 2.93f, 2.9795f, 3.029f, 3.1425f, 3.211f};
            DateTime[] rateDate = new DateTime[]
            {
                new DateTime(2014, 11, 25),
                new DateTime(2015, 11, 25),
                new DateTime(2016, 11, 26),
                new DateTime(2017, 11, 25),
                new DateTime(2018, 11, 25),
                new DateTime(2019, 11, 25),
            };
            //double cleanPrice = 0;
            //double accruedInterest = 0;
            accruedInterest = 0;
            cleanPrice = 0;
            int year = 365;
            List<float> factor = new List<float>();    
            // the issue day cpi 
            RefCPI baseCPI = new RefCPI(indexCPI(_issuedDate.AddMonths(-3)), indexCPI(_issuedDate.AddMonths(-2)), _issuedDate);
            // the pricing day cpi
            RefCPI pricingCPI = new RefCPI(indexCPI(_pricingDate.AddMonths(-3)), indexCPI(_pricingDate.AddMonths(-2)), _pricingDate);
            // the maturity day cpi
            RefCPI settlementCPI = new RefCPI(indexCPI(_maturityDate.AddMonths(-3)), indexCPI(_maturityDate.AddMonths(-2)), _maturityDate);
            
            // discount factor is the list contian all discount factors
            DiscountFactor discountFactor = new DiscountFactor(rateDate,rate, _frequancy);
            factor = discountFactor.calculate();
            //the left payment time exclusive the accrued interest days
            int leftPaymentTime = ((_maturityDate.Year - _nextPaymentDate.Year)*12 + _maturityDate.Month - _nextPaymentDate.Month) / (12/_frequancy);

            if (_nextPaymentDate != _pricingDate)
            {
                //the days with interest: pricing date - next payment date
                double tempDayConter = (_pricingDate.Subtract(_nextPaymentDate.AddMonths(-12 / _frequancy)).TotalDays + 3) / year;
                // the time should be considered when calculate the discount factor
                double discounter = 1/Math.Pow((1+_coupon/_frequancy/100),tempDayConter);
                accruedInterest = tempDayConter * (couponPayment() / 100) * discounter * pricingCPI.calculate() / baseCPI.calculate();
            }
            for(int i= 0 ;i<leftPaymentTime;i++)
            {
                RefCPI currentCPI = new RefCPI(indexCPI(_nextPaymentDate.AddMonths(i * 12 / _frequancy - 3)), indexCPI(_nextPaymentDate.AddMonths(i * 12 / _frequancy - 2)), _nextPaymentDate.AddMonths(i * 12 / _frequancy));
                cleanPrice += couponPayment()/100 * currentCPI.calculate() / baseCPI.calculate() * factor[i];
            }
            cleanPrice = cleanPrice +  _notional*settlementCPI.calculate()/ baseCPI.calculate();

            //return cleanPrice;

        }
        private float indexCPI(DateTime date)
        {
            float[] cpi = {206.1f, 207.3f, 208.0f, 208.9f, 209.7f, 210.9f,
                209.8f, 211.4f, 212.1f, 214.0f, 215.1f, 216.8f,
                216.5f, 217.2f, 218.4f, 217.7f, 216f,
                212.9f, 210.1f, 211.4f, 211.3f, 211.5f,
                212.8f, 213.4f, 213.4f, 213.4f, 214.4f};
            DateTime beginDate = new DateTime(2011, 11, 10);
            DateTime endDate = new DateTime();
            endDate = beginDate.AddMonths(27);

            int index = ((date.Year - beginDate.Year) * 12) + beginDate.Month - date.Month;
            //Console.WriteLine(index);
            if (index >= 27)
            {
                // Assumption: the cpi increase 3% annually 
                return (float)(cpi[26] * Math.Pow((1 + _cpiGrowth/100), index/12));
            }
            else if (index < 0)
            {
                // if the cpi date less than the beginning date of cpi array, using the first number 
                return cpi[0];
            }
            else
            {
                //Console.WriteLine(index);
                return cpi[index];
            }
            
        }
        private float couponPayment()
        {
            float coupon = _coupon;
            if(_frequancy == 2)
            {
                coupon = _coupon/2;
            }
            return coupon * _notional;
            
        }
    }
}
