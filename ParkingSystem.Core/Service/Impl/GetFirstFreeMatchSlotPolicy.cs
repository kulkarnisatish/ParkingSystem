using ParkingSystem.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSystem.Core.Service.Impl
{
    public class GetFirstFreeMatchSlotPolicy : IParkingPolicy
    {
        public int GetFreeSlotId(List<ParkingSlot> slots, VehicleTypes vehicleType)
        {
            slots.Sort((slot1, slot2) => slot1.Id.CompareTo(slot2.Id));
            switch (vehicleType)
            {
                case VehicleTypes.HATCHBACK:
                    {
                        int slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.SMALL);
                        if(slotId < 0)
                        {  // find medium slot
                            slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.MEDIUM);
                        }
                        if (slotId < 0)
                        {  // find medium slot
                            slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.LARGE);
                        }
                        return slotId;
                    }

                case VehicleTypes.SEDAN:
                case VehicleTypes.COMPACTSUV:
                    {
                        int slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.MEDIUM);
                        if (slotId < 0)
                        {  // find large slot
                            slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.LARGE);
                        }
                        return slotId;
                    }

                case VehicleTypes.SUV:
                case VehicleTypes.LARGECARS:
                    {
                        int slotId = GetSlotId(slots, vehicleType, ParkingSlotTypes.LARGE);
                        return slotId;
                    }
            }
            return -1;
        }

        private int GetSlotId(List<ParkingSlot> slots, VehicleTypes vehicleType, ParkingSlotTypes parkingSlotType)
        {
            if (slots.FirstOrDefault((slot) => slot.parkingSlotType == parkingSlotType) != null)
                return slots.FirstOrDefault((slot) => slot.parkingSlotType == parkingSlotType).Id;
            else
                return -1;
        }
    }
}
