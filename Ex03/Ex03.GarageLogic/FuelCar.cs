using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;
using static Ex03.GarageLogic.Enums;


namespace Ex03.GarageLogic
{
    public class FuelCar : Car
    {   
        protected readonly FuelType m_fuelInfo;

        public FuelCar(string i_LicensePlate, string i_ModelName): base(i_LicensePlate, i_ModelName)
        {
            m_fuelInfo = new FuelType(EFuelType.Octan95, 48f, 0f);
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsCar; i++)
            {
                m_listOfTires.Add(new Tire(32f));
            }
        }
        //functions: 

        public FuelType FuelInfo
        {
            get { return m_fuelInfo; }
        }
    }
}
