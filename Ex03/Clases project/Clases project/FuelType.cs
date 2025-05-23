using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Clases_project.Enums;

namespace Clases_project
{
    internal class FuelType
    {
        protected EFuelType m_eFuelType;
        protected float m_maxFuelTank;
        protected float m_currentLiterTank;


        //functions: 
        public void AddFuel(float i_addfuel)
        {
            if (m_currentLiterTank + i_addfuel > m_maxFuelTank)
            {
                throw new Exception($"Cannot be filled beyond the maximum {m_maxFuelTank}.");
            }
            m_currentLiterTank += i_addfuel;
        }
    }
}
