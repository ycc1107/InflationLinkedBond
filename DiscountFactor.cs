using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;



namespace InflationLinkedBond
{
    class DiscountFactor
    {
        private List<DateTime> _rateDate;
        private List<float> _rate;
        private int _frequancy;

        public DiscountFactor(DateTime[] rateDate,float[] rate, int frequancy)
        {

            if(rateDate.Length != rate.Length && rate.Length > 1)
            {
                Console.WriteLine("input wrong.");
            }
            else
            {
                _rateDate = new List<DateTime>();
                _rate = new List<float>();
                for (int i = 0; i < rateDate.Length; i++)
                {
                    _rateDate.Add(rateDate[i]);
                    _rate.Add(rate[i]);
                }
                _frequancy = frequancy;
            }
        }

        public List<float> calculate()
        {
            int halfYear = 365/2;
            int year = 365;
            int dayCounter = Convert.ToInt32(_rateDate[2].Subtract(_rateDate[1]).TotalDays);
            List<float> discountFactor = new List<float>();


            if(dayCounter >= year && _frequancy == 2)
            {
                //given annual rate and pay 2 times a year
                discountFactor.Add(1/(1+_rate[0]/200));
                discountFactor.Add(1/(1+_rate[0]/200)*discountFactor[0]);
                for(int i = 1; i< _rate.Count;i++)
                {
                     discountFactor.Add(1/(1+_rate[i]/200)*discountFactor[discountFactor.Count-1]);
                     discountFactor.Add(1/(1+_rate[i]/200)*discountFactor[discountFactor.Count-1]);
                }
            }
            else if(dayCounter < year && dayCounter >=halfYear  && _frequancy == 2)
            {
                // given semi-annual rate and pay 2 time a year
                 discountFactor.Add(1/(1+_rate[0]/100));
                for(int i = 1; i< _rate.Count;i++)
                {
                     discountFactor.Add(1/(1+_rate[i]/100)*discountFactor[discountFactor.Count-1]);
                }
            }
            else if(dayCounter >= year && _frequancy == 1)
            {
                // given annual rate and pay 1 time a year
                 discountFactor.Add(1/(1+_rate[0]/100));
                for(int i = 1; i< _rate.Count;i++)
                {
                     discountFactor.Add(1/(1+_rate[i]/100)*discountFactor[discountFactor.Count-1]);
                }
            }
            else
            {
                Console.WriteLine("woring inpout");
            }

            return discountFactor;
        }

    
    }
}
