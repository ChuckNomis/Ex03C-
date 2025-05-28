using System;
using System.Collections.Generic;
using System.Security.Policy;
using Ex03.GarageLogic;
using static Ex03.GarageLogic.Enums;
using static Ex03.GarageLogic.ValueRangeException;



namespace Ex03.ConsoleUI
{
    public class Program
    {
        private static GarageManager s_Garage = new GarageManager();
        private static void LoadVehiclesFromFile()
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
            bool m_exit = false;

            while (!m_exit)
            {
                printMainMenu();
                string m_choice = Console.ReadLine();

                switch (m_choice)
                {
                    case "1":
                        LoadVehiclesFromFile();
                        break;
                    case "2":
                        InsertNewVehicle();
                        break;
                    case "3":
                        ListLicenseNumbers();
                        break;
                    case "4":
                        ChangeVehicleStatus();
                        break;
                    case "5":
                        InflateTires();
                        break;
                    case "6":
                        RefuelVehicle();
                        break;
                    case "7":
                        RechargeVehicle();
                        break;
                    case "8":
                        ShowVehicleDetails();
                        break;
                    case "0":
                        m_exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid m_input. Try again.");
                        break;
                }
                Console.WriteLine("\nPress Enter to return to menu...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void InsertNewVehicle()
        {
            string m_license;
            while (true)
            {
                Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                m_license = Console.ReadLine();

                try
                {
                    ValidateLicensePlate(m_license);
                    break;
                }
                catch (InvalidLicensePlateException ex)
                {
                    Console.WriteLine("License plate error: " + ex.Message);
                }
            }


            if (s_Garage.ContainsVehicle(m_license))
            {
                s_Garage.ChangeVehicleStatus(m_license, eVehicleStatus.InRepair);
                Console.WriteLine("Vehicle already in garage. Status changed to 'InRepair'.");
                return;
            }

            Console.WriteLine("Select vehicle type:");
            List<string> types = VehicleCreator.SupportedTypes;
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {types[i]}");
            }

            int    m_typeIndex = int.Parse(Console.ReadLine()) - 1;
            string m_selectedType = types[m_typeIndex];

            Console.Write("Enter m_model name: ");
            string m_model = Console.ReadLine();

            Vehicle vehicle = VehicleCreator.CreateVehicle(m_selectedType, m_license, m_model);

            while (true)
            {
                try
                {
                    Console.Write("Enter current energy m_percent (0-100): ");
                    string input = Console.ReadLine();

                    if (!float.TryParse(input, out float o_currentPercent))
                    {
                        throw new FormatException("Energy m_percent must be a number.");
                    }

                    if (o_currentPercent < 0 || o_currentPercent > 100)
                    {
                        throw new ValueRangeException(0, 100);
                    }

                    vehicle.CurrentEnergyPercent = o_currentPercent;
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

            while (true)
            {
                Console.Write("Enter owner phone (format: xxx-xxxxxxx): ");
                string phone = Console.ReadLine();

                try
                {
                    ValidatePhoneNumber(phone);
                    vehicle.OwnerPhone = phone;
                    break;
                }
                catch (InvalidPhoneNumberException ex)
                {
                    Console.WriteLine("Phone number error: " + ex.Message);
                }
            }

            Console.Write("Enter tire manufacturer: ");
            string manufacturer = Console.ReadLine();

            float m_pressure = 0;
            while (true)
            {
                try
                {
                    Console.Write("Enter tire m_pressure for all wheels (positive number): ");
                    string input = Console.ReadLine();
                    if (!float.TryParse(input, out m_pressure))
                    {
                        throw new FormatException("Tire m_pressure must be a number.");
                    }
                    float maxPressure = vehicle.Tires[0].MaxTirePressure;
                    if (m_pressure <= 0 || m_pressure > maxPressure)
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
                float toInflate = m_pressure - tire.CurrentTirePressure;
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
                    Console.Write("Enter m_license type (A, A2, AB, B2): ");
                    string input = Console.ReadLine();

                    if (Enum.TryParse(input, true, out eLicenseType licenseType) &&
                        Enum.IsDefined(typeof(eLicenseType), licenseType))
                    {
                        motorcycle.LicenseType = licenseType;
                        break;
                    }

                    Console.WriteLine("Invalid m_input. Please enter one of: A, A2, AB, B2.");
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
                    if (Enum.TryParse(input, true, out eCarColor color) &&
                        Enum.IsDefined(typeof(eCarColor), color))
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
                    if (Enum.TryParse(input, true, out eNumberOfDoors doors) &&
                        Enum.IsDefined(typeof(eNumberOfDoors), doors))
                    {
                        car.NumberOfDoors = doors;
                        break;
                    }
                    Console.WriteLine("Invalid m_input. Please enter a number: 2, 3, 4, or 5.");
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
                    Console.WriteLine("Invalid m_input. Please enter 'true' or 'false'.");
                }
                while (true)
                {
                    try
                    {
                        Console.Write("Enter cargo volume (positive number): ");
                        string m_input = Console.ReadLine();
                        if (!float.TryParse(m_input, out float volume))
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

            float m_percent = vehicle.CurrentEnergyPercent;

            switch (vehicle)
            {
                case FuelCar fuelCar:
                    fuelCar.FuelInfo.SetFuelLevelFromPercent(m_percent);
                    break;

                case FuelMotorcycle fuelMotorcycle:
                    fuelMotorcycle.FuelInfo.SetFuelLevelFromPercent(m_percent);
                    break;

                case Truck truck:
                    truck.FuelInfo.SetFuelLevelFromPercent(m_percent);
                    break;

                case ElectricCar electricCar:
                    electricCar.ElectricInfo.SetBatteryLevelFromPercent(m_percent);
                    break;

                case ElectricMotorcycle electricMotorcycle:
                    electricMotorcycle.ElectricInfo.SetBatteryLevelFromPercent(m_percent);
                    break;
            }
            // Add to garage
            s_Garage.AddOrUpdateVehicle(vehicle);
            Console.WriteLine("Vehicle added successfully.");
        }

        private static void ListLicenseNumbers()
        {
            Console.WriteLine("Filter by status?");
            Console.WriteLine("1. All");
            Console.WriteLine("2. InRepair");
            Console.WriteLine("3. Repaired");
            Console.WriteLine("4. Paid");

            string m_choice = Console.ReadLine();
            List<string> licenses;

            switch (m_choice)
            {
                case "1":
                    licenses = s_Garage.GetAllLicenseNumbers();
                    break;
                case "2":
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.InRepair);
                    break;
                case "3":
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.Fixed);
                    break;
                case "4":
                    licenses = s_Garage.GetAllLicenseNumbers(eVehicleStatus.Paid);
                    break;
                default:
                    Console.WriteLine("Invalid m_choice. Showing all vehicles.");
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
        private static void ChangeVehicleStatus()
        {
            string m_license;
            while (true)
            {
                Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                m_license = Console.ReadLine();

                try
                {
                    ValidateLicensePlate(m_license);
                    break;
                }
                catch (InvalidLicensePlateException ex)
                {
                    Console.WriteLine("License plate error: " + ex.Message);
                }
            }
            if (!s_Garage.ContainsVehicle(m_license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }
            Console.WriteLine("Select new status:");
            Console.WriteLine("1. InRepair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Paid");

            string m_choice = Console.ReadLine();
            eVehicleStatus newStatus;

            switch (m_choice)
            {
                case "1":
                    newStatus = eVehicleStatus.InRepair;
                    break;
                case "2":
                    newStatus = eVehicleStatus.Fixed;
                    break;
                case "3":
                    newStatus = eVehicleStatus.Paid;
                    break;
                default:
                    Console.WriteLine("Invalid status selected.");
                    return;
            }
            s_Garage.ChangeVehicleStatus(m_license, newStatus);
            Console.WriteLine("Vehicle status updated successfully.");
        }

        private static void InflateTires()
        {
            string m_license;
            while (true)
            {
                Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                m_license = Console.ReadLine();

                try
                {
                    ValidateLicensePlate(m_license);
                    break;
                }
                catch (InvalidLicensePlateException ex)
                {
                    Console.WriteLine("License plate error: " + ex.Message);
                }
            }
            if (!s_Garage.ContainsVehicle(m_license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }
            try
            {
                s_Garage.InflateTiresToMax(m_license);
                Console.WriteLine("All tires were successfully inflated to their maximum m_pressure.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inflating tires: {ex.Message}");
            }
        }
        private static void RefuelVehicle()
        {
            try
            {
                string m_license;
                while (true)
                {
                    Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                    m_license = Console.ReadLine();

                    try
                    {
                        ValidateLicensePlate(m_license);
                        break;
                    }
                    catch (InvalidLicensePlateException ex)
                    {
                        Console.WriteLine("License plate error: " + ex.Message);
                    }
                }

                Console.WriteLine("Enter fuel type (Octan95, Octan96, Octan98, Soler):");
                string fuelTypeInput = Console.ReadLine();
                if (!Enum.TryParse(fuelTypeInput, true, out eFuelType fuelType))
                {
                    throw new ArgumentException("Invalid fuel type.");
                }

                Console.Write("Enter amount of fuel to add (liters): ");
                string m_input = Console.ReadLine();
                if (!float.TryParse(m_input, out float amount) || amount <= 0)
                {
                    throw new ValueRangeException(0.01f, float.MaxValue);
                }

                s_Garage.RefuelVehicle(m_license, fuelType, amount);
                Console.WriteLine("Vehicle refueled successfully.");
            }
            catch (ValueRangeException ex)
            {
                Console.WriteLine($"Value out of range: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid m_input: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void RechargeVehicle()
        {
            string m_license;
            while (true)
            {
                Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                m_license = Console.ReadLine();

                try
                {
                    ValidateLicensePlate(m_license);
                    break;
                }
                catch (InvalidLicensePlateException ex)
                {
                    Console.WriteLine("License plate error: " + ex.Message);
                }
            }


            if (!s_Garage.ContainsVehicle(m_license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }
            try
            {
                Console.Write("Enter number of minutes to charge: ");
                float minutes = float.Parse(Console.ReadLine());

                s_Garage.RechargeVehicle(m_license, minutes);
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
        private static void ShowVehicleDetails()
        {
            string m_license;
            while (true)
            {
                Console.Write("Enter m_license plate (format: xx-xxx-xx): ");
                m_license = Console.ReadLine();

                try
                {
                    ValidateLicensePlate(m_license);
                    break;
                }
                catch (InvalidLicensePlateException ex)
                {
                    Console.WriteLine("License plate error: " + ex.Message);
                }
            }
            if (!s_Garage.ContainsVehicle(m_license))
            {
                Console.WriteLine("Vehicle not found.");
                return;
            }

            try
            {
                string details = s_Garage.GetVehicleDetails(m_license);
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
            Console.WriteLine("3. List vehicle m_license numbers");
            Console.WriteLine("4. Change vehicle status");
            Console.WriteLine("5. Inflate tires to max");
            Console.WriteLine("6. Refuel a vehicle");
            Console.WriteLine("7. Recharge a vehicle");
            Console.WriteLine("8. Show full vehicle details");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }
        private static void ValidateLicensePlate(string i_LicensePlate)
        {
            if (string.IsNullOrEmpty(i_LicensePlate) || i_LicensePlate.Length != 9)
            {
                throw new InvalidLicensePlateException(i_LicensePlate);
            }

            if (i_LicensePlate[2] != '-' || i_LicensePlate[6] != '-')
            {
                throw new InvalidLicensePlateException(i_LicensePlate);
            }

            for (int charOfLicense = 0; charOfLicense < i_LicensePlate.Length; charOfLicense++)
            {
                if (charOfLicense == 2 || charOfLicense == 6)
                {
                    continue;
                }

                if (!char.IsDigit(i_LicensePlate[charOfLicense]))
                {
                    throw new InvalidLicensePlateException(i_LicensePlate);
                }
            }
        }
        private static void ValidatePhoneNumber(string i_PhoneNumber)
        {
            if (string.IsNullOrEmpty(i_PhoneNumber) || i_PhoneNumber.Length != 11)
            {
                throw new InvalidPhoneNumberException(i_PhoneNumber);
            }

            if (i_PhoneNumber[3] != '-')
            {
                throw new InvalidPhoneNumberException(i_PhoneNumber);
            }

            for (int num = 0; num < i_PhoneNumber.Length; num++)
            {
                if (num == 3)
                {
                    continue;
                }

                if (!char.IsDigit(i_PhoneNumber[num]))
                {
                    throw new InvalidPhoneNumberException(i_PhoneNumber);
                }
            }
        }
    }
}

