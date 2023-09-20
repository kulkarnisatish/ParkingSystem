using ParkingSystem.Core.Domain;
using ParkingSystem.Core.Exceptions;
using ParkingSystem.Infra.Data;
using ParkingSystem.Core.Service;
using ParkingSystem.Core.Service.Impl;

namespace ParkingSystemTests
{
    public class ParkingSpaceTests
    {
        List<Vehicle> hatchBackVehicles = new List<Vehicle>();
        List<Vehicle> sedanVehicles = new List<Vehicle>();
        List<Vehicle> suvVehicles = new List<Vehicle>();
        public ParkingSpaceTests()
        {
            CreateTestData();
        }

        private void CreateTestData()
        {
            for(int i=0;i<10;i++)
            {
                var vehicle = new Vehicle();
                vehicle.VehicleType = VehicleTypes.HATCHBACK;
                vehicle.RegistrationNumber = $"MH-14-100{i}";
                hatchBackVehicles.Add(vehicle);
            }

            for (int i = 0; i < 10; i++)
            {
                var vehicle = new Vehicle();
                vehicle.VehicleType = VehicleTypes.SUV;
                vehicle.RegistrationNumber = $"MH-14-200{i}";
                suvVehicles.Add(vehicle);
            }

            for (int i = 0; i < 10; i++)
            {
                var vehicle = new Vehicle();
                vehicle.VehicleType = VehicleTypes.SEDAN;
                vehicle.RegistrationNumber = $"MH-14-300{i}";
                sedanVehicles.Add(vehicle);
            }
        }


        [Fact]
        public void AddFixedParkingSlotForType_CannotAddSlotsMorethanCapacity()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            Assert.Throws<ParkingCapacityFullException>(() => parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30));
        }

        [Fact]
        public void AddFixedParkingSlotForType_CannotAddSlotsWithoutParkingArea()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            Assert.Throws<ParkingAreaNotCreatedException>(() => parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30));
        }

        [Fact]
        public void AddFixedParkingSlotForType_WithCorrectSlotTypeAndCount()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);
        }


        [Fact]
        public void Entry_ThrowsExceptionWhenParkingIsFull()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(3);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 3);
            parkingSpace.Entry(hatchBackVehicles[1]);
            parkingSpace.Entry(hatchBackVehicles[2]);
            parkingSpace.Entry(hatchBackVehicles[3]);
            Assert.Throws<NoFreeSlotAvailableException>(() => parkingSpace.Entry(hatchBackVehicles[4]));
        }

        [Fact]
        public void Entry_Park5SEDANS3SUV5HB_Positive()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(hatchBackVehicles[1]);
            parkingSpace.Entry(hatchBackVehicles[2]);
            parkingSpace.Entry(hatchBackVehicles[3]);
            parkingSpace.Entry(hatchBackVehicles[4]);
            parkingSpace.Entry(suvVehicles[1]);
            parkingSpace.Entry(sedanVehicles[1]);
            parkingSpace.Entry(suvVehicles[2]);
            parkingSpace.Entry(sedanVehicles[2]);
            parkingSpace.Entry(sedanVehicles[3]);
            parkingSpace.Entry(sedanVehicles[4]);

            slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(87, slots.Count);
            Assert.Equal(45, slots.Count((slot)=> slot.parkingSlotType == ParkingSlotTypes.SMALL));
            Assert.Equal(25, slots.Count((slot) => slot.parkingSlotType == ParkingSlotTypes.MEDIUM));
            Assert.Equal(17, slots.Count((slot) => slot.parkingSlotType == ParkingSlotTypes.LARGE));
        }


        [Fact]
        public void Exit_Park5SEDANS3SUV5HBExit3SEDAN2SUV4HB()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);

            parkingSpace.Exit(hatchBackVehicles[0]);

            parkingSpace.Entry(hatchBackVehicles[1]);
            parkingSpace.Entry(hatchBackVehicles[2]);
            parkingSpace.Entry(hatchBackVehicles[3]);
            parkingSpace.Entry(hatchBackVehicles[4]);
            parkingSpace.Entry(suvVehicles[1]);
            parkingSpace.Entry(sedanVehicles[1]);

            parkingSpace.Exit(hatchBackVehicles[1]);
            parkingSpace.Exit(hatchBackVehicles[2]);
            parkingSpace.Exit(hatchBackVehicles[3]);
            parkingSpace.Exit(suvVehicles[0]);

            parkingSpace.Entry(suvVehicles[2]);

            parkingSpace.Exit(suvVehicles[1]);

            parkingSpace.Entry(sedanVehicles[2]);
            parkingSpace.Entry(sedanVehicles[3]);
            parkingSpace.Entry(sedanVehicles[4]);

            parkingSpace.Exit(sedanVehicles[0]);
            parkingSpace.Exit(sedanVehicles[1]);
            parkingSpace.Exit(sedanVehicles[2]);

            slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(96, slots.Count);
            Assert.Equal(49, slots.Count((slot) => slot.parkingSlotType == ParkingSlotTypes.SMALL));
            Assert.Equal(28, slots.Count((slot) => slot.parkingSlotType == ParkingSlotTypes.MEDIUM));
            Assert.Equal(19, slots.Count((slot) => slot.parkingSlotType == ParkingSlotTypes.LARGE));
        }

    }
}