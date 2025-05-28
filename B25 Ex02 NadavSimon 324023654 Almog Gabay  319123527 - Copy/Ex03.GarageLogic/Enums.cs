using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Enums
    {
        public enum eVehicleStatus
        {
            InRepair,
            Fixed,
            Paid
        }

        public enum eFuelType
        {
            Octan95,
            Octan96,
            Octan98,
            Soler
        }

        public enum eCarColor
        {
            Yellow, 
            Black, 
            White, 
            Silver
        }

        public enum eNumberOfDoors
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public enum eLicenseType
        {
            A,
            A2,
            AB, 
            B2
        }
    }
}
