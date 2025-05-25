using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;


namespace Ex03.GarageLogic
{
    internal class FuelCar : Car
    {   
        protected readonly FuelType m_fuelCarInfo;
        
        //functions: 
        public FuelCar(string i_licensePLate, string i_modelInfo)
        {
            m_licensePlate = i_licensePLate;
            m_modelName = i_modelInfo;
            m_fuelCarInfo = new FuelType(Enums.EFuelType.Octan95, 48f, 0f);
            m_listOfTires = new List<Tire>(); 
            for (int i = 0; i < Constants.k_numberOfWheelsCar; i++)
            {
                m_listOfTires.Add(new Tire(32f));
            }
        }
        public FuelType FuelCarInfo
        {
            get { return m_fuelCarInfo; }
        }
    }
}
