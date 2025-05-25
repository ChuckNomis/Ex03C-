using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageLogic;
using static Ex03.GarageLogic.Enums;


namespace Ex03.GarageLogic
{
    internal class Motorcycle : Vehicle
    {
        protected ELicenseType eLicenseType;
        protected int m_engineVolume;


        //functions:
        public Motorcycle()
        {
            eLicenseType = ELicenseType.A;
            m_engineVolume = 0;
            m_listOfTires = new List<Tire>();
            for (int i = 0; i < Constants.k_numberOfWheelsMotorcycle; i++)
            {
                m_listOfTires.Add(new Tire(30f));
            }
        }

        public ELicenseType LicenseType
        {
            get { return eLicenseType; }
            set { eLicenseType = value; }
        }

        public int EngineVolume
        {
            get { return m_engineVolume; }
            set { m_engineVolume = value; }
        }
    }

}
