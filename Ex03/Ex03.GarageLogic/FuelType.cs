using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Enums;

namespace Ex03.GarageLogic
{
    public class FuelType
    {
        protected EFuelType m_eFuelType;
        protected float m_maxFuelTank;
        protected float m_currentLiterTank;


        //functions: 
        public FuelType(EFuelType i_fuelType, float i_maxFuelTank, float i_currentFuel)
        {
            if(i_currentFuel > i_maxFuelTank)
            {
                throw new ArgumentException("Current fuel amount exceeds tank capacity."); 
            }

            m_eFuelType = i_fuelType;
            m_maxFuelTank = i_maxFuelTank;
            m_currentLiterTank = i_currentFuel; 
        }



        public EFuelType fuelType
        {
            get { return m_eFuelType; }
        }

        public float MaxLiterInTank
        {
            get { return m_maxFuelTank; } 
        }

        public float CurrentLiterInTank
        {
            get { return m_currentLiterTank; }

        }

        public void SetFuelLevelFromPercent(float percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ValueRangeException(0, 100);
            }
            m_currentLiterTank = (percent / 100f) * m_maxFuelTank;
        }

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
