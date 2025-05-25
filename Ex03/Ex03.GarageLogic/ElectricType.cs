using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class ElectricType
    {
        protected float m_maxBatteryTime;
        protected float m_currentBatteryTime;

        //functions:

        public void SetBatteryLevelFromPercent(float percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ValueRangeException(0, 100);
            }
            m_currentBatteryTime = (percent / 100f) * m_maxBatteryTime;
        }


        public void ChargingBattery(float i_batteryTime)
        {
            if(m_currentBatteryTime + i_batteryTime > m_maxBatteryTime)
            {
                throw new Exception($"Cannot be charged beyond the maximum hours{m_maxBatteryTime}.");
            }
            m_currentBatteryTime += i_batteryTime; 
        }

        public ElectricType(float i_maxBattery, float i_currentBatteryTime)
        {
            if (i_currentBatteryTime > m_maxBatteryTime)
            {
                throw new Exception("Current battery time cannot exceed max battery time.");
            }

            m_maxBatteryTime = i_maxBattery;
            m_currentBatteryTime = i_currentBatteryTime;    
        }

        public float MaxBatteryTime
        {
            get { return m_maxBatteryTime; }
        }

        public float CurrentBatteryTime
        {
            get { return m_currentBatteryTime; }
        }
    }
}
