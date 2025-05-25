using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;
using static Ex03.GarageLogic.Enums;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        protected readonly FuelType m_fuelInfo;
        protected bool m_isCargoHazardousMaterial;
        protected float m_cargoVolume;

        public FuelType FuelInfo
        {
            get { return m_fuelInfo; }
        }

        public Truck(string i_licensePlate, string i_modelName): base(i_licensePlate, i_modelName)
        {
            m_fuelInfo = new FuelType(EFuelType.Soler, 115f, 0f);
            m_isCargoHazardousMaterial = false;
            m_cargoVolume = 0f;

            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsTruck; i++)
            {
                m_listOfTires.Add(new Tire(27f));
            }
        }
        //functions: 


        public bool IsCargoHazardousMaterial
        {
            get { return m_isCargoHazardousMaterial; }
            set { m_isCargoHazardousMaterial = value; }
        }

        public float CargoVolume
        {
            get { return m_cargoVolume; }
            set { m_cargoVolume = value; }
        }             
    }
}
