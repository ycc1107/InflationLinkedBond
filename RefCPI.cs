using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InflationLinkedBond
{
    class RefCPI
    {
        private float _cpi3;
        private float _cpi2;
        private int _day;
        private int _month;
        private int _year;


        public RefCPI(float cpi3,float cpi2,DateTime date)
        {
            _cpi2 = cpi2;
            _cpi3 = cpi3;
            _day = date.Day;
            _month = date.Month;
            _year = date.Year;
        }
        public float calculate()
        {
            if (isFirstDay(_day))
            {
                // if issued day is the first day of month, return fouth month preceding the month in which issue date occurs

                return _cpi3;
            }
            else
            {
                //return the refcpi 
                //Console.WriteLine(_cpi3 +"   "+_cpi2);
                return _cpi3 + ((_day - 1) / DateTime.DaysInMonth(_year, _month) * (_cpi3 - _cpi2));
            }
        }
        private bool isFirstDay(int day)
        {
            if (day == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
          
            
        }
    }
}
