using ParkingSystem.Core.Domain;
using ParkingSystem.Core.Service;
using ParkingSystem.Core.Service.Impl;
using ParkingSystem.Infra.Data;

namespace ParkingSystemUsage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("#### Parking System Management #####");
            Console.WriteLine("Enter 1 Create Parking Area");
            string input = Console.ReadLine();
            if(input != "1")
            {
                Console.WriteLine("Invalid Input");
                return;
            }
            if(input == "1")
            {
                Console.WriteLine("Enter Parking Capacity:");
                input= Console.ReadLine();
                int capacity = Convert.ToInt32(input);
                IParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
                parkingSpace.CreateParkingArea(capacity);
                Console.WriteLine("Enter Parking Type: 0-SMALL, 1-MEDIUM, 2-LARGE");
                input = Console.ReadLine();
                Console.WriteLine("Enter Number of Slots to be Created:");
                string input2 = Console.ReadLine();
                int slotCreated = Convert.ToInt32(input2);
                parkingSpace.AddFixedCountParkingSlotForType((ParkingSlotTypes)Convert.ToInt32(input), slotCreated);
                while (capacity - slotCreated != 0)
                {
                    Console.WriteLine($"There are still{capacity - slotCreated} to be created");
                    Console.WriteLine("Enter Parking Type: 0-SMALL, 1-MEDIUM, 2-LARGE");
                    input = Console.ReadLine();
                    Console.WriteLine("Enter Number of Slots to be Created:");
                    input2 = Console.ReadLine();
                    int remainSlotCreated = Convert.ToInt32(input2);
                    parkingSpace.AddFixedCountParkingSlotForType((ParkingSlotTypes)Convert.ToInt32(input), remainSlotCreated);
                    slotCreated = slotCreated+ remainSlotCreated;
                }
                Console.WriteLine("Enter 2 to switch to operator menu");
                input = Console.ReadLine();
                while (input != "2")
                {
                    input = Console.ReadLine();
                }
                while (true)
                {
                    try
                    {
                        Console.WriteLine("1. Make an entry for the vehicle 2. Make an exit for the vehicle");
                        input = Console.ReadLine();
                        if (input != "1" && input != "2")
                        {
                            Console.WriteLine("Invalid Input");
                            return;
                        }


                        if (input == "1")
                        {
                            Console.WriteLine("Enter vehicle Registration Number");
                            string regNumber = Console.ReadLine();
                            Console.WriteLine("Enter Vehicle Type: 0-Hatch back, 1-SEDAN/COMPACT SUV, 2-SUV");
                            string vehType = Console.ReadLine();
                            Vehicle v = new Vehicle()
                            {
                                RegistrationNumber = regNumber,
                                VehicleType = (VehicleTypes)Convert.ToInt32(vehType)
                            };
                            parkingSpace.Entry(v);
                        }
                        if (input == "2")
                        {
                            Console.WriteLine("Enter vehicle Registration Number");
                            string regNumber = Console.ReadLine();
                            Vehicle v = new Vehicle()
                            {
                                RegistrationNumber = regNumber
                            };
                            parkingSpace.Exit(v);
                        }
                        Console.WriteLine("Do you want to mark another entry or exit? Yes/No");
                        string reponse = Console.ReadLine();
                        if (reponse == "No")
                        {
                            break;
                        }
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
        }
    }
}