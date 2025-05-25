    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;
using static Ex03.GarageLogic.Enums;

namespace Ex03.GarageLogic
{
    public class FuelMotorcycle : Motorcycle
    {
        protected FuelType m_fuelInfo;

        //functions:
        public FuelType FuelInfo
        {
            get { return m_fuelInfo; }
        }

        public FuelMotorcycle(string i_licensePlate, string i_modelName): base(i_licensePlate, i_modelName)
        {
            m_fuelInfo = new FuelType(EFuelType.Octan98, 5.8f, 0f);
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsMotorcycle; i++)
            {
                m_listOfTires.Add(new Tire(30f));
            }
        }

    }
}
