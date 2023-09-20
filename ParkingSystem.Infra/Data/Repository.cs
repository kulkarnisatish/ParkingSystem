using ParkingSystem.Core.Domain;
using ParkingSystem.Core.Exceptions;
using ParkingSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Infra.Data
{
    public class Repository : IRepository
    {
        private SortedDictionary<int, ParkingSlot> dataStore = new SortedDictionary<int, ParkingSlot>();
        private Dictionary<string, int> lookUpdataStore = new Dictionary<string, int>(); //can be used as cache
        private ParkingArea? parkingArea; //this will be stored in DB
        public void AddParkingSlot(ParkingSlot slot, int numberOfSlots)
        {
            int latestId = 0;

            if (dataStore.Count > 0)
            {
                //find max id of current data store
                latestId = dataStore.Keys.Max();
            }

            slot.Id = ++latestId;
            dataStore.Add(slot.Id, slot);
        }

        public List<ParkingSlot> GetAllParkingSlots()
        {
            return dataStore.Values.ToList();
        }

        public List<ParkingSlot> GetAllFreeParkingSlots()
        {
            return dataStore.Values.ToList().FindAll((slot) => slot.IsEmpty() == true);
        }

        public bool Park(int slotId, Vehicle vehicle)
        {
            dataStore[slotId].Occupy(vehicle);
            lookUpdataStore[vehicle.RegistrationNumber] = slotId;
            return true;
        }

        public bool UnPark(Vehicle vehicle)
        {
            dataStore[lookUpdataStore[vehicle.RegistrationNumber]].UnOccupy(vehicle);
            lookUpdataStore.Remove(vehicle.RegistrationNumber);
            return true;
        }

        public void AddParkingArea(int capacity)
        {
            parkingArea = new ParkingArea();
            parkingArea.Capacity = capacity;
            //Save this info in DB
            //Also we will link all paringslots to this area
        }

        public ParkingArea GetParkingArea()
        {
            return parkingArea;
        }
    }
}
