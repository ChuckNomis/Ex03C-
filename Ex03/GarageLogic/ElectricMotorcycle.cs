using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;

namespace Ex03.GarageLogic
{
    internal class ElectricMotorcycle : Motorcycle
    {
        protected ElectricType m_electricMotorcycleInfo;


        //functions: 
        public ElectricType electricInfo
        {
            get { return m_electricMotorcycleInfo; }
            set { m_electricMotorcycleInfo = value; }
        }
        public ElectricMotorcycle(string i_licensePlate, string i_modelName)
        {
            m_licensePlate = i_licensePlate;
            m_modelName = i_modelName;
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsMotorcycle; i++)
            {
                m_listOfTires.Add(new Tire(30f)); 
            }
            m_electricMotorcycleInfo = new ElectricType(3.2f, 0f); 
        }
    }
}
