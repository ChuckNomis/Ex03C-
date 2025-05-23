using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public class Program
    {
        private static GarageManager s_Garage = new GarageManager();
        private static void loadVehiclesFromFile()
        {
            try
            {
                Console.WriteLine("Enter path to Vehicles.db (or leave blank for default):");
                string path = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(path))
                {
                    path = "Vehicles.db"; // default
                }

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
                s_Garage.ChangeVehicleStatus(license, eVehicleStatus.InRepair);
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

            Console.Write("Enter current energy percent (0-100): ");
            vehicle.CurrentEnergyPercent = float.Parse(Console.ReadLine());

            Console.Write("Enter owner name: ");
            vehicle.OwnerName = Console.ReadLine();

            Console.Write("Enter owner phone: ");
            vehicle.OwnerPhone = Console.ReadLine();

            Console.Write("Enter tire manufacturer: ");
            string manufacturer = Console.ReadLine();

            Console.Write("Enter tire pressure for all wheels: ");
            float pressure = float.Parse(Console.ReadLine());

            foreach (var tire in vehicle.Tires)
            {
                tire.ManufacturerName = manufacturer;
                float toInflate = pressure - tire.CurrentPressure;
                if (toInflate > 0)
                {
                    tire.Inflate(toInflate);
                }
            }

            // === TYPE-SPECIFIC INPUTS ===
            if (vehicle is Motorcycle motorcycle)
            {
                Console.Write("Enter license type (A, A2, AB, B2): ");
                motorcycle.LicenseType = Enum.Parse<eLicenseType>(Console.ReadLine(), true);

                Console.Write("Enter engine volume: ");
                motorcycle.EngineVolume = int.Parse(Console.ReadLine());
            }
            else if (vehicle is Car car)
            {
                Console.Write("Enter color (Black, White, Silver, Yellow): ");
                car.CarColor = Enum.Parse<eCarColor>(Console.ReadLine(), true);

                Console.Write("Enter number of doors (2, 3, 4, 5): ");
                car.NumberOfDoors = Enum.Parse<eNumberOfDoors>(Console.ReadLine(), true);
            }
            else if (vehicle is Truck truck)
            {
                Console.Write("Is it carrying hazardous materials? (true/false): ");
                truck.IsCarryingHazardousMaterial = bool.Parse(Console.ReadLine());

                Console.Write("Enter cargo volume: ");
                truck.CargoVolume = float.Parse(Console.ReadLine());
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
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.InRepair);
                    break;
                case "3":
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.Repaired);
                    break;
                case "4":
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.Paid);
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
            eVehicleStatus newStatus;

            switch (choice)
            {
                case "1":
                    newStatus = eVehicleStatus.InRepair;
                    break;
                case "2":
                    newStatus = eVehicleStatus.Repaired;
                    break;
                case "3":
                    newStatus = eVehicleStatus.Paid;
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
            Console.Write("Enter license plate: ");
            string license = Console.ReadLine();

            if (!s_Garage.ContainsVehicle(license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            try
            {
                Console.WriteLine("Enter fuel type (Octan95, Octan96, Octan98, Soler):");
                string fuelTypeInput = Console.ReadLine();
                eFuelType fuelType = Enum.Parse<eFuelType>(fuelTypeInput, true);

                Console.Write("Enter amount of fuel to add (liters): ");
                float amount = float.Parse(Console.ReadLine());

                s_Garage.RefuelVehicle(license, fuelType, amount);
                Console.WriteLine("Vehicle refueled successfully.");
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