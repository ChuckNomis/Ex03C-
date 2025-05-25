using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using static Ex03.GarageLogic.Enums;


namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private readonly Dictionary<string, Vehicle> r_VehiclesInGarage = new Dictionary<string, Vehicle>();

        // Load vehicles from file (basic structure, you must implement parsing)
        public void LoadVehiclesFromFile(string i_FilePath)
        {
            string[] lines = File.ReadAllLines(i_FilePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length < 10)
                {
                    continue;
                }
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
                newVehicle.OwnerName = ownerName;
                newVehicle.OwnerPhone = ownerPhone;
                newVehicle.VehicleStatus = Enums.EVehicleStatus.InRepair;

                // Set tire info 
                foreach (Tire tire in newVehicle.Tires)
                {
                    tire.ManufacturerName = tireManufacturer;

                    float toInflate = tirePressure - tire.CurrentTirePressure;
                    if (toInflate > 0)
                    {
                        tire.TireInflating(toInflate);
                    }
                }

                // Type specific fields
                switch (type)
                {
                    case "FuelMotorcycle":
                    case "ElectricMotorcycle":
                        ((Motorcycle)newVehicle).LicenseType = (Enums.ELicenseType)Enum.Parse(typeof(Enums.ELicenseType), parts[8]);
                        ((Motorcycle)newVehicle).EngineVolume = int.Parse(parts[9]);

                        break;

                    case "FuelCar":
                    case "ElectricCar":
                        ((Car)newVehicle).CarColor = (Enums.ECarColor)Enum.Parse(typeof(Enums.ECarColor), parts[8]);
                        ((Car)newVehicle).NumberOfDoors = (Enums.ENumberOfDoors)Enum.Parse(typeof(Enums.ENumberOfDoors), parts[9]);
                        break;

                    case "Truck":
                        ((Truck)newVehicle).IsCargoHazardousMaterial = bool.Parse(parts[8]);
                        ((Truck)newVehicle).CargoVolume = float.Parse(parts[9]);
                        break;
                }

                newVehicle.CurrentEnergyPercent = energyPercent;

                switch (newVehicle)
                {
                    case FuelCar fuelCar:
                        fuelCar.FuelInfo.AddFuel((energyPercent / 100f) * fuelCar.FuelInfo.MaxLiterInTank);
                        break;

                    case FuelMotorcycle fuelMotorcycle:
                        fuelMotorcycle.FuelInfo.AddFuel((energyPercent / 100f) * fuelMotorcycle.FuelInfo.MaxLiterInTank);
                        break;

                    case Truck truck:
                        truck.FuelInfo.AddFuel((energyPercent / 100f) * truck.FuelInfo.MaxLiterInTank);
                        break;

                    case ElectricCar electricCar:
                        electricCar.ElectricInfo.ChargingBattery((energyPercent / 100f) * electricCar.ElectricInfo.MaxBatteryTime);
                        break;

                    case ElectricMotorcycle electricMotorcycle:
                        electricMotorcycle.ElectricInfo.ChargingBattery((energyPercent / 100f) * electricMotorcycle.ElectricInfo.MaxBatteryTime);
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
                r_VehiclesInGarage[i_Vehicle.LicensePlate].VehicleStatus = EVehicleStatus.InRepair;
            }
            else
            {
                r_VehiclesInGarage.Add(i_Vehicle.LicensePlate, i_Vehicle);
            }
        }

        // Get all license numbers (optional filter)
        public List<string> GetAllLicenseNumbers(EVehicleStatus? i_Filter = null)
        {
            List<string> result = new List<string>();

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
        public void ChangeVehicleStatus(string i_LicensePlate, EVehicleStatus i_NewStatus)
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
        public void RefuelVehicle(string i_LicensePlate, EFuelType i_FuelType, float i_Amount)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            Vehicle vehicle = r_VehiclesInGarage[i_LicensePlate];
            FuelType fuelSystem = null;

            if (vehicle is FuelCar fuelCar)
            {
                fuelSystem = fuelCar.FuelInfo;
            }
            else if (vehicle is FuelMotorcycle fuelMotorcycle)
            {
                fuelSystem = fuelMotorcycle.FuelInfo;
            }
            else if (vehicle is Truck truck)
            {
                fuelSystem = truck.FuelInfo;
            }
            else
            {
                throw new ArgumentException("Vehicle is not fuel-based.");
            }

            if (fuelSystem.fuelType != i_FuelType)
            {
                throw new ArgumentException("Incorrect fuel type.");
            }

            fuelSystem.AddFuel(i_Amount);
        }

        // Recharge electric vehicle
        public void RechargeVehicle(string i_LicensePlate, float i_Minutes)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            Vehicle vehicle = r_VehiclesInGarage[i_LicensePlate];
            ElectricType electricSystem = null;

            if (vehicle is ElectricCar electricCar)
            {
                electricSystem = electricCar.ElectricInfo;
            }
            else if (vehicle is ElectricMotorcycle electricMotorcycle)
            {
                electricSystem = electricMotorcycle.ElectricInfo;
            }
            else
            {
                throw new ArgumentException("Vehicle is not electric.");
            }

            electricSystem.ChargingBattery(i_Minutes / 60f); // Convert minutes to hours
        }

        // Get full vehicle details
        public string GetVehicleDetails(string i_LicensePlate)
        {
            if (!r_VehiclesInGarage.ContainsKey(i_LicensePlate))
            {
                throw new ArgumentException("Vehicle not found.");
            }

            Vehicle v = r_VehiclesInGarage[i_LicensePlate];

            string tiresInfo = string.Join("\n", v.Tires);
            string energyInfo;
            string extraInfo = string.Empty;

            if (v is FuelCar fuelCar)
            {
                var fuel = fuelCar.FuelInfo;
                energyInfo = $"Fuel: {fuel.CurrentLiterInTank}/{fuel.MaxLiterInTank} L ({fuel.fuelType})";
                extraInfo = $"Color: {fuelCar.CarColor}\nDoors: {fuelCar.NumberOfDoors}";
            }
            else if (v is ElectricCar electricCar)
            {
                var battery = electricCar.ElectricInfo;
                energyInfo = $"Battery: {battery.CurrentBatteryTime}/{battery.MaxBatteryTime} hrs";
                extraInfo = $"Color: {electricCar.CarColor}\nDoors: {electricCar.NumberOfDoors}";
            }
            else if (v is FuelMotorcycle fuelMoto)
            {
                var fuel = fuelMoto.FuelInfo;
                energyInfo = $"Fuel: {fuel.CurrentLiterInTank}/{fuel.MaxLiterInTank} L ({fuel.fuelType})";
                extraInfo = $"License Type: {fuelMoto.LicenseType}\nEngine Volume: {fuelMoto.EngineVolume} cc";
            }
            else if (v is ElectricMotorcycle electricMoto)
            {
                var battery = electricMoto.ElectricInfo;
                energyInfo = $"Battery: {battery.CurrentBatteryTime}/{battery.MaxBatteryTime} hrs";
                extraInfo = $"License Type: {electricMoto.LicenseType}\nEngine Volume: {electricMoto.EngineVolume} cc";
            }
            else if (v is Truck truck)
            {
                var fuel = truck.FuelInfo;
                energyInfo = $"Fuel: {fuel.CurrentLiterInTank}/{fuel.MaxLiterInTank} L ({fuel.fuelType})";
                extraInfo = $"Hazardous Materials: {truck.IsCargoHazardousMaterial}\nCargo Volume: {truck.CargoVolume}";
            }
            else
            {
                energyInfo = "Unknown energy type.";
            }

            return $"Model: {v.ModelName}\n" +
                   $"License: {v.LicensePlate}\n" +
                   $"Owner: {v.OwnerName}, {v.OwnerPhone}\n" +
                   $"Status: {v.VehicleStatus}\n" +
                   $"Energy: {v.CurrentEnergyPercent}%\n" +
                   $"{energyInfo}\n" +
                   $"Tires:\n{tiresInfo}\n" +
                   $"{extraInfo}";
        }


    }
}
