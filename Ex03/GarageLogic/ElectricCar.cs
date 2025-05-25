using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;

namespace Ex03.GarageLogic
{
    internal class ElectricCar : Car
    {
        protected ElectricType m_electricCarInfo;
        
        //functions: 
        public ElectricType electricInfo
        {
            get { return m_electricCarInfo; }
            set { m_electricCarInfo = value; }
        }

        public ElectricCar(string i_licensePlate, string i_modelName)
        {
            m_licensePlate = i_licensePlate;
            m_modelName = i_modelName;
            m_listOfTires = new List<Tire>(); 
            for (int i = 0;i < Constants.k_numberOfWheelsCar; i++)
            {
                m_listOfTires.Add(new Tire(32f));
            }
            m_electricCarInfo = new ElectricType(4.8f, 0f); 
        }
    }
}
