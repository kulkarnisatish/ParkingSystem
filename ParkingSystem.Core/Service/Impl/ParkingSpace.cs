using ParkingSystem.Core.Domain;
using ParkingSystem.Core.Exceptions;
using ParkingSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Service.Impl
{
    public class ParkingSpace : IParkingSpace
    {
        private readonly IRepository _repository;
        private readonly IParkingPolicy _parkingPolicy;
        public ParkingSpace(IRepository repo, IParkingPolicy parkingPolicy)
        {
            _repository = repo;
            _parkingPolicy = parkingPolicy;
        }

        public void AddFixedCountParkingSlotForType(ParkingSlotTypes slotType, int numberOfSlots)
        {
            if(_repository.GetParkingArea() == null)
            {
                throw new ParkingAreaNotCreatedException();
            }
            if (_repository.GetAllParkingSlots().Count + numberOfSlots > _repository.GetParkingArea().Capacity)
            {
                throw new ParkingCapacityFullException();
            }
            for (int i = 0; i < numberOfSlots; i++)
            {
                var slot = new ParkingSlot();
                slot.parkingSlotType = slotType;
                _repository.AddParkingSlot(slot, numberOfSlots);
            }
        }

        private int GetFreeParkingSlotId(VehicleTypes vehicleType)
        {
            // use the ParkingPolicy based on free slots and vehicleType to the slotid
            List<ParkingSlot> freeSlots = _repository.GetAllFreeParkingSlots();
            if(freeSlots.Count == 0)
            {
                throw new NoFreeSlotAvailableException();
            }
            return _parkingPolicy.GetFreeSlotId(freeSlots, vehicleType);
        }

        public List<ParkingSlot> GetAllFreeParkingSlots()
        {
            //find all free parking slots
            return _repository.GetAllFreeParkingSlots();
        }

        public List<ParkingSlot> GetAllParkingSlots()
        {
            //find all free parking slots
            return _repository.GetAllParkingSlots();
        }

        public bool Entry(Vehicle vehicle)
        {
            int slotId = GetFreeParkingSlotId(vehicle.VehicleType);
            if(slotId < 0)
            {
                throw new NoFreeSlotAvailableException();
            }
            return _repository.Park(slotId, vehicle);
        }

        public bool Exit(Vehicle vehicle)
        {   // find the slotid using vechicle reg number
            return _repository.UnPark(vehicle);
        }

        public bool CreateParkingArea(int capacity)
        {
            ParkingArea parkingArea = new ParkingArea();
            parkingArea.Capacity = capacity;
            _repository.AddParkingArea(capacity);
            return true;
        }

        public ParkingArea GetParkingArea() {
            return _repository.GetParkingArea();
        }

        void IParkingManagement.AddSingleParkingSlot(ParkingSlot slot)
        {
            throw new NotImplementedException();
        }
    }
}
