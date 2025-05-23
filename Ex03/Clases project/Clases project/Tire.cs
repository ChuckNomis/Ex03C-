using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex03.GarageLogic;

namespace Clases_project
{
    internal class Tire
    {
        protected string m_manufacturerName;
        protected float m_currentTirePressure;
        protected float m_maxTirePressure; 


        //functions: 
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
