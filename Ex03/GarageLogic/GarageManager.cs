using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private readonly Dictionary<string, Vehicle> r_VehiclesInGarage = new();

        // Load vehicles from file (basic structure, you must implement parsing)
        public void LoadVehiclesFromFile(string i_FilePath)
        {
            string[] lines = File.ReadAllLines(i_FilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                string type = parts[0];
                string license = parts[1];
                string model = parts[2];
                float energyPercent = float.Parse(parts[3]);
                string tireManufacturer = parts[4];
                float tirePressure = float.Parse(parts[5]);
                string ownerName = parts[6];
                string ownerPhone = parts[7];

                Vehicle newVehicle = VehicleCreator.CreateVehicle(type, license, model);

                // Set basic fields
                newVehicle.CurrentEnergyPercent = energyPercent;
                newVehicle.OwnerName = ownerName;
                newVehicle.OwnerPhone = ownerPhone;
                newVehicle.VehicleStatus = eVehicleStatus.InRepair;

                // Set tire info 
                foreach (Tire tire in vehicle.Tires)
                {
                    tire.ManufacturerName = tireManufacturer;

                    float toInflate = tirePressure - tire.CurrentPressure;
                    if (toInflate > 0)
                    {
                        tire.Inflate(toInflate);
                    }
                }


                // Type specific fields
                switch (type)
                {
                    case "FuelMotorcycle":
                    case "ElectricMotorcycle":
                        ((Motorcycle)newVehicle).LicenseType = Enum.Parse<eLicenseType>(parts[8]);
                        ((Motorcycle)newVehicle).EngineVolume = int.Parse(parts[9]);
                        break;

                    case "FuelCar":
                    case "ElectricCar":
                        ((Car)newVehicle).CarColor = Enum.Parse<eCarColor>(parts[8]);
                        ((Car)newVehicle).NumberOfDoors = Enum.Parse<eNumberOfDoors>(parts[9]);
                        break;

                    case "Truck":
                        ((Truck)newVehicle).IsCarryingHazardousMaterial = bool.Parse(parts[8]);
                        ((Truck)newVehicle).CargoVolume = float.Parse(parts[9]);
                        break;
                }

                // Add to garage
                r_VehiclesInGarage.Add(license, newVehicle);
            }
        }

        public bool ContainsVehicle(string i_LicensePlate)
        {
            return r_VehiclesInGarage.ContainsKey(i_LicensePlate);
        }

        // Add or update a vehicle
        public void AddOrUpdateVehicle(Vehicle i_Vehicle)
        {
            if (r_VehiclesInGarage.ContainsKey(i_Vehicle.LicensePlate))
            {
                r_VehiclesInGarage[i_Vehicle.LicensePlate].VehicleStatus = eVehicleStatus.InRepair;
            }
            else
            {
                r_VehiclesInGarage.Add(i_Vehicle.LicensePlate, i_Vehicle);
            }
        }

        // Get all license numbers (optional filter)
        public List<string> GetAllLicenseNumbers(eVehicleStatus? i_Filter = null)
        {
            List<string> result = new();

            foreach (KeyValuePair<string, Vehicle> entry in r_VehiclesInGarage)
            {
                if (i_Filter == null || entry.Value.VehicleStatus == i_Filter)
                {
                    result.Add(entry.Key);
                }
            }

            return result;
        }

        // Change vehicle status
        public void ChangeVehicleStatus(string i_LicensePlate, eVehicleStatus i_NewStatus)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }
            else
            {
                r_VehiclesInGarage[i_LicensePlate].VehicleStatus = i_NewStatus;
            }
        }

        // Inflate tires to max
        public void InflateTiresToMax(string i_LicensePlate)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            foreach (var tire in r_VehiclesInGarage[i_LicensePlate].Tires)
            {
                tire.InflateToMax();
            }
        }

        // Refuel fuel vehicle
        public void RefuelVehicle(string i_LicensePlate, eFuelType i_FuelType, float i_Amount)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }
            FuelType fuelVehicle = r_VehiclesInGarage[i_LicensePlate] as FuelType;

            if (fuelVehicle == null)
            {
                throw new ArgumentException("Vehicle is not fuel-based.");
            }

            if (fuelVehicle.FuelType != i_FuelType)
            {
                throw new ArgumentException("Incorrect fuel type.");
            }

            fuelVehicle.AddFuel(i_Amount);

        }

        // Recharge electric vehicle
        public void RechargeVehicle(string i_LicensePlate, float i_Minutes)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            ElectricType electricVehicle = r_VehiclesInGarage[i_LicensePlate] as ElectricType;

            if (electricVehicle == null)
            {
                throw new ArgumentException("Vehicle is not electric.");
            }

            electricVehicle.ChargingBattery(i_Minutes / 60f); // minutes to hours
        }


        // Get full vehicle details
        public string GetVehicleDetails(string i_LicensePlate)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            Vehicle chosenVehicle = r_VehiclesInGarage[i_LicensePlate];

            string tiresInfo = string.Join("\n", chosenVehicle.Tires);
            string energyInfo = chosenVehicle is FuelType f ? $"Fuel: {f.CurrentLiterInTank}/{f.MaxLiterInTank} ({f.FuelType})" :
                                 chosenVehicle is ElectricType e ? $"Battery: {e.CurrentBatteryTime}/{e.MaxBatteryTime} hrs";

            return $"Model: {chosenVehicle.ModelName}\n" +
                   $"License: {chosenVehicle.LicensePlate}\n" +
                   $"Owner: {chosenVehicle.OwnerName}, {chosenVehicle.OwnerPhone}\n" +
                   $"Status: {chosenVehicle.VehicleStatus}\n" +
                   $"Energy: {chosenVehicle.CurrentEnergyPercent}%\n" +
                   $"{energyInfo}\n" +
                   $"Tires:\n{tiresInfo}";
        }
    }
}
