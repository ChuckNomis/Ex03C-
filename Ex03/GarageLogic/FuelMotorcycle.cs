    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;
using static Ex03.GarageLogic.Enums;

namespace Ex03.GarageLogic
{
    internal class FuelMotorcycle : Motorcycle
    {
        protected FuelType m_fuelInfo;

        //functions:
        public FuelType FuelInfo
        {
            get { return m_fuelInfo; }
            set { m_fuelInfo = value; }
        }

        public FuelMotorcycle(string i_licensePlate, string i_modelName)
        {
            m_licensePlate = i_licensePlate;
            m_modelName = i_modelName;
            m_listOfTires = new List<Tire>();

            for (int i = 0; i < Constants.k_numberOfWheelsMotorcycle; i++)
            {
                m_listOfTires.Add(new Tire(30f));
            }

            m_fuelInfo = new FuelType(EFuelType.Octan98, 5.8f, 0f);   
        }
    }
}
