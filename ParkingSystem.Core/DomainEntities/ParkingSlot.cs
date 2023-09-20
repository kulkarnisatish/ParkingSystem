using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Domain
{

    public enum ParkingSlotTypes { SMALL, MEDIUM, LARGE };
    public class ParkingSlot
    {
        
        public ParkingSlotTypes parkingSlotType { get; set; }
        public int Id { get; set; }
        public Vehicle? ParkedVehicle { get; private set; } = null;

        public bool IsEmpty() {
            return ParkedVehicle == null;
        }

        public void Occupy(Vehicle vehicle)
        {
            ParkedVehicle = vehicle;
        }

        public void UnOccupy(Vehicle vehicle)
        {
            ParkedVehicle = null;
        }

    }
    

}
