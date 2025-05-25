using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Enums
    {
        public enum EVehicleStatus
        {
            InRepair,
            Fixed,
            Paid
        }

        public enum EFuelType
        {
            Octan95,
            Octan96,
            Octan98,
            Soler
        }

        public enum ECarColor
        {
            Yellow, 
            Black, 
            White, 
            Silver
        }

        public enum ENumberOfDoors
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        public enum ELicenseType
        {
            A,
            A2,
            AB, 
            B2
        }
    }
}
