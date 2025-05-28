using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;

namespace Ex03.GarageLogic
{
    public class ElectricCar : Car
    {
        protected ElectricType m_electricInfo;
        
        //functions: 
        public ElectricType ElectricInfo
        {
            get { return m_electricInfo; }
            set { m_electricInfo = value; }
        }

        public ElectricCar(string i_licensePlate, string i_modelName): base(i_licensePlate, i_modelName)
        {
            m_electricInfo = new ElectricType(4.8f, 0f);
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsCar; i++)
            {
                m_listOfTires.Add(new Tire(32f));
            }
        }
    }
}
