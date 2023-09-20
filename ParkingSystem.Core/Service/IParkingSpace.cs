using ParkingSystem.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Service
{
    public interface IParkingManagement
    {
        public bool CreateParkingArea(int capacity);
        public void AddSingleParkingSlot(ParkingSlot slot);
        public void AddFixedCountParkingSlotForType(ParkingSlotTypes slotType, int numberOfSlots);
        public List<ParkingSlot> GetAllFreeParkingSlots();
        public List<ParkingSlot> GetAllParkingSlots();
    }

    public interface IParkingOperation
    {
        public bool Entry(Vehicle vehicle);
        public bool Exit(Vehicle vehicle);        
    }

    public interface IParkingSpace : IParkingManagement, IParkingOperation
    { }
}
