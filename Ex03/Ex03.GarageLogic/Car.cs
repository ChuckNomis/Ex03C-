using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Enums;


namespace Ex03.GarageLogic
{
    internal class Car : Vehicle
    {
        protected EColor eColor;
        protected ENumberOfDoors eNumberOfDoors;

        //functions: 
        public EColor CarColor
        {
            get { return eColor; }
            set { eColor = value; }
        }

        public ENumberOfDoors NumberOfDoors
        {
            get { return eNumberOfDoors; }
            set { eNumberOfDoors = value; }
        }

    }
}
