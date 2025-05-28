using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;

namespace Ex03.GarageLogic
{
    public class ElectricMotorcycle : Motorcycle
    {
        protected ElectricType m_electricInfo;

        public ElectricMotorcycle(string i_licensePlate, string i_modelName): base(i_licensePlate, i_modelName)
        {
            m_electricInfo = new ElectricType(3.2f, 0f);
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsMotorcycle; i++)
            {
                m_listOfTires.Add(new Tire(30f));
            }
        }

        //functions: 
        public ElectricType ElectricInfo
        {
            get { return m_electricInfo; }
            set { m_electricInfo = value; }
        }
    }
}
