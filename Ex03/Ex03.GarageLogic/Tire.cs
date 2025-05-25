using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace Ex03.GarageLogic
{
    internal class Tire
    {
        protected string m_manufacturerName;
        protected float m_currentTirePressure;
        protected float m_maxTirePressure;


        //functions: 
        public Tire(float i_maxTirePressure, float i_currentPressure, string i_manufacturer = "")
        {
            if (i_currentPressure > i_maxTirePressure)
            {
                throw new ArgumentException("Current pressure exceeds max pressure.");
            }

            m_maxTirePressure = i_maxTirePressure;
            m_currentTirePressure = i_currentPressure;
            m_manufacturerName = i_manufacturer;
        }

        public Tire(float i_maxPressure) // check what const is better 
        {
            m_maxTirePressure = i_maxPressure;
            m_currentTirePressure = 0f;
            m_manufacturerName = string.Empty;
        }


        public string ManufacturerName
        {
            get { return m_manufacturerName; }
            set { m_manufacturerName = value; }
        }

        public float CurrentTirePressure
        {
            get { return m_currentTirePressure; }
        }

        public float MaxTirePressure
        {
            get { return m_maxTirePressure; }
        }

        public void TireInflating (float i_tirePresureToAdd)
        {
            if(m_currentTirePressure + i_tirePresureToAdd > m_maxTirePressure)
            {
                throw new Exception($"Cannot inflate beyond max pressure of {m_maxTirePressure}."); 
            }
            m_currentTirePressure += i_tirePresureToAdd; 
        }
    }
}
