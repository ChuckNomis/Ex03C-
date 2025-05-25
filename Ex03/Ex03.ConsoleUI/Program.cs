using System;
using System.Collections.Generic;
using Ex03.GarageLogic;
using static Ex03.GarageLogic.Enums;

namespace Ex03.ConsoleUI
{
    public class Program
    {
        private static GarageManager s_Garage = new GarageManager();
        private static void loadVehiclesFromFile()
        {
            try
            {
                string path = "Vehicles.db";
                s_Garage.LoadVehiclesFromFile(path);
                Console.WriteLine("Vehicles loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading vehicles: {ex.Message}");
            }
        }


        public static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                printMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        loadVehiclesFromFile();
                        break;
                    case "2":
                        insertNewVehicle();
                        break;
                    case "3":
                        listLicenseNumbers();
                        break;
                    case "4":
                        changeVehicleStatus();
                        break;
                    case "5":
                        inflateTires();
                        break;
                    case "6":
                        refuelVehicle();
                        break;
                    case "7":
                        rechargeVehicle();
                        break;
                    case "8":
                        showVehicleDetails();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }

                Console.WriteLine("\nPress Enter to return to menu...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void insertNewVehicle()
        {
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (s_Garage.ContainsVehicle(license))
            {
                s_Garage.ChangeVehicleStatus(license, EVehicleStatus.InRepair);
                Console.WriteLine("Vehicle already in garage. Status changed to 'InRepair'.");
                return;
            }

            Console.WriteLine("Select vehicle type:");
            List<string> types = VehicleCreator.SupportedTypes;
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            int typeIndex = int.Parse(Console.ReadLine()) - 1;
            string selectedType = types[typeIndex];

            Console.Write("Enter model name: ");
            string model = Console.ReadLine();

            Vehicle vehicle = VehicleCreator.CreateVehicle(selectedType, license, model);

            while (true)
            {
                try
                {
                    Console.Write("Enter current energy percent (0-100): ");
                    string input = Console.ReadLine();

                    float currentPercent;
                    if (!float.TryParse(input, out currentPercent)) 
                    {
                        throw new FormatException("Energy percent must be a number.");
                    }

                    if (currentPercent < 0 || currentPercent > 100)
                    {
                        throw new ValueRangeException(0, 100);
                    }

                    vehicle.CurrentEnergyPercent = currentPercent;
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Format error: " + ex.Message);
                }
                catch (ValueRangeException ex)
                {
                    Console.WriteLine("Range error: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Argument error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected error: " + ex.Message);
                }
            }

            Console.Write("Enter owner name: ");
            vehicle.OwnerName = Console.ReadLine();

            Console.Write("Enter owner phone: ");
            vehicle.OwnerPhone = Console.ReadLine();

            Console.Write("Enter tire manufacturer: ");
            string manufacturer = Console.ReadLine();

            float pressure = 0;
            while (true)
            {
                try
                {
                    Console.Write("Enter tire pressure for all wheels (positive number): ");
                    string input = Console.ReadLine();
                    if (!float.TryParse(input, out pressure))
                    {
                        throw new FormatException("Tire pressure must be a number.");
                    }
                    float maxPressure = vehicle.Tires[0].MaxTirePressure;
                    if (pressure <= 0 || pressure > maxPressure)
                    {
                        throw new ValueRangeException(0.01f, maxPressure);
                    }
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Format error: " + ex.Message);
                }
                catch (ValueRangeException ex)
                {
                    Console.WriteLine("Range error: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Argument error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unexpected error: " + ex.Message);
                }
            }

            foreach (var tire in vehicle.Tires)
            {
                tire.ManufacturerName = manufacturer;
                float toInflate = pressure - tire.CurrentTirePressure;
                if (toInflate > 0)
                {
                    tire.TireInflating(toInflate);
                }
            }

            // Type specific inputs
            if (vehicle is Motorcycle motorcycle)
            {
                while (true)
                {
                    Console.Write("Enter license type (A, A2, AB, B2): ");
                    string input = Console.ReadLine();

                    if (Enum.TryParse(input, true, out ELicenseType licenseType) &&
                        Enum.IsDefined(typeof(ELicenseType), licenseType))
                    {
                        motorcycle.LicenseType = licenseType;
                        break;
                    }

                    Console.WriteLine("Invalid input. Please enter one of: A, A2, AB, B2.");
                }


                while (true)
                {
                    try
                    {
                        Console.Write("Enter engine volume (positive number): ");
                        string input = Console.ReadLine();
                        if (!int.TryParse(input, out int engineVolume))
                        {
                            Console.WriteLine("Invalid format. Please enter a whole number.");
                            continue;
                        }

                        if (engineVolume <= 0)
                        {
                            throw new ValueRangeException(1, int.MaxValue);
                        }

                        motorcycle.EngineVolume = engineVolume;
                        break;
                    }
                    catch (ValueRangeException ex)
                    {
                        Console.WriteLine($"Range error: {ex.Message}");
                    }
                }
            }
            else if (vehicle is Car car)
            {
                while (true)
                {
                    Console.Write("Enter color (Yellow, Black, White, Silver): ");
                    string input = Console.ReadLine();
                    if (Enum.TryParse(input, true, out ECarColor color) &&
                        Enum.IsDefined(typeof(ECarColor), color))
                    {
                        car.CarColor = color;
                        break;
                    }
                    Console.WriteLine("Invalid color. Please enter one of: Yellow, Black, White, Silver.");
                }

                while (true)
                {
                    Console.Write("Enter number of doors (2, 3, 4, 5): ");
                    string input = Console.ReadLine();
                    if (Enum.TryParse(input, true, out ENumberOfDoors doors) &&
                        Enum.IsDefined(typeof(ENumberOfDoors), doors))
                    {
                        car.NumberOfDoors = doors;
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a number: 2, 3, 4, or 5.");
                }
            }
            else if (vehicle is Truck truck)
            {
                while (true)
                {
                    Console.Write("Is it carrying hazardous materials? (true/false): ");
                    string input = Console.ReadLine();
                    if (bool.TryParse(input, out bool isHazardous))
                    {
                        truck.IsCargoHazardousMaterial = isHazardous;
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                }

                while (true)
                {
                    try
                    {
                        Console.Write("Enter cargo volume (positive number): ");
                        string input = Console.ReadLine();
                        if (!float.TryParse(input, out float volume))
                        {
                            Console.WriteLine("Invalid format. Please enter a number.");
                            continue;
                        }

                        if (volume <= 0)
                        {
                            throw new ValueRangeException(0.01f, float.MaxValue);
                        }

                        truck.CargoVolume = volume;
                        break;
                    }
                    catch (ValueRangeException ex)
                    {
                        Console.WriteLine($"Range error: {ex.Message}");
                    }
                }
            }


            float percent = vehicle.CurrentEnergyPercent;

            switch (vehicle)
            {
                case FuelCar fuelCar:
                    fuelCar.FuelInfo.SetFuelLevelFromPercent(percent);
                    break;

                case FuelMotorcycle fuelMotorcycle:
                    fuelMotorcycle.FuelInfo.SetFuelLevelFromPercent(percent);
                    break;

                case Truck truck:
                    truck.FuelInfo.SetFuelLevelFromPercent(percent);
                    break;

                case ElectricCar electricCar:
                    electricCar.ElectricInfo.SetBatteryLevelFromPercent(percent);
                    break;

                case ElectricMotorcycle electricMotorcycle:
                    electricMotorcycle.ElectricInfo.SetBatteryLevelFromPercent(percent);
                    break;
            }


            // Add to garage
            s_Garage.AddOrUpdateVehicle(vehicle);
            Console.WriteLine("Vehicle added successfully.");
        }

        private static void listLicenseNumbers()
        {
            Console.WriteLine("Filter by status?");
            Console.WriteLine("1. All");
            Console.WriteLine("2. InRepair");
            Console.WriteLine("3. Repaired");
            Console.WriteLine("4. Paid");

            string choice = Console.ReadLine();
            List<string> licenses;

            switch (choice)
            {
                case "1":
                    licenses = s_Garage.GetAllLicenseNumbers();
                    break;
                case "2":
                    licenses = s_Garage.GetAllLicenseNumbers(EVehicleStatus.InRepair);
                    break;
                case "3":
                    licenses = s_Garage.GetAllLicenseNumbers(EVehicleStatus.Fixed);
                    break;
                case "4":
                    licenses = s_Garage.GetAllLicenseNumbers(EVehicleStatus.Paid);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Showing all vehicles.");
                    licenses = s_Garage.GetAllLicenseNumbers();
                    break;
            }

            if (licenses.Count == 0)
            {
                Console.WriteLine("No vehicles found.");
            }
            else
            {
                Console.WriteLine("License numbers:");
                foreach (string license in licenses)
                {
                    Console.WriteLine($"- {license}");
                }
            }
        }

        private static void changeVehicleStatus()
        {
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (!s_Garage.ContainsVehicle(license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            Console.WriteLine("Select new status:");
            Console.WriteLine("1. InRepair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Paid");

            string choice = Console.ReadLine();
            EVehicleStatus newStatus;

            switch (choice)
            {
                case "1":
                    newStatus = EVehicleStatus.InRepair;
                    break;
                case "2":
                    newStatus = EVehicleStatus.Fixed;
                    break;
                case "3":
                    newStatus = EVehicleStatus.Paid;
                    break;
                default:
                    Console.WriteLine("Invalid status selected.");
                    return;
            }

            s_Garage.ChangeVehicleStatus(license, newStatus);
            Console.WriteLine("Vehicle status updated successfully.");
        }

        private static void inflateTires()
        {
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (!s_Garage.ContainsVehicle(license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }
            try
            {
                s_Garage.InflateTiresToMax(license);
                Console.WriteLine("All tires were successfully inflated to their maximum pressure.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inflating tires: {ex.Message}");
            }
        }

        private static void refuelVehicle()
        {
            try
            {
                Console.Write("Enter license plate: ");
                string license = Console.ReadLine();

                Console.WriteLine("Enter fuel type (Octan95, Octan96, Octan98, Soler):");
                string fuelTypeInput = Console.ReadLine();
                if (!Enum.TryParse(fuelTypeInput, true, out EFuelType fuelType))
                {
                    throw new ArgumentException("Invalid fuel type.");
                }

                Console.Write("Enter amount of fuel to add (liters): ");
                string input = Console.ReadLine();
                if (!float.TryParse(input, out float amount) || amount <= 0)
                {
                    throw new ValueRangeException(0.01f, float.MaxValue);
                }

                s_Garage.RefuelVehicle(license, fuelType, amount);
                Console.WriteLine("Vehicle refueled successfully.");
            }
            catch (ValueRangeException ex)
            {
                Console.WriteLine($"Value out of range: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void rechargeVehicle()
        {
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (!s_Garage.ContainsVehicle(license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            try
            {
                Console.Write("Enter number of minutes to charge: ");
                float minutes = float.Parse(Console.ReadLine());

                s_Garage.RechargeVehicle(license, minutes);
                Console.WriteLine("Vehicle charged successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number format.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Argument error: {ex.Message}");
            }
            catch (ValueRangeException ex)
            {
                Console.WriteLine($"Value out of range: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void showVehicleDetails()
        {
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (!s_Garage.ContainsVehicle(license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            try
            {
                string details = s_Garage.GetVehicleDetails(license);
                Console.WriteLine("=== Vehicle Details ===");
                Console.WriteLine(details);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving details: {ex.Message}");
            }
        }

        private static void printMainMenu()
        {
            Console.WriteLine("===== Garage Manager =====");
            Console.WriteLine("1. Load vehicles from file");
            Console.WriteLine("2. Insert a new vehicle");
            Console.WriteLine("3. List vehicle license numbers");
            Console.WriteLine("4. Change vehicle status");
            Console.WriteLine("5. Inflate tires to max");
            Console.WriteLine("6. Refuel a vehicle");
            Console.WriteLine("7. Recharge a vehicle");
            Console.WriteLine("8. Show full vehicle details");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }

    }
}