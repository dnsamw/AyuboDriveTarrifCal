using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyuboDriveTarrifCal
{
    class Vehicle
    {
        private string VehicleType { get; set; }
        private int PerDayCharge { get; set; } // vehicle only charge
        private bool IsDriverIncluded { get; set; }
        private int DriverFeePerDay { get; set; }
        private int PerDayVD { get; set; } //vehicle charge + driver fee
        private int Days { get; set; }

        //Constructor1
        public Vehicle(string VehicleType,int PerDayCharge,bool IsDriverIncluded, int DriverFeePerDay)
        {
            this.VehicleType = VehicleType;
            this.PerDayCharge = PerDayCharge;
            this.IsDriverIncluded = IsDriverIncluded;
            this.DriverFeePerDay = DriverFeePerDay;
        }

        //Constructor2
        public Vehicle(int PerDayVD,int Days)
        {
            this.PerDayVD = PerDayVD;
            this.Days = Days;
        }

        public int PerDay()
        {
            if (IsDriverIncluded)
            {
                return (PerDayCharge + DriverFeePerDay);
            }
            else
            {
                return (PerDayCharge);
            }

        }

        public int PerWeek()
        {
            if (IsDriverIncluded)
            {
                return ((PerDayCharge*7) + (DriverFeePerDay*7));
            }
            else
            {
                return (PerDayCharge*7);
            }
        }

        public int PerMonth()
        {
            if (IsDriverIncluded)
            {
                return ((PerDayCharge * 30) + (DriverFeePerDay * 30));
            }
            else
            {
                return (PerDayCharge * 30);
            }
        }

        public int StandardRent()
        {
            return PerDayVD * Days;
        }





    }
}
