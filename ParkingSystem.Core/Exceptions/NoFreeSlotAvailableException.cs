using System.Runtime.Serialization;

namespace ParkingSystem.Core.Exceptions
{
    public class NoFreeSlotAvailableException : Exception
    {
        public NoFreeSlotAvailableException()
        {
        }
    }
}