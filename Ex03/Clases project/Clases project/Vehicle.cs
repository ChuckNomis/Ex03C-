using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Clases_project.Enums;

namespace Clases_project
{
    internal class Vehicle
    {
        protected string  m_modelName;
        protected string m_licensePlate;
        protected float m_currentEnergyPercent;
        protected List<Tire> m_listOfTires;
        protected string m_ownerName;
        protected string m_ownerPhone;
        protected EVehicleStatus m_vehicleStatus;
    }
}
