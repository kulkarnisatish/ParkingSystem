using ParkingSystem.Core.Domain;
using ParkingSystem.Core.Service;
using ParkingSystem.Core.Service.Impl;
using ParkingSystem.Infra.Data;

namespace ParkingSystemTests
{
    public class GetFirstFreeMatchSlotPolicyTests
    {
        List<Vehicle> hatchBackVehicles = new List<Vehicle>();
        List<Vehicle> sedanVehicles = new List<Vehicle>();
        List<Vehicle> suvVehicles = new List<Vehicle>();
        public GetFirstFreeMatchSlotPolicyTests()
        {
            CreateTestData();
        }
        private void CreateTestData()
        {
            for (int i = 0; i < 10; i++)
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
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForSmallSlots()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(hatchBackVehicles[1]);
            parkingSpace.Entry(hatchBackVehicles[2]);
            parkingSpace.Entry(hatchBackVehicles[3]);
            parkingSpace.Entry(hatchBackVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(hatchBackVehicles[0].RegistrationNumber, slots[0].ParkedVehicle.RegistrationNumber);
            Assert.Equal(hatchBackVehicles[4].RegistrationNumber, slots[4].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForSmallSlots_AfterExits()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(hatchBackVehicles[1]);
            parkingSpace.Exit(hatchBackVehicles[0]);
            parkingSpace.Entry(hatchBackVehicles[2]);
            parkingSpace.Exit(hatchBackVehicles[1]);
            parkingSpace.Entry(hatchBackVehicles[3]);
            parkingSpace.Entry(hatchBackVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(hatchBackVehicles[2].RegistrationNumber, slots[0].ParkedVehicle.RegistrationNumber);
            Assert.Equal(hatchBackVehicles[3].RegistrationNumber, slots[1].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForMediumSlots()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(sedanVehicles[1]);
            parkingSpace.Entry(sedanVehicles[2]);
            parkingSpace.Entry(sedanVehicles[3]);
            parkingSpace.Entry(sedanVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(sedanVehicles[0].RegistrationNumber, slots[50].ParkedVehicle.RegistrationNumber);
            Assert.Equal(sedanVehicles[4].RegistrationNumber, slots[54].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForMediumSlots_AfterExits()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(sedanVehicles[0]); // @51
            parkingSpace.Entry(sedanVehicles[1]); //@52
            parkingSpace.Exit(sedanVehicles[0]); // empty @51
            parkingSpace.Entry(sedanVehicles[2]); // should be @51
            parkingSpace.Exit(sedanVehicles[1]); // empty @52
            parkingSpace.Entry(sedanVehicles[3]); // should be @52
            parkingSpace.Entry(sedanVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(sedanVehicles[2].RegistrationNumber, slots[50].ParkedVehicle.RegistrationNumber);
            Assert.Equal(sedanVehicles[3].RegistrationNumber, slots[51].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForLargeSlots()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(suvVehicles[1]);
            parkingSpace.Entry(suvVehicles[2]);
            parkingSpace.Entry(suvVehicles[3]);
            parkingSpace.Entry(suvVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(suvVehicles[0].RegistrationNumber, slots[80].ParkedVehicle.RegistrationNumber);
            Assert.Equal(suvVehicles[3].RegistrationNumber, slots[83].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsFirstAvailableSlotForLargeSlots_AfterExits()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(100);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 50);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 30);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 20);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            Assert.Equal(100, slots.Count);

            parkingSpace.Entry(suvVehicles[0]); //@81
            parkingSpace.Entry(suvVehicles[1]); //@82
            parkingSpace.Exit(suvVehicles[0]); // empty @81
            parkingSpace.Entry(suvVehicles[2]); //@81
            parkingSpace.Exit(suvVehicles[1]); //@ empty 82
            parkingSpace.Entry(suvVehicles[3]); //@82
            parkingSpace.Entry(suvVehicles[4]);

            slots = parkingSpace.GetAllParkingSlots();
            Assert.Equal(suvVehicles[2].RegistrationNumber, slots[80].ParkedVehicle.RegistrationNumber);
            Assert.Equal(suvVehicles[3].RegistrationNumber, slots[81].ParkedVehicle.RegistrationNumber);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsNegativeValueWhenFreeSlotisNotAvailableForSUV()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 1);
            parkingSpace.Entry(suvVehicles[0]); 

            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.SUV);
            
            Assert.Equal(-1, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsNegativeValueWhenFreeSlotisNotAvailableForSedan()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(suvVehicles[1]);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.SEDAN);

            Assert.Equal(-1, slotId);
        }


        [Fact]
        public void GetFreeSlotId_ReturnsNegativeOneWhenNoSlotAvldlForHatchback()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(sedanVehicles[1]);
            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(suvVehicles[1]);

            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.HATCHBACK);

            Assert.Equal(-1, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsMediumSlotWhenSmallSlotIsNotAvlblForHatchback()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);

            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.HATCHBACK);

            Assert.Equal(2, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsNextMediumSlotWhenSmallSlotIsNotAvlblForHatchback()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.HATCHBACK);

            Assert.Equal(3, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsLargeSlotWhenSmallAdMediumSlotIsNotAvlblForHatchback()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(sedanVehicles[1]);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.HATCHBACK);

            Assert.Equal(4, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsLargeSlotWhenMediumSlotIsNotAvlblForSedan()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(sedanVehicles[1]);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.COMPACTSUV);

            Assert.Equal(4, slotId);
        }

        [Fact]
        public void GetFreeSlotId_ReturnsNegativeOneWhenMediumAndLargeSlotIsNotAvlblForSedan()
        {
            ParkingSpace parkingSpace = new ParkingSpace(new Repository(), new GetFirstFreeMatchSlotPolicy());
            parkingSpace.CreateParkingArea(5);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.SMALL, 1);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.MEDIUM, 2);
            parkingSpace.AddFixedCountParkingSlotForType(ParkingSlotTypes.LARGE, 2);
            parkingSpace.Entry(hatchBackVehicles[0]);
            parkingSpace.Entry(sedanVehicles[0]);
            parkingSpace.Entry(sedanVehicles[1]);
            parkingSpace.Entry(suvVehicles[0]);
            parkingSpace.Entry(suvVehicles[1]);
            var slots = parkingSpace.GetAllFreeParkingSlots();
            IParkingPolicy policy = new GetFirstFreeMatchSlotPolicy();
            int slotId = policy.GetFreeSlotId(slots, VehicleTypes.SEDAN);

            Assert.Equal(-1, slotId);
        }

    }
}
