    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;

namespace Ex03.GarageLogic
{
    internal class Truck : Vehicle
    {
        protected bool m_isCargoHazardousMaterial;
        protected float m_cargoVolume;

        //functions: 
        public Truck()
        {
            m_isCargoHazardousMaterial = false;
            m_cargoVolume = 0f;
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsTruck; i++)
            {
                m_listOfTires.Add(new Tire(27f));
            }
        }

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
