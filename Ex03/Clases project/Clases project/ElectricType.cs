using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases_project
{
    internal class ElectricType
    {
        protected float m_maxBatteryTime;
        protected float m_currentBatteryTime;
        
        //functions:
        public void ChargingBattery(float i_batteryTime)
        {
            if(m_currentBatteryTime + i_batteryTime > m_maxBatteryTime)
            {
                throw new Exception($"Cannot be charged beyond the maximum hours{m_maxBatteryTime}.");
            }
            m_currentBatteryTime += i_batteryTime; 
        }
    }
}
