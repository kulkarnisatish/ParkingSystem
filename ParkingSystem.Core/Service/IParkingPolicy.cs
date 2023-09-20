using ParkingSystem.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Service
{
    /// <summary>
    /// This interface provides the extention point for the parking assigment mechanism.
    /// </summary>
    public interface IParkingPolicy
    {
        public int GetFreeSlotId(List<ParkingSlot> slots, VehicleTypes vehicleType);
    }
}
