﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex03.GarageLogic.Enums;

namespace Ex03.GarageLogic
{
    public class Vehicle
    {
        protected string m_modelName;
        protected string m_licensePlate;
        protected float m_currentEnergyPercent;
        protected List<Tire> m_listOfTires;
        protected string m_ownerName;
        protected string m_ownerPhone;
        protected eVehicleStatus m_vehicleStatus = eVehicleStatus.InRepair;

        //functions: 
        public Vehicle()
        {
            m_modelName = string.Empty;
            m_licensePlate = string.Empty;
            m_currentEnergyPercent = 0f;
            m_listOfTires = new List<Tire>();
            m_ownerName = string.Empty;
            m_ownerPhone = string.Empty;
            m_vehicleStatus = eVehicleStatus.InRepair;
        }
        public Vehicle(string i_LicensePlate, string i_ModelName)
        {
            m_licensePlate = i_LicensePlate;
            m_modelName = i_ModelName;
            m_currentEnergyPercent = 0f;
            m_listOfTires = new List<Tire>();
            m_ownerName = string.Empty;
            m_ownerPhone = string.Empty;
            m_vehicleStatus = eVehicleStatus.InRepair;
        }
        public string ModelName
        {
            get { return m_modelName; }
        }

        public string LicensePlate
        {
            get { return m_licensePlate; }
        }

        public float CurrentEnergyPercent
        {
            get { return m_currentEnergyPercent; }
            set { m_currentEnergyPercent = value; }
        }

        public List<Tire> Tires
        {
            get { return m_listOfTires; }
        }

        public string OwnerName
        {
            get { return m_ownerName; }
            set { m_ownerName = value; }
        }

        public string OwnerPhone
        {
            get { return m_ownerPhone; }
            set { m_ownerPhone = value; }
        }

        public eVehicleStatus VehicleStatus
        {
            get { return m_vehicleStatus; }
            set { m_vehicleStatus = value; }
        }
    }
}
