namespace ParkingSystem.Core.Domain
{
    public enum VehicleTypes { HATCHBACK, SEDAN, SUV, COMPACTSUV, LARGECARS
    }
    public class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }

        public VehicleTypes VehicleType { get; set; }
    }

}