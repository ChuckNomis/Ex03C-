using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Enums;


namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        protected eCarColor eColor;
        protected eNumberOfDoors eNumberOfDoors;

        public Car(string i_LicensePlate, string i_ModelName): base(i_LicensePlate, i_ModelName){}

        //functions: 
        public eCarColor CarColor
        {
            get { return eColor; }
            set { eColor = value; }
        }

        public eNumberOfDoors NumberOfDoors
        {
            get { return eNumberOfDoors; }
            set { eNumberOfDoors = value; }
        }

    }
}
