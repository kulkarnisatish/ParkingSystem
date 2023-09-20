using ParkingSystem.Core.Domain;

namespace ParkingSystem.Core.Repositories
{
    public interface IRepository
    {
        public void AddParkingSlot(ParkingSlot slot, int numberOfSlots);
        public List<ParkingSlot> GetAllFreeParkingSlots();
        public ParkingArea GetParkingArea();
        public List<ParkingSlot> GetAllParkingSlots();
        public bool Park(int slotId, Vehicle vehicle);
        public bool UnPark(Vehicle vehicle);
        void AddParkingArea(int capacity);
    }
}